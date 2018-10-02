namespace Sales.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Products
    {
       
        public int ProductId { get; set; }

        
        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublishOn { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
