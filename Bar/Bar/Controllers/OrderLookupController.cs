﻿using Bar.Models;
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
            if (searchid.HasValue) //do nothing when sumbit empty input
            {
                try                //try to match record from db with orderID
                {
                    var order = storeDB.Orders
                                     .Single(Orderid => Orderid.OrderId == searchid);

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

                catch (Exception)
                                    //when fail to match any record
                                    //show "not found" message beside search button
                {
                    ViewBag.result = "not found";
                    return View();
                }

            }
            return View();
        }
    }
}