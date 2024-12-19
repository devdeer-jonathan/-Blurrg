namespace Ui.Console
{
    using Logic.Interfaces.Logic;

    using Microsoft.Extensions.Logging;

    using Console = System.Console;

    /// <summary>
    /// Contains the application code for the console app.
    /// </summary>
    public class App
    {
        #region member vars

        private readonly ILogger<App> _logger;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="pythonDetectionLogic">Logic related to detecting Python installations on the runtime system.</param>
        public App(ILogger<App> logger, IPythonDetectionLogic pythonDetectionLogic)
        {
            _logger = logger;
            PythonDetectionLogic = pythonDetectionLogic;
        }

        #endregion

        #region methods

        /// <summary>
        /// Represents the program logic of the console app.
        /// </summary>
        /// <param name="args">The command line arguments passed to the app at startup.</param>
        /// <returns>0 if the app ran succesfully otherwise 1.</returns>
        public Task<int> StartAsync(string[] args)
        {
            var pythonDetectionResult = PythonDetectionLogic.IsPythonInstalled();
            Console.WriteLine($"Detected Python on system: {pythonDetectionResult.DetectedPython}");
            Console.WriteLine($"Path to Python: {pythonDetectionResult.PathToPython}");
            Console.WriteLine($"Python test process result: {PythonDetectionLogic.RunTestProcess().RawOutput}");
            Console.ReadKey();
            return Task.FromResult(0);
        }

        #endregion

        #region properties

        /// <summary>
        /// Logic related to detecting Python installations on the runtime system.
        /// </summary>
        private IPythonDetectionLogic PythonDetectionLogic { get; }

        #endregion
    }
}