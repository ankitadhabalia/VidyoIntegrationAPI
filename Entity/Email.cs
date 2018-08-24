using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Email
    {
        [Key]
        public string toEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
     
    }
}
