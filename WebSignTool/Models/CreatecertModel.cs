using System.ComponentModel.DataAnnotations;

namespace WebSignTool.Models
{
    public class CreatecertModel
    {
        public string Password = "";
        [Required]
        public string Issuer = "";
        [Required]
        public string AsymmetricAlgorithm = "RSA2048";
        [Required]
        public string HashAlgorithm = "SHA256";
        [Required]
        public uint Valid = 1;
    }
}
