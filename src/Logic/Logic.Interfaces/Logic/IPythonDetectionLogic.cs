namespace Logic.Interfaces.Logic
{
    using Models;

    /// <summary>
    /// Must be implemented by all logic specific to python detection.
    /// </summary>
    public interface IPythonDetectionLogic
    {
        #region methods

        /// <summary>
        /// Runs a test python process.
        /// </summary>
        /// <returns>
        /// A <see cref="PythonTestProcessResultModel" /> containing information about whether Python the test process.
        /// </returns>
        PythonTestProcessResultModel RunTestProcess();

        #endregion
    }
}