using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022.Entities
{
    public class FileData
    {
        public List<Contributor> Contributors { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
    }
}
