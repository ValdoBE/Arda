using System.Runtime.InteropServices;

namespace Arda.Core.Logging;

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

public static class Log
{
    public static LogLevel MinLevel { get; set; } = LogLevel.Debug;

    private static bool _initialized;

    public static void Debug(string message)   => Write(LogLevel.Debug,   message);
    public static void Info(string message)    => Write(LogLevel.Info,    message);
    public static void Warn(string message)    => Write(LogLevel.Warning, message);
    public static void Error(string message)   => Write(LogLevel.Error,   message);

    public static void Debug(string format, params object[] args)   => Write(LogLevel.Debug,   string.Format(format, args));
    public static void Info(string format, params object[] args)    => Write(LogLevel.Info,    string.Format(format, args));
    public static void Warn(string format, params object[] args)    => Write(LogLevel.Warning, string.Format(format, args));
    public static void Error(string format, params object[] args)   => Write(LogLevel.Error,   string.Format(format, args));

    private static void Write(LogLevel level, string message)
    {
        if (level < MinLevel) return;
        EnsureInitialized();

        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var (tag, color) = level switch
        {
            LogLevel.Debug   => ("DBG", "\x1b[90m"),  // gray
            LogLevel.Info    => ("INF", "\x1b[37m"),  // white
            LogLevel.Warning => ("WRN", "\x1b[33m"),  // yellow
            LogLevel.Error   => ("ERR", "\x1b[31m"),  // red
            _                => ("???", "\x1b[0m")
        };

        Console.WriteLine($"{color}[{timestamp}] [{tag}] {message}\x1b[0m");
    }

    private static void EnsureInitialized()
    {
        if (_initialized) return;
        _initialized = true;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            EnableWindowsAnsi();
    }

    private static void EnableWindowsAnsi()
    {
        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (GetConsoleMode(handle, out uint mode))
            SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }

    [DllImport("kernel32.dll")]
    private static extern nint GetStdHandle(int handle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(nint handle, out uint mode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(nint handle, uint mode);
}
