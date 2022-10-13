using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class FileHashModel
    {
        [Required]
        public IFormFile? upFile;
        [Required]
        public string Method = "SHA1";
        [Required]
        public string Output = "Base64";
    }
}
