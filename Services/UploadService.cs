using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
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
    string UploadFile(IFormFile file);
  }
  public class UploadService : IUploadService
  {
    public static IWebHostEnvironment _environment;
    public FileEntity fileEntity;

    public UploadService(IWebHostEnvironment environment) {
      _environment = environment;
    }
    public string UploadFile(IFormFile file)
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
            FileEntity fileEntity = CreateFileEntity(filePath);
            SaveFileToDB(fileEntity);
            return file.FileName + " uploaded successful!";
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

    public void CreateUploadDirectory() {

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
          ";
          FillData(command, file);
          connection.Open();
          command.ExecuteNonQuery();
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public FileEntity CreateFileEntity(string filePath) {
      FileEntity fileEntity = new FileEntity();
      fileEntity.Location = filePath;
      fileEntity.Context = "testContext";
      fileEntity.Available = false;
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
  }
}