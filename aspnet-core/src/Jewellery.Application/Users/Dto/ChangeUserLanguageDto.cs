using System.ComponentModel.DataAnnotations;

namespace Jewellery.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}