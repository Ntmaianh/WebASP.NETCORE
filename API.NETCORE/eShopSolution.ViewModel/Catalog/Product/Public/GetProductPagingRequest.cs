using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Product.DTO.Public
{
    public class GetProductPagingRequest : PageRequestBase
    {
        public int? CategoryID { get; set; }
    }
}
