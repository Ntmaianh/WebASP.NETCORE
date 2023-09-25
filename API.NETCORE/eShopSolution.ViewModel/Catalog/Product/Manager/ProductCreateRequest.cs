using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product.Manager
{
    // lớp này để chứa những thuộc tính chung có thể tạo được
    public class ProductCreateRequest
    {
        public decimal price { get; set; }
        public decimal originalPrice { get; set; }
        public int stock { get; set; }
        public DateTime dateCreate { get; set; }

        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }

        // chứa ảnh 
        public IFormFile ThumbnailImge { get; set; }


    }
}
