using System.Text;
using System.Text.Encodings.Web;

namespace Digitalia.Identity.Api
{
    public class EncryptMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            await modifiRequest(context.Request);

            var response = context.Response;
            var originalBody = response.Body;
            using var newBody = new MemoryStream();
            response.Body = newBody;

            await _next.Invoke(context);

            await modifyResponse(response);

            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
            response.Body = originalBody;
        }

        private async Task modifiRequest(HttpRequest httpRequest)
        {
            string originalContent;
            using (var stream = new StreamReader(httpRequest.Body))
            {
                originalContent = await stream.ReadToEndAsync();
            }

            var data = Convert.FromBase64String(originalContent);

            httpRequest.Body = new MemoryStream(data);
            httpRequest.ContentLength = data.Length;
            httpRequest.ContentType = "application/json";
        }

        private async Task modifyResponse(HttpResponse httpResponse)
        {
            var stream = httpResponse.Body;
            string originalContent;
            using var reader = new StreamReader(stream, leaveOpen: true);
            originalContent = await reader.ReadToEndAsync();

            var modifiedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalContent));
            stream.SetLength(0);

            using var writer = new StreamWriter(stream, leaveOpen: true);
            await writer.WriteAsync(modifiedContent);
            await writer.FlushAsync();
            httpResponse.ContentLength = stream.Length;
        }
    }
}
