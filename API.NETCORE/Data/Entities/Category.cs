using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Category
    {
        public int id {  get; set; }
        public int sortOrder { set; get; }
        public bool isShowOnHome { set; get; }
        public int? parentId { set; get; }
        public Status Status { set; get; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<CategoryTranslation> CategoryTranstions { get; set; }
    }
}
