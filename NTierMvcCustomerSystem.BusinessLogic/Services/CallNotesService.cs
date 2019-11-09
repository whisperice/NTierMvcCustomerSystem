using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.LayoutRenderers;
using NTierMvcCustomerSystem.BusinessLogic.Common;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.DataAccess.Implementation;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.BusinessLogic.Services
{
    public class CallNotesService
    {
        private readonly string _callNotesFilePath;

        private readonly CustomersRepository _customersRepository;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public CallNotesService()
        {
            _customersRepository = new CustomersRepository();
            _callNotesFilePath = ConfigurationHandler.GetDataSourcePath() + Path.DirectorySeparatorChar +
                                 Constants.CallNoteContentFolderName;
        }

        public CallNotesService(string customersFilePath, string customersFileName)
        {
            _customersRepository = new CustomersRepository(customersFilePath, customersFileName);
            _callNotesFilePath = customersFilePath + Path.DirectorySeparatorChar + Constants.CallNoteContentFolderName;
        }

        public IList<CallNote> GetAllCallNotes(string userName)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CallNotesService::GetAllCallNotes] Starting getting all CallNotes. UserName: {}",
                    userName);
            }

            try
            {
                if (userName == null)
                {
                    throw new ArgumentNullException(nameof(userName), "User Name can not be null");
                }

                var customerEntity = _customersRepository.SelectByUserName(userName);
                if (customerEntity == null)
                {
                    throw new BusinessLogicException(
                        $"[CallNotesService::GetAllCallNotes] Can not find this User by userName. UserName: {userName}");
                }

                var callNotes = CallNoteFileHelper.ReadCallNotes(_callNotesFilePath, customerEntity.CallNoteName);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(
                        "[CallNotesService::GetAllCallNotes] Getting all CallNotes successfully. CallNotes: {}",
                        callNotes);
                }

                return callNotes;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CallNotesService::GetAllCallNotes] Getting all CallNotes failed. UserName: {}",
                    userName);
                throw new BusinessLogicException("[CallNotesService::GetAllCallNotes] Getting all CallNotes failed.",
                    e);
            }
        }

        public void WriteCallNote(string userName, CallNote callNote)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CallNotesService::WriteCallNote] Starting writing CallNote. UserName: {}, CallNote: {}",
                    userName, callNote);
            }

            try
            {
                if (userName == null)
                {
                    throw new ArgumentNullException(nameof(userName), "User Name can not be null");
                }

                if (callNote == null)
                {
                    throw new ArgumentNullException(nameof(callNote), "callNote can not be null");
                }

                var customerEntity = _customersRepository.SelectByUserName(userName);
                if (customerEntity == null)
                {
                    throw new BusinessLogicException(
                        $"[CallNotesService::GetAllCallNotes] Can not find this User by userName. UserName: {userName}");
                }

                var fileName = customerEntity.CallNoteName;
                var callNotes = CallNoteFileHelper.ReadCallNotes(_callNotesFilePath, fileName);
                callNotes.Add(callNote);
                CallNoteFileHelper.WriteCallNotes(_callNotesFilePath, fileName, callNotes);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(
                        "[CallNotesService::WriteCallNote] Writing CallNote successfully. UserName: {}, CallNote: {}",
                        userName, callNote);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,
                    "[CallNotesService::WriteCallNote] Writing CallNote failed. UserName: {}, CallNote: {}", userName,
                    callNote);
                throw new BusinessLogicException("[CallNotesService::WriteCallNote] Writing CallNote  failed.", e);
            }
        }

        public void WriteChildCallNotes(string userName, ChildCallNote childCallNote)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(
                    "[CallNotesService::WriteChildCallNotes] Starting writing ChildCallNote. UserName: {}, ChildCallNote: {}",
                    userName, childCallNote);
            }

            try
            {
                if (userName == null)
                {
                    throw new ArgumentNullException(nameof(userName), "User Name can not be null");
                }

                if (childCallNote == null)
                {
                    throw new ArgumentNullException(nameof(childCallNote), "childCallNote can not be null");
                }

                var customerEntity = _customersRepository.SelectByUserName(userName);
                if (customerEntity == null)
                {
                    throw new BusinessLogicException(
                        $"[CallNotesService::GetAllCallNotes] Can not find this User by userName. UserName: {userName}");
                }

                var fileName = customerEntity.CallNoteName;
                var callNotes = CallNoteFileHelper.ReadCallNotes(_callNotesFilePath, fileName);

                if (callNotes.Count == 0)
                {
                    throw new ArgumentException("Can not add childCallNote without CallNote.", nameof(childCallNote));
                }

                var lastCallNote = callNotes[callNotes.Count - 1];
                var childCallNotes = lastCallNote.ChildCallNotes ?? new List<ChildCallNote>();
                childCallNotes.Add(childCallNote);

                CallNoteFileHelper.WriteCallNotes(_callNotesFilePath, fileName, callNotes);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(
                        "[CallNotesService::WriteChildCallNotes] Writing ChildCallNote successfully. UserName: {}, ChildCallNote: {}",
                        userName, childCallNote);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,
                    "[CallNotesService::WriteChildCallNotes] Writing ChildCallNote failed. UserName: {}, ChildCallNote: {}",
                    userName, childCallNote);
                throw new BusinessLogicException(
                    "[CallNotesService::WriteChildCallNotes] Writing ChildCallNote failed.", e);
            }
        }

        public void DeleteCallNotes(string userName)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CallNotesService::DeleteCallNotes] Deleting a single CallNotes file. UserName: {}",
                    userName);
            }

            try
            {
                if (userName == null)
                {
                    throw new ArgumentNullException(nameof(userName), "User Name can not be null");
                }

                var customerEntity = _customersRepository.SelectByUserName(userName);
                if (customerEntity == null)
                {
                    throw new BusinessLogicException(
                        $"[CallNotesService::GetAllCallNotes] Can not find this User by userName. UserName: {userName}");
                }

                var fileName = customerEntity.CallNoteName;
                CallNoteFileHelper.DeleteCallNotes(_callNotesFilePath, fileName);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(
                        "[CallNotesService::DeleteCallNotes] Delete a single CallNotes file successfully. UserName: {}",
                        userName);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,
                    "[CallNotesService::DeleteCallNotes] Delete a single CallNotes file failed. UserName: {}",
                    userName);
                throw new BusinessLogicException(
                    "[CallNotesService::DeleteCallNotes] Delete a single CallNotes file failed.", e);
            }
        }
    }
}