using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetFinale.Models
{
    public class Produits
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Marque { get; set; }
        public double Capacite { get; set; }
        public string Utilisation { get; set; }
        public string Fonctionnalite { get; set; }
        [Display(Name = "Prix")]
        [Column(TypeName = "decimal(10,2)")]
        [DataType(DataType.Currency)]
        public double Prix { get; set; }
        [Required(ErrorMessage = "L'image est  requise")]
        [Display(Name = "Front Image")]
        public string Image { get; set; }
        public int CategorieId { get; set; }
    }
}
