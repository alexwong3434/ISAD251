using Bar.Models;
using Bar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bar.Controllers
{
    //[Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();

        // GET: Checkout
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult ConfirmOrder()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult ConfirmOrder(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                order.UserName = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                //Save Order
                storeDB.Orders.Add(order);
                    storeDB.SaveChanges();

                //Process the order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                if (order.Total == 0) //prevent user to Check out with empty cart 
                {
                    return View("Error");
                }
                else
                {
                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View("Error");
            }

        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.UserName == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}