using System;
using MvvmCross.Platform.Platform;

namespace FoodByMe.Tests.Utilities
{
    internal class ConsoleTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Trace(level, tag, message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Trace(level, tag, message, new object[] {});
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            var formatted = string.Format(message, args);
            Console.WriteLine($"{tag} [{level}]: ${formatted}");
        }
    }
}
