namespace BoolTest.DataBase
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Jornal")]
    public partial class Jornal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(50)]
        public string MaterialName { get; set; }
    }
}
