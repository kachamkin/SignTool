using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class VerifyModel
    {
        [Required]
        public IFormFile? upFile;
    }
}
