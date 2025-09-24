using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Pageination
{
    public class PageinatedList<T>
    {
        public IList<T> Items { get; private set; }

        public int PageNumber { get; private set; }

       // public int PageCount => (int)Math.Ceiling
    }
}
