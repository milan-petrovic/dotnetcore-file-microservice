using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace FileMicroservice.Constants
{
  public static class Constants
  {
    public static string UploadFolderName
    {
      get
      {
        return "\\Upload\\";
      }
    }

    public static string DBConnectionString
    {
      get
      {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        return builder.Build().GetSection("ConnectionStrings").GetSection("FileConnectionString").Value;
      }
    }

    public static string DownloadRoute
    {
      get
      {
        return "https://localhost:44313/api/download/";
      }
    }
    
  }
}
