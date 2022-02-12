namespace Minecraft.Tools
{
    public static class ConsoleOutputLevel
    {
        public enum LogLevel { INFO = 0, WARNING = 1, ERROR = 2 };
        public static string GetName(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.INFO => "INFO",
                LogLevel.WARNING => "WARNING",
                LogLevel.ERROR => "ERROR",
                _ => "DefaultInfo",
            };
        }
    }
}
