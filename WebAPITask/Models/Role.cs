using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITask.Models
{
    [Table("roles")]
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [Column("n_id")]
        public int n_id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("s_description")]
        public string s_description { get; set; }

        [InverseProperty("RoleNavigation")]
        public virtual ICollection<User> Users { get; set; }
    }
}
