using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fullstack.Models
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "{0} Gerekli"), Display(Name = "Rol Adı"), StringLength(20, MinimumLength = 3, ErrorMessage = "{0} en az {2} en fazla {1} karakter olabilir"),]
        public string RoleName { get; set; }
    }
}
