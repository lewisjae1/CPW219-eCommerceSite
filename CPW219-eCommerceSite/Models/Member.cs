using System.ComponentModel.DataAnnotations;

namespace CPW219_eCommerceSite.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        [Required]
        public string Email { get; set; }

        public string PassWord { get; set; }

        public string Phone { get; set; }

        public string Username { get; set; }
    }
}
