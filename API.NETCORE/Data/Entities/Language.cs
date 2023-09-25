using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Language
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public List<CategoryTranslation> CategoryTranstions { get; set; }
        public List<ProductTranslation> ProductTranstions { get; set; }
    }
}
