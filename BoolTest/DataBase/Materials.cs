namespace BoolTest.DataBase
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BookTbl")]
    public partial class Materials
    {
        [Key]
        public int BId { get; set; }

        [Required]
        [StringLength(100)]
        public string BTitle { get; set; }

        [Required]
        [StringLength(50)]
        public string BAuthor { get; set; }

        [Required]
        [StringLength(50)]
        public string BCat { get; set; }

        public int BQty { get; set; }

        public int BPrice { get; set; }
    }
}
