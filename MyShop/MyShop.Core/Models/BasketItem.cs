﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class BasketItem: BaseObjectEntity
    {

        public string BasketId { get; set; }
        public int quantity { get; set; }
        public string ProductId { get; set; }

    }
}
