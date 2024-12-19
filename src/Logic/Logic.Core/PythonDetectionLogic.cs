namespace Logic.Core
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using Microsoft.Win32;

    using Models;

    /// <summary>
    /// Logic specific to detecting Python installations on the current system.
    /// </summary>
    public class PythonDetectionLogic
    {
        #region constructors and destructors

        public PythonDetectionLogic(OperatingSystemLogic operatingSystemLogic)
        {
            OperatingSystemLogic = operatingSystemLogic;
        }

        #endregion

        #region methods

        public bool DetectPythonInstallation()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            if (path != null && path.Split(Path.PathSeparator)
                    .Any(p => File.Exists(Path.Combine(p, "python")) || File.Exists(Path.Combine(p, "python3"))))
            {
                return true;
            }
            var operatingSystem = OperatingSystemLogic.GetRuntimeOperatingSystem();
            if (operatingSystem == OSPlatform.Windows)
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Python\PythonCore");
                if (key != null)
                {
                    return true;
                }
            }
            else if (operatingSystem == OSPlatform.Linux || operatingSystem == OSPlatform.OSX)
            {
                var commonPaths = new[] { "/usr/bin", "/usr/local/bin", "/opt" };
                return commonPaths.Any(
                    dir => File.Exists(Path.Combine(dir, "python")) || File.Exists(Path.Combine(dir, "python3")));
            }
            return false;
        }

        public PythonTestProcessResultModel RunTestProcess()
        {
            var result = new PythonTestProcessResultModel();
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Start the process
            using var process = Process.Start(startInfo);
            var output = process?.StandardOutput.ReadToEnd();
            var error = process?.StandardError.ReadToEnd();
            process?.WaitForExit();

            // Populate the result
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

        #region properties

        /// <summary>
        /// Logic to interact with the operating system.
        /// </summary>
        private OperatingSystemLogic OperatingSystemLogic { get; }

        #endregion
    }
}