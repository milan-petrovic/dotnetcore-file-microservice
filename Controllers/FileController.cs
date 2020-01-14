using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileMicroservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FileMicroservice.Services;

namespace FileMicroservice.Controllers
{
  //[Route("api/[controller]")]
  [ApiController]
  public class FileController : ControllerBase
  {
    public IUploadService _uploadService;
    public IDownloadService _downloadService;

    public FileController(IUploadService uploadService, IDownloadService downloadService)
    {
      _uploadService = uploadService;
      _downloadService = downloadService;
    }

    [Route("api/upload"), HttpPost]
    public async Task<string> Upload(IFormFile formFile, string context)
    {
      //hard coded context
      context = "context123";
      return _uploadService.UploadFile(formFile, context);
    }

    [Route("api/download/{context}/{id}"), HttpGet]
    public async Task<IActionResult> Download([FromRoute]string context, [FromRoute] int id)
    {
      return _downloadService.DownloadFile(context, id);
    }
  }
    
}
