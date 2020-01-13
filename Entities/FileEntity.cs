using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMicroservice.Entities
{
  public class FileEntity
  {
    public string Id { get; set; }
    public string Location { get; set; }
    public string Context { get; set; }
    public bool Available { get; set; }
  }
}
