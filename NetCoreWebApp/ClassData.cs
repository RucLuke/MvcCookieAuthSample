using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApp
{
    public class ClassData
    {
        public string ClassNo { get; set; }
        public string ClassDesc { get; set; }

        public List<StudentData> Students { get; set; }
    }

    public class StudentData
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }
}
