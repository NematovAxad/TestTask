namespace ConsoleProject.Extensions;

public static class StringExtensions
{
    public static string Env(this string key)
    {
        return Environment.GetEnvironmentVariable(key) ??
               Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine) ?? String.Empty;
    }
}