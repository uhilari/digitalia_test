using System.Text;
using System.Text.Encodings.Web;

namespace Digitalia.Identity.Api
{
    public class EncryptMiddleware(RequestDelegate next)
    {
        class EncryptData { public required string Data { get; set; }  }

        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            await modifiRequest(context.Request);

            var originalBody = context.Response.Body;
            using (var newBody = new MemoryStream())
            {
                context.Response.Body = newBody;

                await _next.Invoke(context);

                newBody.Position = 0;
                var responseBody = await new StreamReader(newBody).ReadToEndAsync();

                newBody.SetLength(0);
                var data = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(responseBody)));
                newBody.Write(data, 0, data.Length);

                newBody.Position = 0;
                await newBody.CopyToAsync(originalBody);
            }
            
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
    }
}
