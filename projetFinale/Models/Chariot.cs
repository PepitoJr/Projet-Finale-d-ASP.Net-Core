using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetFinale.Models
{
    public class Chariot
    {
        public int Id { get; set; }
        public int ProduitId { get; set; }
        public string Marque { get; set; }
        public double Capacite { get; set; }
        [Display(Name = "Prix")]
        [Column(TypeName = "decimal(10,2)")]
        [DataType(DataType.Currency)]
        public double Prix { get; set; }
        public int Quantite { get; set; }
        public  Produits Produit { get; set; }
    }
}
