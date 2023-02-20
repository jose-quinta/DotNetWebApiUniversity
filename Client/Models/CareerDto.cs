using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models {
    public class CareerDto {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}