using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTierMvcCustomerSystem.BusinessLogic.Services;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.Controllers
{
    public class CallNotesController : Controller
    {
        private readonly CustomersService _customersService = new CustomersService();
        private readonly CallNotesService _callNotesService = new CallNotesService();
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return RedirectToAction("Search", "Customers");
        }

        public ActionResult NoteDetails(int id, string userName)
        {
            ViewBag.Id = id;
            ViewBag.UserName = userName;

            _logger.Info("[CustomersController::NoteDetails] Getting NoteDetails of CallNotes. Id: {}, UserName {}", id, userName);

            try
            {
                var callNotes = _callNotesService.GetAllCallNotes(userName);

                if (callNotes == null)
                {
                    _logger.Info("[CustomersController::NoteDetails] Can not Get NoteDetails of CallNotes. Id: {}, UserName {}", id, userName);
                    return View("ItemNotFound");
                }

                ViewBag.ShowAddChildCallNote = callNotes.Count != 0;

                _logger.Info("[CustomersController::NoteDetails] CallNotes Details of Customers Found. Id: {}, UserName {}, Note: {}", id, userName, callNotes);
                return View(callNotes);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::NoteDetails] Finding CallNotes of Customer. Id: {}, UserName {}", id, userName);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult AddNote(int id, string userName)
        {
            ViewBag.Id = id;
            ViewBag.UserName = userName;
            _logger.Info("[CustomersController::AddNote] HttpGet Done. Id: {}, UserName {}", id, userName);
            return View();
        }

        [HttpPost, ActionName("AddNote")]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotePost(int id, string userName, CallNote callNote)
        {
            _logger.Info("[CustomersController::AddNotePost] Adding CallNote. Id: {}, UserName {}", id, userName);

            try
            {
                var note = new CallNote
                {
                    NoteTime = DateTime.Now,
                    NoteContent = callNote.NoteContent
                };

                try
                {
                    _callNotesService.WriteCallNote(userName, note);
                }
                catch (FileNotFoundException)
                {
                    // Create notes file if not found and the user is valid
                    var customer = _customersService.SelectById(id);
                    if (customer != null && string.Equals(customer.UserName, userName))
                    {
                        _callNotesService.CreateCallNotesFile(userName);
                        _callNotesService.WriteCallNote(userName, note);
                    }
                }

                _logger.Info("[CustomersController::AddNotePost] Adding CallNote Successfully. Id: {}, UserName {}, Note: {}", id, userName, note);
                return RedirectToAction("NoteDetails", new { id, userName });
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::AddNotePost] Adding CallNote Error. Id: {}, UserName: {}", id, userName);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult AddChildNote(int id, string userName)
        {
            ViewBag.Id = id;
            ViewBag.UserName = userName;
            _logger.Info("[CustomersController::AddChildNote] HttpGet Done. Id: {}, UserName {}", id, userName);
            return View();
        }

        [HttpPost, ActionName("AddChildNote")]
        [ValidateAntiForgeryToken]
        public ActionResult AddChildNotePost(int id, string userName, ChildCallNote childCallNote)
        {
            _logger.Info("[CustomersController::AddChildNote] Adding ChildCallNote. Id: {}, UserName {}", id, userName);

            try
            {
                var note = new ChildCallNote()
                {
                    NoteTime = DateTime.Now,
                    NoteContent = childCallNote.NoteContent
                };

                try
                {
                    _callNotesService.WriteChildCallNote(userName, note);
                }
                catch (FileNotFoundException)
                {
                    // Create notes file if not found and the user is valid
                    var customer = _customersService.SelectById(id);
                    if (customer != null && string.Equals(customer.UserName, userName))
                    {
                        _callNotesService.CreateCallNotesFile(userName);
                        // add as a call note since there is no call note
                        _callNotesService.WriteCallNote(userName, new CallNote
                        {
                            NoteTime = DateTime.Now,
                            NoteContent = childCallNote.NoteContent,
                            ChildCallNotes = new List<ChildCallNote>()
                        });
                    }
                }

                _logger.Info("[CustomersController::AddChildNote] Adding ChildCallNote Successfully. Id: {}, UserName: {}, Note: {}", id, userName, note);
                return RedirectToAction("NoteDetails", new { id, userName });
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersController::AddChildNote] Adding ChildCallNote Error. Id: {}, UserName: {}, ChildCallNote: {}", id, userName, childCallNote);
                return View("Error");
            }
        }
    }
}