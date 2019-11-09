using System;
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
        private CustomersService customersService = new CustomersService();
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

            return View();
//            if (!ModelState.IsValid)
//            {
//                return View();
//            }
//
//            try
//            {
//                InsertEmployee(int.Parse(collection["Id"]),
//                    collection["Name"],
//                    int.Parse(collection["Age"]),
//                    collection["HiringDate"].Trim().Length == 0
//                        ? (DateTime?)null
//                        : DateTime.ParseExact(collection["HiringDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
//                    decimal.Parse(collection["GrossSalary"]));
//
//                return RedirectToAction("ListAll");
//            }
//            catch (Exception ex)
//            {
//                _loggingHandler.LogEntry(ExceptionHandler.GetExceptionMessageFormatted(ex), true);
//                ViewBag.Message = Server.HtmlEncode(ex.Message);
//                return View("Error");
//            }
        }

        // GET: Customers/Edit
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customers/Edit
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            _logger.Info("[CustomersController::Edit] Editing Customer with id :{}.", id);

            try
            {
                var customerSelectById = customersService.SelectById(id);
                customer.UserName = customerSelectById.UserName;
                var isUpdated = customersService.Update(customer);

                if (!isUpdated)
                {
                    // todo: indicate the id is invalid
                    _logger.Warn("[CustomersController::Edit] Editing Customer with id :{} failed. Details: {} ", id, customer);
                    return View("ItemNotFound");
                }

                _logger.Info("[CustomersController::Edit] Editing Customer with id :{} Successfully. Details: {} ", id, customer);
                return RedirectToAction("Details", new {id});
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::Edit] Editing Customer with id :{} failed.", id);
                return View("Error");
            }
        }

        // GET: Customers/Delete/id
        public ActionResult Delete(int id)
        {
            _logger.Info("[CustomersController::Delete] Deleting Customers with Id: {} Before Delete Confirmed..", id);

            try
            {
                _logger.Info("[CustomersController::Delete] Finding Details of Customers with Id: {} Before Delete Confirmed.", id);
                
                var customer = customersService.SelectById(id);

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

        // POST: Employees/Delete/id
        [HttpPost]
        public ActionResult Delete(int id, Customer customer)
        {
            _logger.Info("[CustomersController::Delete] Deleting Customers with Id: {} After Delete Confirmed.", id);

            try
            {
                var isDeleted = customersService.DeleteById(id);

                if (!isDeleted)
                {
                    _logger.Warn("[CustomersController::Delete] Deleting Customer with id :{} After Delete Confirmed Failed.", id);

                    return View("ItemNotFound");
                }

                _logger.Info("[CustomersController::Delete] Deleting Customer with id :{} After Delete Confirmed Successfully. Details: {} ", id, customer);

                // todo: indicate delete successfully
                return RedirectToAction("ListAll");
            }
            catch(Exception e)
            {
                _logger.Error(e, "[CustomersController::Delete] Deleting Customer with Id: {} After Delete Confirmed Error.", id);
                return View("Error");
            }
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
            _logger.Info("[CustomersController::ListAll] Finding All Customers.");

            try
            {
                var customers = from customer in customersService.SelectAll()
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

        public ActionResult AddNote()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            _logger.Info("[CustomersController::Details] Finding Details of Customers with Id: {}.", id);

            try
            {
                var customer = customersService.SelectById(id);

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
    }
}