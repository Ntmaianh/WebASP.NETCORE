using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product.Manager
{
    // Lớp này là lớp kế thừa của PageRequestBase để lấy ra những thuộc tính riêng ngoài những thuộc tính chung của lớp base
    public class GetProductPagingRequest : PageRequestBase
    {
        public string KeyWord { get; set; }
        public List<int> CategoryID { get; set; }
    }
}
