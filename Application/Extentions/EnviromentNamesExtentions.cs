namespace Application.Extentions;

public static class EnviromentNamesExtentions
{
    public static string EnviromentName(this string topic)
    {
        return Environment.MachineName.ToLower() + topic;
    }
}
