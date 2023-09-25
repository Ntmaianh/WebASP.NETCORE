using Application.Catalog.Product.DTO.Public;
using Data.EF;
using eShopSolution.ViewModel.Catalog.Product;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Product.DTO
{
    public class PublicProductBase
    {
        private readonly ESHOPDBContext _context;

        public PublicProductBase(ESHOPDBContext context)
        {
            _context = context;
        }
        public async Task<PageResult<ViewModelProduct>> GetAllCategoryById(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.id equals pt.ProductId

                        // lấy ra được tên loại => join ProductInCategory ; bảng Category join với CategoryTraslation để lấy ra tên loại 

                        join cp in _context.ProductInCategories on p.id equals cp.ProductId
                        join c in _context.Categories on cp.CategoryId equals c.id
                        //where pt.Name.Contains(request.KeyWord)
                        select new { p, pt, cp, c };

            // b2 Find


            //var result =  query.FirstOrDefaultAsync(x => x.c.id == request.CategoryID);
            // if (result == null)
            // {
            //     throw new Exception("Can not find product");
            // }

            if (request.CategoryID.HasValue && request.CategoryID.Value > 0)
            {
                query = query.Where(p => p.cp.CategoryId == request.CategoryID);

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
        }
    }
