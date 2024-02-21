using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumRailsPosts.Common.Entities;

namespace ZumRailsPosts.Core.Logic
{
    public interface IPostsLogic
    {
        Task<List<Post>> GetPostsAsync(string[] tags, string sortBy = "id", string direction = "desc");
    }
}
