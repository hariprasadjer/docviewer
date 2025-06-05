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
        public IActionResult Preview(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !IsExtensionAllowed(url))
            {
                return BadRequest("Invalid or unsupported document url");
            }

            var model = new DocumentViewModel { DocumentUrl = url };
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
    }
}
