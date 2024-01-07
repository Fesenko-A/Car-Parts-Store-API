using System.ComponentModel.DataAnnotations;

namespace DAL.Repository.Models {
    public class SpecialTag {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
