using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Product
    {
        public int id { get; set; } 
        public decimal price { get; set; } 
        public decimal originalPrice { get; set; } 
        public int stock { get; set; } 
        public int viewCount { get; set; } 
        public DateTime dateCreate { get; set; } 
        public string seoAlias { get; set; } 
 
        public List<ProductInCategory> ProductInCategories { get; set; }

        public List<OderDetail> OrderDetails { set; get; }

        public List<Cart> Carts { get; set; }   
        public List<ProductIMage> productIMages { get; set; }
        public List<ProductTranslation> ProductTranstions { get; set; }

    }
}
