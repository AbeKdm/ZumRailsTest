using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumRailsPosts.Core.Infrastructure.Models;

namespace ZumRailsPosts.Core.Infrastructure
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetAllPostsAsync(string tag);
    }
}
