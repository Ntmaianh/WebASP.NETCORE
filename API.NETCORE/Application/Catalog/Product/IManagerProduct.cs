using eShopSolution.ViewModel.Catalog.Product;
using eShopSolution.ViewModel.Catalog.Product.Manager;
using eShopSolution.ViewModel.Catalog.ProductIMage;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Catalog.Product
{
    // tất cả phường thức của class đều định nghĩa qua interface => để t/ca class đều sd được => áp dụng Dependency injection pattern
    // Dependency injection pattern : đảo ngược sự phụ thuộc.

    // interface này để quản lí những chức năng của web => Dev sẽ sd Interface này => để quản lí các chức năng của web

    // sử dụng Bất đồng bộ để sử lí nhiều task 1 lúc 
    public interface IManagerProduct
    {
        // interface này để tạo mới 1 sản phẩm => int là trả về mã sp 
        Task<int> Create(ProductCreateRequest request);

        // interface này để tạo update 1 sản phẩm => int là trả về mã sp 
        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int id);

      // Task<ViewModelProduct> GetById(int id);
        // lấy các sp theo key 
       Task<PageResult<ViewModelProduct>> GetAllPaging(GetProductPagingRequest request);

        // update giá và số lượng

        Task<bool> UpdatePrice(int productID, decimal Newprice);
        Task<bool> UpdateStock(int productID, int Quantity);
        Task<int> ViewCount (int productID);

        // crud ảnh 
        Task<int> AddImage(int productID , ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int ImgID, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<List<ProductImageViewModel>> GetListImages (int productId);
    }
}
