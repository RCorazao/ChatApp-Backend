
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Attributes
{
    public class ImageFileAttribute : ValidationAttribute
    {
        public int MaxFileSize { get; set; } = 2 * 1024 * 1024; // Default max size is 2MB
        public string[] AllowedExtensions { get; set; } = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (!file.ContentType.StartsWith("image/"))
                {
                    return new ValidationResult("The file is not a valid image.");
                }

                if (file.Length > MaxFileSize)
                {
                    return new ValidationResult($"The file size exceeds the limit of {MaxFileSize / (1024 * 1024)} MB.");
                }

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedExtensions.Contains(extension))
                {
                    return new ValidationResult("The file extension is not allowed.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
