using Application.Catalog.Product;
using Application.Catalog.Product.DTO.Public;
using Application.Catalog.Product.Public;
using Data.Entities;
using eShopSolution.ViewModel.Catalog.Product.Manager;
using eShopSolution.ViewModel.Catalog.ProductIMage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static System.Net.WebRequestMethods;


// [FromQuery] có nghĩa là tca các thuộc tính trong GetProductPagingRequest đều được lấy từ query

//[FromQuery] is to get values from the query string
//[FromRoute] is to get values from route data
//[FromForm] is to get values from posted form fields
//[FromBody] is to get values from the request body
//[FromHeader] is to get values from HTTP headers
//[FromService] will have value injected by the DI(Dependency Injection) resolver

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProduct _Pproduct;
        private readonly IManagerProduct _Mproduct;

        public ProductController(IPublicProduct Pproduct, IManagerProduct Mproduct)
        {
            _Pproduct = Pproduct;
            _Mproduct = Mproduct;

        }

        #region Product Public
        // local host : http://localhost:port/product -> chính là tên của controller 
        [HttpGet]
        [Authorize] //  có nghĩa là phải đăng nhập , phải có token thì nó mới ăn

        // đang bị lỗi đoạn cuối 
        public async Task<IActionResult> getProduct()
        {
            var data = await _Pproduct.GetAll();
            return Ok(data);
        }
        // local host : http://localhost:port/product -> chính là tên của controller / getbyid -> content trong ()

        [HttpGet("getCategoribyid_Public")]
        [Authorize] //  có nghĩa là phải đăng nhập , phải có token thì nó mới ăn
        public async Task<IActionResult> getGetAllCategoryById([FromQuery] Application.Catalog.Product.DTO.Public.GetProductPagingRequest request)
        {
            var data = await _Pproduct.GetAllCategoryById(request);
            return Ok(data);
        }
        #endregion

        #region Product Manager

        [HttpGet("getAllForKey")]

        public async Task<IActionResult> GetAllPaging([FromQuery] ViewModel.Catalog.Product.Manager.GetProductPagingRequest request)
        {
            var procduct = await _Mproduct.GetAllPaging(request);
            return Ok(procduct);
        }

        [HttpPost]

        // thêm 
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {

            var result = await _Mproduct.Create(request);
            // result bằng 0 là vì nếu bằng 0 là nó để trống không điền gì => lỗi 
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        // sửa 
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var result = await _Mproduct.Update(request);
            // result bằng 0 là vì nếu bằng 0 là nó để trống không điền gì => lỗi 
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        // xóa
        // chỗ {id này không phải để thêm và host mà để tìm kiếm }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _Mproduct.Delete(id);
            if (result == 0) // có nghĩa là nó không xóa cái gì 
            {
                return BadRequest();
            }
            return Ok();
        }
        // Update Price 

        [HttpPatch("UpdatePrict/{productID}/{newPrice}")]
        public async Task<IActionResult> UpDatePrice(int productID, decimal newPrice)
        {
            {
                var product = await _Mproduct.UpdatePrice(productID, newPrice);
                if (product == null)
                {
                    return BadRequest();
                }

                return Ok();
            }
            
        }
        // Update Stock 
        [HttpPatch("UpdateStock/{productID}/{Quantity}")]
        public async Task<IActionResult> UpDateStock(int productID, int Quantity)
        {
            {
                var product = await _Mproduct.UpdateStock(productID, Quantity);
                if (product == null)
                {
                    return BadRequest();
                }

                return Ok();
            }
        }
        // Update Số lượng 
        // [HttpPatch -> Update 1 phần của bản ghi 
        [HttpPatch("UpdateCount/{productID}")]
        public async Task<IActionResult> UpdateViewCount (int productID)
        {
            var product = await _Mproduct.ViewCount(productID);
            if(product == null)
            {
                return NotFound();
            }
            return Ok();
        }

        #endregion

        #region Xử lí ảnh 

        [HttpPost("AddImageforProduct/{productId}")]
        public async Task<IActionResult> AddImageForProduct([FromForm]int productId,[FromForm]ProductImageCreateRequest request)
        {
            var productImg = await _Mproduct.AddImage(productId,request); 
            if(productImg < 0) // vì bên Manager nó trả về Id của ảnh => nếu ID < 0 => không tồn tại ảnh đó 
            {
                return BadRequest();
            }
            return Ok(productImg);
        }

        [HttpDelete("DeleteForProduct{ImageID}")]

        public async Task<IActionResult> DeleteImageForProduct(int ImageID)
        {
            var img = await _Mproduct.RemoveImage(ImageID);
            if(img == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("UpdateImageforProduct/{IdIMage}")]
        public async Task<IActionResult> UpdateImageForProduct(int IdIMage,[FromForm] ProductImageUpdateRequest request)
        {
            var productImg = await _Mproduct.UpdateImage(IdIMage, request);
            if(productImg == null)
            {
                return BadRequest();
            }
            return Ok();
        }
        #endregion

    }
}