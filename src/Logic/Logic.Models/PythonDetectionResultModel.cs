namespace Logic.Models
{
    /// <summary>
    /// Represents the result of a Python detection run.
    /// </summary>
    public class PythonDetectionResultModel
    {
        /// <summary>
        /// Indicates whether Python was detected on the system.
        /// </summary>
        public bool DetectedPython { get; set; }

        /// <summary>
        /// The path to the Python executable, or null if Python was not detected.
        /// </summary>
        public string? PathToPython { get; set; }
    }
}