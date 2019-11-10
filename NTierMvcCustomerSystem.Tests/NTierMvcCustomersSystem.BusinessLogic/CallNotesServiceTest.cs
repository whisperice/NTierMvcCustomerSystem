using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTierMvcCustomerSystem.BusinessLogic.Services;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.Model;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomersSystem.BusinessLogic
{
    [TestClass]
    public class CallNotesServiceTest
    {
        private string _path;
        private string _fileName;
        private string _userName;
        private IList<CallNote> _callNotes;
        private CallNotesService _callNotesService;

        [TestInitialize]
        public void TestInitialize()
        {
            _path = Path.Combine(TestConstants.DataSourcePath, TestConstants.CallNoteContentFolderName);
            _fileName = "whisper.json";
            _userName = "whisper";
            _callNotes = new List<CallNote>
            {
                new CallNote
                {
                    NoteTime = DateTime.ParseExact("02/02/1999 21:14:14", Constants.DateTimeFormat, null),
                    NoteContent = "Note1",
                    ChildCallNotes = new List<ChildCallNote>
                    {
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/1999 23:14:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note12"
                        },
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/1999 23:34:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note123"
                        }
                    }
                },
                new CallNote
                {
                    NoteTime = DateTime.ParseExact("02/02/2000 21:14:14", Constants.DateTimeFormat, null),
                    NoteContent = "Note2",
                    ChildCallNotes = new List<ChildCallNote>
                    {
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/2000 23:14:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note22"
                        },
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/2000 23:34:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note223"
                        }
                    }
                }
            };
            CallNoteFileHelper.WriteCallNotes(_path, _fileName, _callNotes);

            _callNotesService = new CallNotesService(TestConstants.DataSourcePath, TestConstants.CustomerFileName);
        }

        [TestMethod]
        public void GetAllCallNotes_ExistUserName_GetAllCallNotes()
        {
            var allCallNotes = _callNotesService.GetAllCallNotes(_userName);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        public void WriteCallNotes_ExistUserName_WriteSuccess()
        {
            try
            {
                _callNotesService.WriteCallNote(_userName, _callNotes[0]);
            }
            catch (Exception)
            {
                Assert.Fail("Write Call Note Failed.");
            }

            var allCallNotes = _callNotesService.GetAllCallNotes(_userName);
            _callNotes.Add(_callNotes[0]);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        public void WriteChildCallNotes_ExistUserName_WriteSuccess()
        {
            var childCallNote = _callNotes[0].ChildCallNotes[0];
            try
            {
                _callNotesService.WriteChildCallNote(_userName, childCallNote);
            }
            catch (Exception)
            {
                Assert.Fail("Write Child Call Note Failed.");
            }

            var allCallNotes = _callNotesService.GetAllCallNotes(_userName);
            _callNotes[_callNotes.Count - 1].ChildCallNotes.Add(childCallNote);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        public void DeleteCallNotes_ExistUserName_WriteSuccess()
        {
            try
            {
                _callNotesService.DeleteCallNotes(_userName);
            }
            catch (Exception)
            {
                Assert.Fail("Write Child Call Note Failed.");
            }

            Assert.IsFalse(File.Exists(Path.Combine(_path, _fileName)));
        }
    }
}
