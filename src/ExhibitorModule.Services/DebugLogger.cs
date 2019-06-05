using System;
using FFImageLoading.Helpers;
using Prism.Logging;
using static System.Diagnostics.Debug;

namespace ExhibitorModule.Services
{
    public class DebugLogger : ILoggerFacade, IMiniLogger
    {
        const string TAG = "[DEBUG]";

        public void Debug(string message) =>
            WriteLine($"{TAG} - Debug: {message}");

        public void Error(string errorMessage) =>
            WriteLine($"{TAG} - Error: {errorMessage}");

        public void Error(string errorMessage, Exception ex) =>
            WriteLine($"{TAG} - Error: {errorMessage}\n{ex.GetType().Name}: {ex}");

        public void Log(string message, Category category, Priority priority) =>
            WriteLine($"{TAG} - {category} - {priority}: {message}");
    }
}