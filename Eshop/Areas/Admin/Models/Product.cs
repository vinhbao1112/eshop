using Eshop.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Code")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string SKU { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string Name { get; set; }

        [DisplayName("Tác Giả")]
        public string Author { get; set; }

        [DisplayName("Số Trang")]
        public string NumerOfPages { get; set; }

        [DisplayName("Nhà Xuất Bản")]
        public string Publishing { get; set; }

        [DisplayName("Nội Dung")]
        public string Description { get; set; }

        [DisplayName("Giá (VNĐ)")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [DefaultValue(0)]
        public int Price { get; set; } = 0;

        [DisplayName("Tồn kho")]
        [DefaultValue(0)]
        public int Stock { get; set; } = 0;

        [DisplayName("Loại Sản Phẩm")]
        public int ProductTypeId { get; set; }

        // Navigation reference property cho khóa ngoại đến ProductType
        [DisplayName("Loại sản phẩm")]
        public ProductType ProductType { get; set; }
        [DisplayName("Ảnh Sản Phẩm")]
        public string Image { get; set; }

        [DisplayName("Ảnh Sản Phẩm")]

        [NotMapped]
        //[RegularExpression(@"^.*\.(jpg|JPG|gif|GIF|png|PNG)$", ErrorMessage = "Sai kiểu dữ liệu")]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        // Collection reference property cho khóa ngoại từ Cart
        public List<Cart> Carts { get; set; }

        // Collection reference property cho khóa ngoại từ InvoiceDetail
        public List<InvoiceDetail> InvoiceDetails { get; set; }

        public List<Advertisement> Advertisements { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
