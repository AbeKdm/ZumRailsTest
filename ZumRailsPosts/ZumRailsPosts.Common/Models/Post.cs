using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZumRailsPosts.Common.Models
{
    public class Post : IEquatable<Post>
    {
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public int Id { get; set; }
        public int Likes { get; set; }
        public double Popularity { get; set; }
        public int Reads { get; set; }
        public List<string> Tags { get; set; }

        public bool Equals(Post other)
        {
            if (other == null)
                return false;

            if (this.Id == other.Id)
                return true;
            else
                return false;

        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            Post postObj = obj as Post;
            if (postObj == null)
                return false;
            else
                return Equals(postObj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator == (Post post1, Post post2)
        {
            if (((object)post1) == null || ((object)post2) == null)
                return Object.Equals(post1, post2);

            return post1.Equals(post2);
        }

        public static bool operator != (Post post1, Post post2)
        {
            if (((object)post1) == null || ((object)post2) == null)
                return ! Object.Equals(post1, post2);

            return ! post1.Equals(post2);
        }

    }
}
