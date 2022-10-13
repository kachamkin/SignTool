using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class DHModel
    {
        [Required]
        public IFormFile? PubCert;
        [Required]
        public IFormFile? PrivCert;
        public string Password = "";
        [Required]
        public string Output = "Base64";
        [Required]
        public string Method = "SHA256";
    }
}
