using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileMicroservice.Services
{

  public interface IDownloadService
  {
    IActionResult DownloadFile(string context, int id);
  }

  public class DownloadService : ControllerBase, IDownloadService
  {
    public IActionResult DownloadFile(string context, int id)
    {
      return AsyncDownloadTask(context, id);
    }

    private IActionResult AsyncDownloadTask(string context, int id)
    {
      var path = GetPathFromDB(context, id);
      var memory = new MemoryStream();
      using (var fileStream = new FileStream(path, FileMode.Open))
      {
        fileStream.CopyToAsync(memory);
      }
      memory.Position = 0;
      return File(memory, GetContentType(path), Path.GetFileName(path));
    }

    private String GetPathFromDB(string context, int id)
    {
      string path = "";

      using (SqlConnection connection = new SqlConnection(Constants.Constants.DBConnectionString))
      {
        SqlCommand command = connection.CreateCommand();
        command.CommandText = @"
            SELECT
                [location]
            FROM
                [tblFile]
            WHERE
               [id]= @id AND
               [context] = @context
        ";

        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters.Add("@context", SqlDbType.VarChar);
        command.Parameters["@id"].Value = id;
        command.Parameters["@context"].Value = context;
        connection.Open();

        using(SqlDataReader reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            path = ReadFileLocation(reader);
          }
        }

        return path;
      }
    }

    private string ReadFileLocation(SqlDataReader reader)
    {
      return (string)reader["location"] as string;
    }

    private static object CreateLikeQueryString(string str)
    {
      return str == null ? (object)DBNull.Value : "%" + str + "%";
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

