using MRCase.Core.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.Core.Data
{
    [Table("Data")]
    public class Datum
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Event { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
