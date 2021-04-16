namespace BoolTest.DataBase
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BillTbl")]
    public partial class BillTbl
    {
        [Key]
        public int BillId { get; set; }

        [Required]
        [StringLength(50)]
        public string UName { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        public int Amount { get; set; }
    }
}
