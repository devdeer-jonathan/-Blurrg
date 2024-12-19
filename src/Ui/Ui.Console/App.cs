namespace Ui.Console
{
    using Logic.Interfaces.Logic;

    using Microsoft.Extensions.Logging;

    using Spectre.Console;

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
            // Get Python detection result
            var pythonDetectionResult = PythonDetectionLogic.IsPythonInstalled();

            // Clear the console and start with a cool title
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold green]Python Installation Detection[/]");

            // Output the result with nice formatting
            AnsiConsole.WriteLine(); // Add some space between sections
            AnsiConsole.MarkupLine(
                "[bold]Detected Python on system:[/] "
                + (pythonDetectionResult.DetectedPython ? "[green]Yes[/]" : "[red]No[/]"));
            if (pythonDetectionResult.DetectedPython)
            {
                AnsiConsole.MarkupLine(
                    "[bold]Path to Python:[/] [underline]{0}[/]",
                    pythonDetectionResult.PathToPython ?? "N/A");
                var testProcessResult =
                    PythonDetectionLogic.RunTestProcess(pythonDetectionResult.PythonExecutable ?? "python");
                AnsiConsole.MarkupLine(
                    "[bold]Python test process result:[/] {0}",
                    testProcessResult.RawOutput ?? "N/A");
                if (!string.IsNullOrEmpty(testProcessResult.ErrorMessage))
                {
                    AnsiConsole.MarkupLine("[bold red]Error:[/] {0}", testProcessResult.ErrorMessage);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Python is not installed on this system.[/]");
            }
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Press any key to exit...[/]");
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