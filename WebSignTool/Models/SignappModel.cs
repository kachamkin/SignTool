using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class SignappModel
    {
        [Required]
        public IFormFile? upFile;
        [Required]
        public IFormFile? Cert;
        public string Password = "";
        public bool IsAppX = false;
        [Required]
        public string TimeStampURL = "http://timestamp.sectigo.com";
    }
}
