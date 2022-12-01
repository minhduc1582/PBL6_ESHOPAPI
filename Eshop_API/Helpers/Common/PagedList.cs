using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eshop_pbl6.Helpers.Common
{
    public  class PagedList<T>
    {
        [JsonProperty("CurrentPage")]
        public int CurrentPage { get; private set; }
        [JsonProperty("TotalPages")]
        public int TotalPages { get; private set; }
        [JsonProperty("PageSize")]
        public int PageSize { get; private set; }
        [JsonProperty("TotalCount")]
        public int TotalCount { get; private set; }
        [JsonProperty("Items")]
        public List<T> Items {get;set;} = new List<T>(); 
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public  PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items.AddRange(items);
        }
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var a =  new PagedList<T>(items, count, pageNumber, pageSize);
            return a;
        }
    }
}