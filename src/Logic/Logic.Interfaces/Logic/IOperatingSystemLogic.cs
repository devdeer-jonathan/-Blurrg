namespace Logic.Interfaces.Logic
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Must be implemented by all logic regarding the operating system.
    /// </summary>
    public interface IOperatingSystemLogic
    {
        #region methods

        /// <summary>
        /// Detects and returns the current runtime operating system platform.
        /// </summary>
        /// <returns>
        /// The detected <see cref="OSPlatform" /> for the current runtime.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// Thrown when the operating system cannot be determined or is not supported.
        /// </exception>
        OSPlatform GetRuntimeOperatingSystem();

        #endregion
    }
}