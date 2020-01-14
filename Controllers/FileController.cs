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

    public FileController(IUploadService uploadService)
    {
      _uploadService = uploadService;
    }

    [Route("api/upload")]
    [HttpPost]
    public async Task<string> Upload(IFormFile formFile)
    {
      return _uploadService.UploadFile(formFile);
    }

    [Route("api/download/")]
    [HttpGet]
    public async Task<IActionResult> Download()
    {
      var path = @"C:\Users\milan\source\repos\FileMicroservice\FileMicroservice\wwwroot\Upload\URIS_SRS_proces_IEEEstd.pdf";
      var memory = new MemoryStream();
      using (var fileStream = new FileStream(path, FileMode.Open))
      {
        await fileStream.CopyToAsync(memory);
      }
      memory.Position = 0;
      return File(memory, GetContentType(path), Path.GetFileName(path));
    }

    private string GetContentType(string path)
    {
      var types = GetMimeTypes();
      var ext = Path.GetExtension(path).ToLowerInvariant();
      return types[ext];
    }

    private Dictionary<string, string> GetMimeTypes()
    {
      return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
    }
  }
    
}
