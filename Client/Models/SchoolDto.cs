using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models {
    public class SchoolDto {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int FacultyId { get; set; }
    }
}