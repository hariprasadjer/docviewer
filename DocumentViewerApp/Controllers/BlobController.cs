using DocumentViewerApp.Models;
using DocumentViewerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentViewerApp.Controllers
{
    /// <summary>
    /// Controller for listing documents stored in Azure Blob Storage.
    /// </summary>
    public class BlobController : Controller
    {
        private readonly BlobStorageService _service;

        public BlobController(BlobStorageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Displays all documents in the configured container.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var documents = await _service.ListDocumentsAsync();
            return View(documents);
        }

        /// <summary>
        /// Generates a short-lived SAS URL for the given blob.
        /// </summary>
        [HttpGet]
        public IActionResult GenerateUrl(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var uri = _service.GenerateSasUri(name);
            return Json(new { url = uri.ToString() });
        }
    }
}
