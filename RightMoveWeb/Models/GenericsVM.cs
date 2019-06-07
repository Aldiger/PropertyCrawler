using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightMoveWeb.Models
{

    public class SelectVm
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
    public class PagedList<T>
    {
        public PagedList()
        {
            Items = new List<T>();
        }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }

}
