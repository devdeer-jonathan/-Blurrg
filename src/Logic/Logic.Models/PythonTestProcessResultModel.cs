namespace Logic.Models
{
    /// <summary>
    /// Represents the result of a python test process.
    /// </summary>
    public class PythonTestProcessResultModel
    {
        #region properties

        // Indicates if Python was detected.
        public bool IsPythonInstalled { get; set; }

        // The version of Python detected (null if not found).
        public string? DetectedVersion { get; set; }

        // Any error encountered during the test process.
        public string? ErrorMessage { get; set; }

        // The raw output from the test process.
        public string? RawOutput { get; set; }

        #endregion
    }
}