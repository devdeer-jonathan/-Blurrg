namespace Logic.Core
{
    using System.Runtime.InteropServices;

    using Interfaces.Logic;

    /// <summary>
    /// Contains logic related to operating systems.
    /// </summary>
    public class OperatingSystemLogic : IOperatingSystemLogic
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
        public OSPlatform GetRuntimeOperatingSystem()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows :
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? OSPlatform.Linux :
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? OSPlatform.OSX :
                throw new ApplicationException($"Unsupported Operating System: {RuntimeInformation.RuntimeIdentifier}");
        }

        #endregion
    }
}