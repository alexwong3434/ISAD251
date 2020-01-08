using Bar.Models;
using Bar.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Bar.Controllers
{
    public class OrderHistoryController : Controller
    {
        readonly ApplicationDbContext storeDB = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: OrderHistory
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.OrderIdSortParm = sortOrder == "OrderId" ? "orderid_desc" : "OrderId";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var orders = from o in storeDB.Orders
                           select o;

            switch (sortOrder)
            { 
                case "orderid_desc":
                    orders = orders.OrderByDescending(o => o.OrderId);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.OrderDate);
                    break;
                case "name_desc":
                    orders = orders.OrderByDescending(o => o.UserName);
                    break;
                default:
                    orders = orders.OrderBy(o => o.OrderId);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
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