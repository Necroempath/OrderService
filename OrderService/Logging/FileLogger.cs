namespace OrderService.Logging;

public class FileLogger(string filePath = "log.txt") : ILogger
{
    public void Log(string message) => File.AppendAllText(filePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
    public void LogError(string message) => Log($"[ERROR] {message}");
    public void LogInfo(string message) => Log($"[INFO] {message}");
}