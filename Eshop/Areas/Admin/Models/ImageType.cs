using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Models
{
    public class ImageType
    {
        public int Id { get; set; }
        [DisplayName("Loại Ảnh")]
        public string ImageName { get; set; }

        public List<Advertisement> Advertisements { get; set; }
    }
}
