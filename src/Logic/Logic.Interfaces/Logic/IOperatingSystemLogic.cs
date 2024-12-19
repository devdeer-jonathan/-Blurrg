namespace Logic.Interfaces.Logic
{
    using System.Runtime.InteropServices;

    public interface IOperatingSystemLogic
    {
        #region methods

        /// <summary>
        /// Detects and returns the current runtime operating system platform.
        /// </summary>
        /// <returns>
        /// The detected <see cref="System.Runtime.InteropServices.OSPlatform" /> for the current runtime.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// Thrown when the operating system cannot be determined or is not supported.
        /// </exception>
        OSPlatform GetRuntimeOperatingSystem();

        #endregion
    }
}