namespace Application.Extentions;

public static class EnviromentNamesExtentions
{
    public static string EnviromentName(this string topic)
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower() + topic;
    }
}
