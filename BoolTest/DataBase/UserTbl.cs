namespace BoolTest.DataBase
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserTbl")]
    public partial class UserTbl
    {
        [Key]
        public int UId { get; set; }

        [Required]
        [StringLength(50)]
        public string UName { get; set; }

        [Required]
        [StringLength(50)]
        public string UPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string UAdd { get; set; }

        [Required]
        [StringLength(50)]
        public string UPass { get; set; }
    }
}
