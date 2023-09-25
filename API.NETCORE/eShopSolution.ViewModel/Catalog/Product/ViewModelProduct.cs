using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product
{
    // để lấy ra những danh sách , thuộc tính chúng ta muốn hiển thị lên 
    public class ViewModelProduct
    {
        public int Id { get; set; }
        public decimal price { get; set; }
        public decimal originalPrice { get; set; }
        public int stock { get; set; }
        public DateTime dateCreate { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public int viewCount { set; get; }
        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }
    }
}
