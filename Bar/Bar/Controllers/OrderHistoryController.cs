using Bar.Models;
using Bar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bar.Controllers
{
    public class OrderHistoryController : Controller
    {
        readonly ApplicationDbContext storeDB = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: OrderHistory
        public ActionResult Index()
        {
            return View(storeDB.Orders.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var order = storeDB.Orders
                                     .Single(Orderid => Orderid.OrderId == id);

            var orderDetail = storeDB.OrderDetails
                              .Where(orderdetail => orderdetail.OrderId == id);

            var itemsIdList = (from orderdetail in orderDetail
                               select orderdetail.ItemId).ToList();

            var itemsList = storeDB.Items
                              .Where(t => itemsIdList.Contains(t.ID));

            OrderLookupViewModel viewModel = new OrderLookupViewModel();
            viewModel.Order = order;
            viewModel.OrderDetails = orderDetail.ToList();
            viewModel.ItemsList = itemsList.ToList();
            return View(viewModel);

        }
    }
}