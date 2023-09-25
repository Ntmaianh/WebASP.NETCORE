using Application.Catalog.Product.DTO.Public;
using eShopSolution.ViewModel.Catalog.Product;
using eShopSolution.ViewModel.Common;

namespace Application.Catalog.Product.Public
{
    // interface này để public thông tin. Chuyên chứa những phương thức public 
    public interface IPublicProduct
    {
       Task <PageResult<ViewModelProduct>> GetAllCategoryById(GetProductPagingRequest request);
       Task <List<ViewModelProduct>> GetAll();
    }
}
