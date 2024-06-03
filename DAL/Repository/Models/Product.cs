using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repository.Models {
    public class Product {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? InStock { get; set; }
        public int SpecialTagId { get; set; }

        [ForeignKey(nameof(SpecialTagId))]
        public SpecialTag SpecialTag { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string ImageUrl { get; set; }

        [Range(0, 100)]
        public int? DiscountPercentage { get; set; }
        public double? FinalPrice { get; set; }
    }
}
