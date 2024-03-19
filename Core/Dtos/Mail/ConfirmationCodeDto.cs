using System.Security.Cryptography;

namespace be_project_swp.Core.Dtos.Mail
{
    public class ConfirmationCodeDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } 
        public bool IsDeleted { get; set; }
    }
}
