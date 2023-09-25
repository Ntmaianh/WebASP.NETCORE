using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Common
{
    // lớp này để chứa những thuộc tính chung để get theo thuộc tính 
    public class PageRequestBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
