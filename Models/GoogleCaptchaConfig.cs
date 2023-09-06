using System.ComponentModel.DataAnnotations;

namespace UFOPay.Models
{
    public class GoogleCaptchaConfig
    {
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
        [Required]
        public string captchaToken { get; set; }
    }
}