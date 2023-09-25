using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.ProductIMage
{
    public class ProductImageUpdateRequest
    {
        public int Id { get; set; } // không phải sửa Id đâu mà để tìm kiếm bằng Id với id đầu và

        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public int SortOrder { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
