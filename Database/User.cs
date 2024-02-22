using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        
        [Required(ErrorMessage ="EmailId is required..")]
        [EmailAddress(ErrorMessage ="Enter valid email")]
       
        public string? EmailID { get; set; }
        
        [Required(ErrorMessage ="Password is required..")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Your password must be minimum six characters, at least one uppercase letter, one lowercase letter, one number and one special characte")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "UserName is required..")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "FullName is required..")]
        [RegularExpression(@"^[A-Za-z\\s ]+$", ErrorMessage = "Allow only alphabets and spaces..")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Joining Date is required..")]
        public DateTime? JoinDate { get; set; }

        [Required(ErrorMessage = "Contact No is required..")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid phone number")]
        public string? ContactNo { get; set; }

        [Required(ErrorMessage = "Select Gender ..")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Address is required..")]
        public string? Address { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

       
       
    }
}
