using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.Model;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class CallNoteFileHelperTest
    {
        private string _fileName;
        private string _path;
        private List<CallNote> _callNotes;

        [TestInitialize]
        public void TestInitialize()
        {
            _fileName = "whisper.json";
            _path = Path.Combine(TestConstants.DataSourcePath, TestConstants.CallNoteContentFolderName);
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
        }

        [TestMethod]
        public void WriteCallNotes_NotNullPathAndName_WriteSuccess()
        {
            try
            {
                CallNoteFileHelper.WriteCallNotes(_path, _fileName, _callNotes);
            }
            catch (Exception)
            {
                Assert.Fail("WriteCallNotes Failed");
            }
        }

        [TestMethod]
        public void ReadCallNotes_NotNullPathAndName_ReadCorrect()
        {
            var readCallNotes = CallNoteFileHelper.ReadCallNotes(_path, _fileName);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, readCallNotes));
        }

        [TestMethod]
        public void DeleteCallNotes_NotNullPathAndName_FileDeleted()
        {
            CallNoteFileHelper.DeleteCallNotes(_path, _fileName);
            Assert.IsFalse(File.Exists(Path.Combine(_path, _fileName)));
        }
    }
}
