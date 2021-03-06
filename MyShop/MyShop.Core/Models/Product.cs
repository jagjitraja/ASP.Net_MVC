﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Product : BaseObjectEntity
    {
        
        [DisplayName("Product Name")]
        [StringLength(20)]
        public string Name { get; set; }
        [DisplayName("Product Description")]
        [StringLength(50)]
        public string Description { get; set; }
        [Range(0,1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        public string ToString()
        {
            return "Name: "+Name + "\nDescription: " + Description + "\nPrice: " + Price + "\nCategory: " + Category + "\nImage: " + Image + "\n";
        }

    }
}
