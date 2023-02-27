using Core;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CompanyDto : IDto
    {
        public Company company { get; set; }
        public int userId { get; set; }
    }
}
