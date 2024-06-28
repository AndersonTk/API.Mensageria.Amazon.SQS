using Microsoft.Owin;
using System.Text.RegularExpressions;

public class HangfireDashboardMiddleware : OwinMiddleware
{
    public HangfireDashboardMiddleware(OwinMiddleware next)
        : base(next)
    {
    }

    public override async Task Invoke(IOwinContext context)
    {
        // Interceptando a resposta
        var originalStream = context.Response.Body;
        using (var newStream = new MemoryStream())
        {
            context.Response.Body = newStream;

            await Next.Invoke(context);

            newStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(newStream);
            string responseHtml = await reader.ReadToEndAsync();

            // Modificar HTML para incluir tags
            if (context.Request.Path.StartsWithSegments(new PathString("/jobs")))
            {
                responseHtml = AddTagsToHtml(responseHtml);
            }

            var modifiedStream = new MemoryStream();
            var writer = new StreamWriter(modifiedStream);
            writer.Write(responseHtml);
            writer.Flush();
            modifiedStream.Seek(0, SeekOrigin.Begin);

            await modifiedStream.CopyToAsync(originalStream);
            context.Response.Body = originalStream;
        }
    }

    private string AddTagsToHtml(string html)
    {
        // Regex para encontrar onde adicionar tags no HTML
        // Este é um exemplo e pode precisar de ajustes conforme seu HTML específico
        var regex = new Regex("(regex para localizar o ponto de inserção das tags)");
        string replacementHtml = "html para inserir as tags";

        return regex.Replace(html, replacementHtml);
    }
}
