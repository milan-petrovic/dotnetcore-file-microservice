using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using FileMicroservice.Constants;
using System.Collections.Generic;
using FileMicroservice.Entities;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileMicroservice.Services
{

  public interface IUploadService {
    string UploadFile(IFormFile file, string context);
  }

  public class UploadService : IUploadService
  {
    public static IWebHostEnvironment _environment;
    public FileEntity fileEntity;

    public UploadService(IWebHostEnvironment environment)
    {
      _environment = environment;
    }

    public string UploadFile(IFormFile file, string context)
    {
      try
      {
        if (file != null && file.Length > 0)
        {
          var filePath = _environment.WebRootPath + Constants.Constants.UploadFolderName + file.FileName;
          CreateUploadDirectory();
          using (FileStream fileStream = System.IO.File.Create(filePath))
          {
            file.CopyTo(fileStream);
            fileStream.Flush();
            fileEntity = CreateFileEntity(filePath, context);
            SaveFileToDB(fileEntity);

            string downloadLink = GenerateDownloadLink(fileEntity);
            return file.FileName + " uploaded successful!" + " Download link: " + downloadLink;
          }
        }
        else
        {
          return "File not selected!";
        }
      }
      catch (Exception ex)
      {
        return ex.Message.ToString();
      }
    }

    public void CreateUploadDirectory()
    {
      if (!Directory.Exists(_environment.WebRootPath + Constants.Constants.UploadFolderName))
      {
        Directory.CreateDirectory(_environment.WebRootPath + Constants.Constants.UploadFolderName);
      }
    }

    public void SaveFileToDB(FileEntity file)
    {
      try
      {
        using (SqlConnection connection = new SqlConnection(Constants.Constants.DBConnectionString))
        {
          SqlCommand command = connection.CreateCommand();
          command.CommandText = @"
            INSERT INTO [tblFile]
            (
               [location],
               [context],
               [available]
            )
            VALUES
             (
                @location,
                @context,
                @available
             )
            declare @id as int;
            set @id = SCOPE_IDENTITY();
            select @id as id;
          ";
          FillData(command, file);
          connection.Open();
   
          using (SqlDataReader reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              file.Id = ReadId(reader);
            }
          }
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public FileEntity CreateFileEntity(string filePath, string context)
    {
      FileEntity fileEntity = new FileEntity();
      fileEntity.Location = filePath;
      fileEntity.Context = context;
      fileEntity.Available = true;
      return fileEntity;
    }

    private void FillData(SqlCommand command, FileEntity file)
    {
      command.Parameters.Add("@location", SqlDbType.VarChar);
      command.Parameters.Add("@context", SqlDbType.VarChar);
      command.Parameters.Add("@available", SqlDbType.Bit);
      command.Parameters["@location"].Value = file.Location;
      command.Parameters["@context"].Value = file.Context;
      command.Parameters["@available"].Value = file.Available;
    }

    private string GenerateDownloadLink(FileEntity fileEntity)
    {
      return Constants.Constants.DownloadRoute + fileEntity.Context + "/" + fileEntity.Id;
    }

    public int ReadId(SqlDataReader reader)
    {
      return (int)reader["id"];
    }
  }
}
