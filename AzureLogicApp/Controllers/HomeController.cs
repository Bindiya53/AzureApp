using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureLogicApp.Models;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureLogicApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    static readonly HttpClient client = new HttpClient(); 
    private readonly BlobServiceClient _blobClient;
    

    public HomeController(ILogger<HomeController> logger, BlobServiceClient blobClient)
    {
        _logger = logger;
        _blobClient = blobClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LogicAppRequest logicAppRequest, IFormFile file)
    {
        logicAppRequest.Id = Guid.NewGuid().ToString();
        using(var content = new StringContent(JsonConvert.SerializeObject(logicAppRequest),
        System.Text.Encoding.UTF8, "application/json"))
        { 
            HttpResponseMessage httpResponse = await client.PostAsync("https://prod-76.eastus.logic.azure.com:443/workflows/055855e096e947ee8984f0dcb388240b/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=jZbtUTGV8GhIOvUdJe0Ly5B3bft9mjvGlYZop4YzCco", content);
 
        }

         if (file != null)
        {
            var fileName = logicAppRequest.Id + Path.GetExtension(file.FileName);
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient("logicappholder");
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
