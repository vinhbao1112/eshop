using Eshop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        
        [DisplayName("Khách Hàng")]
        public Account Account { get; set; }
        [DisplayName("Nội Dung")]
        public string Content { get; set; }
        [DisplayName("Đăng vào lúc")]
        public DateTime CreatedAt { get; set; }
        
        public int ProductId { get; set; }
        [DisplayName("Tên sản phẩm")]
        public Product Product { get; set; }
    }
}
