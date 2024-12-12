using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
namespace fitnessapi.Controllers
{
	public class DefaultController : ControllerBase
	{
        static string? _etag;

        readonly IWebHostEnvironment _env;

        public DefaultController(IWebHostEnvironment env)
        {
            _env = env;
        }


		[HttpGet("{**catchAll}")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
            if (string.IsNullOrWhiteSpace(_etag))
            {
                var path = Path.Combine(_env.WebRootPath, "index.html");
                var fileBytes = await System.IO.File.ReadAllBytesAsync(path, cancellationToken);
                var hash = SHA256.HashData(fileBytes);
                _etag = Convert.ToBase64String(hash);
            }
            HttpContext.Response.Headers.ETag = _etag;
            if (HttpContext.Request.Headers.IfNoneMatch.ToString() == _etag)
                return StatusCode(304);
            return File("index.html", "text/html");
        }
		
	}
}

