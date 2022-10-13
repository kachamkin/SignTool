using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class ConvertModel
    {
        [Required]
        public IFormFile? Cert;
        public string Password = "";
        public IFormFile? PrivKey;
        [Required]
        public string Method = "To CER";
    }
}
