using Microsoft.AspNetCore.Mvc;
using DocumentViewerApp.Models;

namespace DocumentViewerApp.Controllers
{
    public class DocumentController : Controller
    {
        private static readonly HashSet<string> AllowedExtensions = new()
        {
            ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"
        };

        private static readonly HashSet<string> AllowedContentTypes = new()
        {
            "application/pdf",
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/webp"
        };

        [HttpGet]
        public IActionResult Index(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return RedirectToAction(nameof(Preview), new { url });
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(DocumentViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.DocumentUrl))
            {
                ModelState.AddModelError(nameof(model.DocumentUrl), "Document URL is required");
                return View(model);
            }
            if (!IsExtensionAllowed(model.DocumentUrl))
            {
                ModelState.AddModelError(nameof(model.DocumentUrl), "File type not supported");
                return View(model);
            }
            return RedirectToAction(nameof(Preview), new { url = model.DocumentUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Preview(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !IsExtensionAllowed(url))
            {
                return BadRequest("Invalid or unsupported document url");
            }

            var contentType = await GetContentTypeAsync(url);
            if (contentType != null && !AllowedContentTypes.Contains(contentType))
            {
                return BadRequest("Unsupported content type");
            }

            var model = new DocumentViewModel { DocumentUrl = url, ContentType = contentType };
            return View(model);
        }

        private static bool IsExtensionAllowed(string url)
        {
            try
            {
                var ext = Path.GetExtension(url).ToLowerInvariant();
                return AllowedExtensions.Contains(ext);
            }
            catch
            {
                return false;
            }
        }

        private static async Task<string?> GetContentTypeAsync(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Head, url);
                using var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode && response.Content.Headers.ContentType != null)
                {
                    return response.Content.Headers.ContentType.MediaType;
                }
            }
            catch
            {
                // ignore errors and fallback to extension-based detection
            }
            return null;
        }
    }
}
