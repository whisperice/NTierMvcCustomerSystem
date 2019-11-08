﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTierMvcCustomerSystem.BusinessLogic.Implementation;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.Controllers
{
    public class CustomersController : Controller
    {

        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return RedirectToAction("ListAll");
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return View();
        }

        // GET: Customers/Edit
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Customers/Edit
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            return View();
        }

        // GET: Customers/Delete/id
        public ActionResult Delete(int id)
        {
            var customer = GetAllCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customers/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: Customers/Search
        [HttpPost]
        public ActionResult Search(Customer customer)
        {
            return View();
        }

        // GET: Customers/ListAll
        public ActionResult ListAll()
        {
            _logger.Info("[CustomersController::ListAll] Request for All Customers.");

            try
            {
                var customers = from customer in GetAllCustomers()
                                orderby customer.Id
                                select customer;

                _logger.Info("[CustomersController::ListAll] Request for All Customers Successfully.");
                return View(customers);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::ListAll] Request for All Customers Error.");
                return View("Error");
            }
        }

        private IEnumerable<Customer> GetAllCustomers()
        {
            var customersService = new CustomersService();
            return customersService.SelectAll();
        }

        public ActionResult AddNote()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            _logger.Info("[CustomersController::Details] Request for Details of Customers with Id: {}.", id);

            try
            {
                var customer = GetAllCustomers().SingleOrDefault(c => c.Id == id);

                if (customer == null)
                {
                    _logger.Info("[CustomersController::Details] Can not find Details of Customers with Id: {}.", id);
                    return View("ItemNotFound");
                }

                _logger.Info("[CustomersController::Details] Details of Customers with Id: {} Found. Details: {} ", id, customer);
                return View(customer);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Details] Request for Details of Customer with Id: {} Error.", id);
                return View("Error");
            }

        }
    }
}