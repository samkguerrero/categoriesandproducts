using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAndCategories.Models
{
    public class ShowProduct
    {
        public Product Product {get;set;}
        public Category Category {get;set;}

    }
}