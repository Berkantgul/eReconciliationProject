using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class AdminCompaniesForUserDto : Company, IDto
    {
        public bool IsTrue { get; set; }
    }
}
