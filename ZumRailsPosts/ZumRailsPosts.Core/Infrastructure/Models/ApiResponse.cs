using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumRailsPosts.Common.Models;

namespace ZumRailsPosts.Core.Infrastructure.Models
{
    public class ApiResponse
    {
        public List<Post> Posts { get; set; }
    }
}
