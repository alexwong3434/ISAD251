using Bar.Models;
using Bar.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Bar.Controllers
{
    public class OrderLookupController : Controller
    {
        readonly ApplicationDbContext storeDB = new ApplicationDbContext();
        // GET: OrderLookup

        public ActionResult Index(int? searchid)

        {
            if (searchid.HasValue)
            {
                try
                {
                    var order = storeDB.Orders
                                     .Single(Orderid => Orderid.OrderId == searchid);

                    if (order == null)
                    {
                        return View();
                    }

                    else

                    {

                        var orderDetail = storeDB.OrderDetails
                                         .Where(item => item.OrderId == order.OrderId);

                        var itemsIdList = (from a in orderDetail
                                           select a.ItemId).ToList();

                        var itemsList = storeDB.Items
                                          .Where(t => itemsIdList.Contains(t.ID));

                        OrderLookupViewModel viewModel = new OrderLookupViewModel();
                        viewModel.Order = order;
                        viewModel.OrderDetails = orderDetail.ToList();
                        viewModel.ItemsList = itemsList.ToList();

                        return View(viewModel);
                    }
                }

                catch (Exception e)

                {
                    return View();
                }

            }

            return View();
        }
    }
}