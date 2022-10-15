using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class SignfileModel
    {
        [Required]
        public IFormFile? upFile;
        [Required]
        public IFormFile? Cert;
        public string Password = "";
        public bool Detached = false;
    }
}
