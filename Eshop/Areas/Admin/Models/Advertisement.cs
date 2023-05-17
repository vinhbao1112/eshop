using Eshop.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Models
{
    public class Advertisement
    {
        public int Id { get; set; }

       
        [NotMapped]
        [RegularExpression(@"^.*\.(jpg|JPG|gif|GIF|png|PNG)$", ErrorMessage = "Sai kiểu dữ liệu")]
        [DisplayName("Ảnh")]
        public IFormFile ImageFile { get; set; }
        [DisplayName("Ảnh")]
        public string Image { get; set; }
        [DisplayName("Loại Ảnh")]
        public int ImageTypeId { get; set; }
        [DisplayName("Loại Ảnh")]
        public ImageType ImageType { get; set; }
        public Product Product { get; set; }
      
    }
}
