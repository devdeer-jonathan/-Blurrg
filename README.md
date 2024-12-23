# Blurrg

This project can be used to detect and find installations of python on the current system. This can be useful if you want to create a stable environment to run Python code from as a process inside your .NET project.

## Features

- **Cross-platform Support**: Works on Windows, Linux, and macOS.
- **Environment Variable Search**: Scans the `PATH` environment variable for Python executables.
- **Common Directory Search**: Searches in known directories and locations for Python installations.
- **Registry-based Detection (Windows)**: Utilizes the Windows Registry for enhanced Python detection.
- **Microsoft Store Check (Windows)**: Checks if Python was installed using the Microsoft Store.
- **Version Check**: Runs a test process to confirm the installed Python version.

## Usage

### Detecting Python Installations

The PythonDetectionLogic class provides the main functionality. Use the IsPythonInstalled() method to check if Python is installed:

```
// Optimally the logic and logger should be added to your DI.
var pythonDetection = new PythonDetectionLogic(operatingSystemLogic, logger);
var detectionResult = pythonDetection.IsPythonInstalled();
if (detectionResult.DetectedPython)
{
Console.WriteLine($"Python detected at: {detectionResult.PathToPython}");
}
else
{
Console.WriteLine("Python is not installed.");
}
```

## Test Python

```
Once you verified that Python is installed on the machine you can verify it by running a test process.
// Optimally the logic and logger should be added to your DI.
var pythonDetection = new PythonDetectionLogic(operatingSystemLogic, logger);
var testResult = pythonDetection.RunTestProcess(pythonDetection.PythonExecutable);
if (testResult.IsPythonInstalled)
{
Console.WriteLine($"Python Version: {testResult.DetectedVersion}");
}
else
{
Console.WriteLine("Unable to retrieve Python version.");
}
```

## Coming soon:

- Support for older python versions
- Additional checks for MacOS and Linux
- More options to offer a more customizable experience (such as checking specific paths and such).
- Adjustable config
