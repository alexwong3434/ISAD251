using Bar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bar.ViewModels
{
    public class OrderLookupViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public List<Item> ItemsList { get; set; }
    }
}