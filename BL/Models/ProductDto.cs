﻿namespace BL.Models {
    public class ProductDto {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SpecialTagId { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
