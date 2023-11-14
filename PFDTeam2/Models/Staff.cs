using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PFDTeam2.Models
{
    public class Staff
    {
        [Display(Name = "Staff ID")]
        [Required(ErrorMessage = "Staff ID is required.")]
        public int StaffID { get; set; }


        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [StringLength(20, ErrorMessage = "Login ID cannot exceed 20 characters.")]
        public string LastName { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(25, ErrorMessage = "Password cannot exceed 25 characters.")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }


        [Display(Name = "AllocatedLaptop")]
        public string? AllocatedLaptop { get; set; }


        [Display(Name = "DepartmentID")]
        public string? DepartmentID { get; set; }
    }
}
