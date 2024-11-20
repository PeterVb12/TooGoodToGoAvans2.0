using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooGoodToGoAvans.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Alcoholic { get; set; }
        public byte[] image { get; set; }

        public ICollection<Package>? Packages { get; set; }

        public Product(Guid id, string name, bool alcoholic, byte[] image)
        {
            Id = id;
            Name = name;
            Alcoholic = alcoholic;
            this.image = image;
        }
    }
}
