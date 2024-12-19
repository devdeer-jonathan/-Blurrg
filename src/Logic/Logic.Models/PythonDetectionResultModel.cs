namespace Logic.Models
{
    /// <summary>
    /// Represents the result of a Python detection run.
    /// </summary>
    public class PythonDetectionResultModel
    {
        #region properties

        /// <summary>
        /// Indicates whether Python was detected on the system.
        /// </summary>
        public bool DetectedPython { get; set; }

        /// <summary>
        /// The path to the Python executable, or null if Python was not detected.
        /// </summary>
        public string? PathToPython { get; set; }

        /// <summary>
        /// The command used to invoke Python (e.g., "python"or "python3").
        /// </summary>
        public string? PythonExecutable =>
            PathToPython?.ToLowerInvariant()
                    .Split(Path.DirectorySeparatorChar)
                    .LastOrDefault()
                    ?.ToLowerInvariant() switch
                {
                    "python.exe" => "python",
                    "python3.exe" => "python3",
                    _ => null
                };

        #endregion
    }
}