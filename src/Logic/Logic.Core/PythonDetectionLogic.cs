namespace Logic.Core
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using Interfaces.Logic;

    using Microsoft.Extensions.Logging;
    using Microsoft.Win32;

    using Models;

    /// <summary>
    /// Logic specific to detecting Python installations on the current system.
    /// </summary>
    public class PythonDetectionLogic : IPythonDetectionLogic
    {
        #region constructors and destructors

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="operatingSystemLogic">Logic to interact with the operating system.</param>
        /// <param name="logger">The logger to use.</param>
        public PythonDetectionLogic(IOperatingSystemLogic operatingSystemLogic, ILogger<PythonDetectionLogic> logger)
        {
            OperatingSystemLogic = operatingSystemLogic;
            Logger = logger;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public PythonDetectionResultModel IsPythonInstalled()
        {
            var result = new PythonDetectionResultModel
            {
                DetectedPython = false,
                PathToPython = null
            };
            var path = Environment.GetEnvironmentVariable("PATH");
            if (path != null)
            {
                // Check each directory in the PATH for python or python3 executable
                foreach (var directory in path.Split(Path.PathSeparator))
                {
                    var pythonPath = Path.Combine(directory, "python");
                    var python3Path = Path.Combine(directory, "python3");
                    if (File.Exists(pythonPath))
                    {
                        result.DetectedPython = true;
                        result.PathToPython = pythonPath;
                        return result;
                    }
                    if (File.Exists(python3Path))
                    {
                        result.DetectedPython = true;
                        result.PathToPython = python3Path;
                        return result;
                    }
                }
            }
            var operatingSystem = OperatingSystemLogic.GetRuntimeOperatingSystem();
            if (operatingSystem == OSPlatform.Windows)
            {
                // Check common directories for all drives
                var detectionResult = CheckCommonDirectories();
                if (detectionResult.DetectedPython)
                {
                    return detectionResult;
                }
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Python\PythonCore");
                if (key != null)
                {
                    return new PythonDetectionResultModel
                    {
                        DetectedPython = true,
                        PathToPython = @"SOFTWARE\Python\PythonCore"
                    };
                }
            }
            else if (operatingSystem == OSPlatform.Linux || operatingSystem == OSPlatform.OSX)
            {
                var commonPaths = new[] { "/usr/bin", "/usr/local/bin", "/opt" };
                foreach (var dir in commonPaths)
                {
                    var pythonPath = Path.Combine(dir, "python");
                    var python3Path = Path.Combine(dir, "python3");
                    if (File.Exists(pythonPath))
                    {
                        result.DetectedPython = true;
                        result.PathToPython = pythonPath;
                        return result;
                    }
                    if (File.Exists(python3Path))
                    {
                        result.DetectedPython = true;
                        result.PathToPython = python3Path;
                        return result;
                    }
                }
                return result;
            }
            return result;
        }

        /// <inheritdoc />
        public PythonTestProcessResultModel RunTestProcess(string pythonExecutable)
        {
            var result = new PythonTestProcessResultModel();
            var startInfo = new ProcessStartInfo
            {
                FileName = pythonExecutable,
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(startInfo);
            var output = process?.StandardOutput.ReadToEnd();
            var error = process?.StandardError.ReadToEnd();
            process?.WaitForExit();
            result.RawOutput = !string.IsNullOrWhiteSpace(output) ? output : error;
            if (result.RawOutput == null)
            {
                return result;
            }
            result.IsPythonInstalled = result.RawOutput.ToLower()
                .Contains("python");
            if (result.IsPythonInstalled)
            {
                result.DetectedVersion = result.RawOutput.Split(' ')
                    .LastOrDefault()
                    ?.Trim();
            }
            return result;
        }

        #endregion

        #region methods

        /// <summary>
        /// Checks common directories for Python. By default, all drives available will be searched. Optionally, a specific drive
        /// identifier can be passed in.
        /// </summary>
        /// <returns>A <see cref="PythonDetectionResultModel" /> indicating if Python was found and its path.</returns>
        private PythonDetectionResultModel CheckCommonDirectories(IEnumerable<string>? specificDrives = null)
        {
            var result = new PythonDetectionResultModel
            {
                DetectedPython = false,
                PathToPython = null
            };
            var systemDrives = new List<string>();
            if (!systemDrives.Any())
            {
                systemDrives = DriveInfo.GetDrives()
                    .Where(d => d.IsReady && d.DriveType == DriveType.Fixed)
                    .Select(d => d.RootDirectory.FullName)
                    .ToList();
            }
            else
            {
                systemDrives = specificDrives?.ToList();
            }
            var userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] commonDirectories =
            {
                "Python", @$"{userFolderPath}\AppData\Local\Programs\Python", @"Program Files\Python",
                @"Program Files (x86)\Python", @$"{userFolderPath}\AppData\Local\Microsoft\WindowsApps"
            };
            if (systemDrives == null || !systemDrives.Any())
            {
                Logger.LogWarning("Could not receive any drives to check for common python installation directories.");
                return result;
            }
            // Check each common directory on the system drives for Python executables
            foreach (var fullPath in systemDrives.SelectMany(_ => commonDirectories, Path.Combine))
            {
                var pythonExePath = new[] { "python.exe", "python3.exe" }
                    .Select(exe => Path.Combine(fullPath, exe))
                    .FirstOrDefault(File.Exists);
                if (pythonExePath != null)
                {
                    result.DetectedPython = true;
                    result.PathToPython = pythonExePath;
                    return result;
                }
            }
            return result;
        }

        #endregion

        #region properties

        /// <summary>
        /// Logic to interact with the operating system.
        /// </summary>
        private IOperatingSystemLogic OperatingSystemLogic { get; }

        /// <summary>
        /// The logger to use.
        /// </summary>
        private ILogger<PythonDetectionLogic> Logger { get; }

        #endregion
    }
}