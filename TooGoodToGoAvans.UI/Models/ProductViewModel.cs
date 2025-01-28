using System;
using System.Collections.Generic;
using TooGoodToGoAvans.Domain.Models;


namespace TooGoodToGoAvans.UI.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Alcoholic { get; set; }
        public string ImageBase64 { get; set; }
        public bool Selected { get; internal set; }

        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Alcoholic = product.Alcoholic;

            if (product.image != null)
            {
                ImageBase64 = Convert.ToBase64String(product.image);
            }
        }
        public string GetImageBase64()
        {
            return ImageBase64;
        }
    }
}
