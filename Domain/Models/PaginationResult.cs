using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PaginationResult<T>
    {
        public List<T> Values { get; set; } = new List<T>();
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int TotalPages { get; set; }
    }
}
