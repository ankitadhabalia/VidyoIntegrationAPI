using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProductImage
    {
        [Key]
        public int id { get; set; }
        public byte[] Image { get; set; }
    }
}
