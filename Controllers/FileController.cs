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
  }
    
}
