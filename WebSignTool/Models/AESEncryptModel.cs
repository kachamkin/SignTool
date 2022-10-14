using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class AESEncryptModel
    {
        [Required]
        public IFormFile? upFile;
        [Required]
        public string Output = "Base64";
        [Required]
        public string Key = "";
    }
}
