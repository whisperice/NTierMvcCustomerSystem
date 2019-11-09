using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTierMvcCustomerSystem.BusinessLogic.Services;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.Model;
using PagedList;

namespace NTierMvcCustomerSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomersService _customersService = new CustomersService();
        private readonly CallNotesService _callNotesService = new CallNotesService();
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            ViewBag.IsNotUnique = false;
            _logger.Info("[CustomersController::Create] Creating Customer: {}.", customer);

            if (!ModelState.IsValid)
            {
                _logger.Info("[CustomersController::Create] Parameters not valid: {}.", customer);

                return View();
            }

            try
            {
                var isInserted = _customersService.Insert(customer, out var id);
                customer.Id = id;

                if (!isInserted)
                {
                    ViewBag.IsNotUnique = true;
                    _logger.Warn("[CustomersController::Create] User Name should be unique. Details: {} ", customer);
                    return View();
                }

                _logger.Info("[CustomersController::Create] Creating Customer Successfully. Details: {} ", customer);
                return RedirectToAction("Details", new { id });
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Create] Creating New Customer Failed.");
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id, string userName)
        {
            var customer = new Customer { Id = id, UserName = userName };
            return View(customer);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customer)
        {
            _logger.Info("[CustomersController::Edit] Editing Customer with id :{}.", id);

            if (!ModelState.IsValid)
            {
                _logger.Info("[CustomersController::Edit] Parameters not valid: {}.", customer);

                return View();
            }

            try
            {
                var isUpdated = _customersService.Update(customer);

                if (!isUpdated)
                {
                    // todo: indicate the id is invalid
                    _logger.Warn("[CustomersController::Edit] Editing Customer with id :{} failed. Details: {} ", id, customer);
                    return View("ItemNotFound");
                }

                _logger.Info("[CustomersController::Edit] Editing Customer with id :{} Successfully. Details: {} ", id, customer);
                return RedirectToAction("Details", new { id });
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Edit] Editing Customer with id :{} failed.", id);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _logger.Info("[CustomersController::Delete] Deleting Customers with Id: {} Before Delete Confirmed..", id);

            try
            {
                _logger.Info("[CustomersController::Delete] Finding Details of Customers with Id: {} Before Delete Confirmed.", id);

                var customer = _customersService.SelectById(id);

                if (customer == null)
                {
                    _logger.Warn("[CustomersController::Delete] Can not find Details of Customers with Id: {} Before Delete Confirmed. ", id);
                    return View("ItemNotFound");
                }

                _logger.Info("[CustomersController::Delete] Finding Details of Customers with Id: {} Found Before Delete Confirmed. Details: {} ", id, customer);
                return View(customer);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Delete] Finding Details of Customer with Id: {} Before Delete Confirmed Error.", id);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Customer customer)
        {
            _logger.Info("[CustomersController::Delete] Deleting Customers with Id: {} After Delete Confirmed.", id);

            try
            {
                var isDeleted = _customersService.DeleteById(id);

                if (!isDeleted)
                {
                    _logger.Warn("[CustomersController::Delete] Deleting Customer with id :{} After Delete Confirmed Failed.", id);

                    return View("ItemNotFound");
                }

                _logger.Info("[CustomersController::Delete] Deleting Customer with id :{} After Delete Confirmed Successfully. Details: {} ", id, customer);

                // todo: indicate delete successfully
                return RedirectToAction("ListAll");
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Delete] Deleting Customer with Id: {} After Delete Confirmed Error.", id);
                return View("Error");
            }
        }

        public ActionResult Search(string currentFilter, string searchString, int? page)
        {
            _logger.Info("[CustomersController::Search] Searching Customers. Search key word: {}", searchString);

            try
            {
                if (searchString != null)
                {
                    page = Constants.FirstPage;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

                IEnumerable<Customer> customers;

                if (!string.IsNullOrEmpty(searchString))
                {
                    customers = from customer in _customersService.SelectByFirstOrLastName(searchString)
                                orderby customer.Id
                                select customer;
                }
                else
                {
                    customers = from customer in _customersService.SelectAll()
                                orderby customer.Id
                                select customer;
                }

                int pageSize = Constants.PageSize;
                int pageNumber = page ?? Constants.FirstPage;

                _logger.Info("[CustomersController::Search] Finding All Customers Successfully. Search key word: {}", searchString);
                return View(customers.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Search] Searching Customers Error. Search key word: {}", searchString);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult ListAll()
        {
            _logger.Info("[CustomersController::ListAll] Finding All Customers.");

            try
            {
                var customers = from customer in _customersService.SelectAll()
                                orderby customer.Id
                                select customer;

                _logger.Info("[CustomersController::ListAll] Finding All Customers Successfully.");
                return View(customers);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::ListAll] Finding All Customers Error.");
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            _logger.Info("[CustomersController::Details] Finding Details of Customers with Id: {}.", id);

            try
            {
                var customer = _customersService.SelectById(id);

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
                _logger.Error(e, "[CustomersController::Details] Finding Details of Customer with Id: {} Error.", id);
                return View("Error");
            }

        }

        public ActionResult NoteDetails(int id, string userName)
        {
            ViewBag.Id = id;

            // try
            return View();
        }

        public ActionResult AddNote()
        {
            // try
            return View("NoteDetails");
        }

        public ActionResult AddChildNote()
        {
            // try
            return View("NoteDetails");
        }
    }
}