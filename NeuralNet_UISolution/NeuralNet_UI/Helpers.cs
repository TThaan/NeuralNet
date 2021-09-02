using System;

namespace NeuralNet_UI
{
    public static class Helpers
    {
        internal static string GetFormattedExceptionMessage(Exception e)
        {
            return $"{e.GetType().Name}:\nDetails: {e.Message}";
        }
        internal static void ThrowFormattedException(Exception e)
        {
            throw new ArgumentException(GetFormattedExceptionMessage(e));
        }
        internal static void ThrowFormattedArgumentException(string message)
        {
            throw new ArgumentException($"ArgumentException:\nDetails: {message}");
        }
    }
}
