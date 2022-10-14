using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class RSADecryptModel
    {
        [Required]
        public IFormFile? upFile;
        [Required]
        public IFormFile? Cert;
        [Required]
        public string Method = "SHA256";
        public string Password = "";
    }
}
