using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.Application.Data.Dtos
{
    public class DatumResponseDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Event { get; set; }
    }
}
