using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}
        
        [Display(Name="Name")]
        public string Name {get;set;}
        
        [Display(Name="Description")]
        public string Description {get;set;}
        
        [Display(Name="Price")]
        public float Price {get;set;}

        public List<Association> Categories {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}