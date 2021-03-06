﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyShop.Core.Models{
    public class Product : BaseEntity
    {
        //public string Id { get; set; } since BaseEntity already has an Id
        [StringLength(20)]
        [DisplayName("ProductName")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        //public Product()  //removed constructor since the creation of the Id is handled in the base class
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}
    }}