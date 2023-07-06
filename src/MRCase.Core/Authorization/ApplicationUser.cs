using Microsoft.AspNetCore.Identity;
using MRCase.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.Core.Authorization
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Datum> Data { get; set; }
    }
}
