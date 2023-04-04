using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserCompanyForListDto : IDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public DateTime UserAddetAt { get; set; }
        public bool UserIsActive { get; set; }
        public string Value { get; set; }
    }

}
