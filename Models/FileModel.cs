using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMicroservice.Models
{
  public class FileModel
  {
    public IFormFile Files { get; set; }
  }
}
