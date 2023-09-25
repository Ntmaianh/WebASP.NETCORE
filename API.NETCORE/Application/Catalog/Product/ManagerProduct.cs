using Data.EF;
using Data.Entities;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Catalog.Product;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using eShopSolution.ViewModel.Catalog.Product.Manager;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Catalog.Product;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Application.Common;
using eShopSolution.ViewModel.Catalog.ProductIMage;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Catalog.Product.DTO
{
    public class ManagerProduct : IManagerProduct
    {
        private readonly ESHOPDBContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ManagerProduct(ESHOPDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Data.Entities.Product()
            {
                price = request.price,
                originalPrice = request.originalPrice,
                stock = request.stock,
                viewCount = 0,
                seoAlias = request.SeoAlias,
                dateCreate = DateTime.Now,
                ProductTranstions = new List<ProductTranslation>
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId

                    }
                }
            };

            // Xử lí Save Imge

            // nếu cái ThumbnailImge(Nơi chứa ảnh) không trống 
            if (request.ThumbnailImge != null)
            {
                // thì thêm cái ảnh đấy vào List ảnh ở product thêm qua bảng ProductIMage
                product.productIMages = new List<ProductIMage>()
                {
                    new ProductIMage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImge.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImge),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);

            // sử dụng bất đồng bộ để có thể sử lí nhiều request 1 lúc 
            return await _context.SaveChangesAsync();
  

        }

        public async Task<int> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                // văng ra 1 ex tự tạo ở project dùng chứa những cái dùng chung cho các project 
                throw new Exception("Can not find product");
            }
            var images = _context.ProductImages.Where(i => i.ProductId == id);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id);
            if (productTranslations == null && product == null)
            {
                throw new Exception("Can not find product");
            }
            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;

            // xử lí update ảnh 
            if (request.ThumbnailImge != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImge.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImge);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productID, decimal Newprice)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product == null)
            {
                throw new Exception("Can not find product");
            }
            product.price = Newprice;
            // nếu trả về giá lớn hơn 0 thì trả về true 
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productID, int Quantity)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product == null)
            {
                throw new Exception("Can not find product");
            }
            product.stock += Quantity;
            // nếu trả về giá lớn hơn 0 thì trả về true 
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> ViewCount(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            product.viewCount += 1;
            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<ViewModelProduct>> GetAllPaging(GetProductPagingRequest request)
        {
            // b1 Select
            // để lấy ra được tên sản phẩm = join với bảng ProductTraslation

            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.id equals pt.ProductId

                        // lấy ra được tên loại => join ProductInCategory ; bảng Category join với CategoryTraslation để lấy ra tên loại 

                        join cp in _context.ProductInCategories on p.id equals cp.ProductId
                        join c in _context.Categories on cp.CategoryId equals c.id
                        //where pt.Name.Contains(request.KeyWord)
                        select new { p, pt, cp };

            /// b2 Find
            if (!String.IsNullOrEmpty(request.KeyWord))
            {
                query = query.Where(x => x.pt.Name.Contains(request.KeyWord));
            }
            if (request.CategoryID.Count > 0)
            {
                query = query.Where(p => request.CategoryID.Contains(p.cp.CategoryId));
            }

            // xuất ra màn 

            // đếm tông số bảng ghi thỏa mãn
            int totalRow = await query.CountAsync();

            // phân trang 
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ViewModelProduct()
                {
                    Id = x.p.id,
                    Name = x.pt.Name,
                    dateCreate = x.p.dateCreate,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    originalPrice = x.p.originalPrice,
                    price = x.p.price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    stock = x.p.stock,
                    viewCount = x.p.viewCount
                }).ToListAsync();

            // 

            var pagedResult = new PageResult<ViewModelProduct>()
            {
                Item = data,
                TotalRecord = totalRow,
            };
            return pagedResult;
        }

        // Method để xử lí thêm ảnh 
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }


        // CRUD với Ảnh

        // thêm id sản phẩm để thêm ảnh cho sp đó 
        public async Task<int> AddImage(int productID, ProductImageCreateRequest request)
        {
            var productImg = new ProductIMage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productID,
                SortOrder = request.SortOrder
            };

            // lấy ra link ảnh 
            if (request.ImageFile != null)
            {
                productImg.ImagePath = await this.SaveFile(request.ImageFile);
                productImg.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImg);
            await _context.SaveChangesAsync();
            return productImg.Id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var ImgforDelete = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (ImgforDelete == null)
            {
                throw new Exception("Can not find productIMg");
            }
            _context.ProductImages.Remove(ImgforDelete);
        return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int ImgID, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(ImgID);
            if (productImage == null)
                throw new Exception("Can not find productIMg");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new Exception("Can not find productIMg"); 

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        // lấy ra danh sách ảnh của sản phẩm đó (tìm kiếm ảnh theo ID )
        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();

            // List<ProductImageViewModel> lstImg = new List<ProductImageViewModel> ();
            // var product = _context.ProductImages.FirstOrDefault(x => x.ProductId == productId);
            // if(product == null)
            // {
            //     throw new Exception("Can not find product");
            // }
            //lstImg.Add(product);
        }

       
    }
}

