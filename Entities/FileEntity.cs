using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileMicroservice.Entities
{
  public class FileEntity
  {
    public int Id { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string Context { get; set; }
    [Required]
    public bool Available { get; set; }
  }
}
