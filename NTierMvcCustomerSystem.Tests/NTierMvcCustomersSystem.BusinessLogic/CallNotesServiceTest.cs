using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.BusinessLogic.Common;
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
        private string _customersFilePath;
        private string _customersFileName;

        private string _filePath;
        private string _fileName;

        private string _userName;
        private string _notExistingUserName;

        private JObject _jObject;
        private CallNote _callNote1;
        private CallNote _callNote2;
        private ChildCallNote _childCallNote1;
        private ChildCallNote _childCallNote2;
        private IList<CallNote> _callNotes;
        private CallNotesService _callNotesService;


        [TestInitialize]
        public void TestInitialize()
        {
            _customersFilePath = Path.Combine(TestConstants.DataSourcePath, TestConstants.DataSourcePathSegment);
            _customersFileName = TestConstants.CustomerFileName;

            _filePath = Path.Combine(TestConstants.DataSourcePath, TestConstants.DataSourcePathSegment, TestConstants.CallNoteContentFolderName);
            _fileName = "whisper.json";

            _userName = "whisper";
            _notExistingUserName = "whisperAAA";

            _callNote1 = new CallNote
            {
                NoteTime = DateTime.ParseExact("02/02/1988 21:14:14", Constants.DateTimeFormat, null),
                NoteContent = "CallNote1",
                ChildCallNotes = new List<ChildCallNote>
                {
                    new ChildCallNote
                    {
                        NoteTime = DateTime.ParseExact("02/02/1988 23:14:14", Constants.DateTimeFormat, null),
                        NoteContent = "CallNote12"
                    },
                    new ChildCallNote
                    {
                        NoteTime = DateTime.ParseExact("02/02/1988 23:34:14", Constants.DateTimeFormat, null),
                        NoteContent = "CallNote123"
                    }
                }
            };

            _callNote2 = new CallNote
            {
                NoteTime = DateTime.ParseExact("02/02/2011 21:14:14", Constants.DateTimeFormat, null),
                NoteContent = "CallNote2",
                ChildCallNotes = null
            };

            _childCallNote1 = new ChildCallNote
            {
                NoteTime = DateTime.ParseExact("02/02/1977 23:14:14", Constants.DateTimeFormat, null),
                NoteContent = "ChildNote12"
            };

            _childCallNote2 = new ChildCallNote
            {
                NoteTime = DateTime.ParseExact("02/02/1976 23:22:14", Constants.DateTimeFormat, null),
                NoteContent = "ChildNote22"
            };

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

            _jObject = JObject.Parse(@"{
                                          'Customers': [
                                            {
                                              'Id': 1,
                                              'UserName': 'whisper',
                                              'FirstName': 'sdfdsf',
                                              'LastName': 'sdfsdfsd',
                                              'PhoneNumber': '0444444444',
                                              'DateOfBirth': '09/11/2016',
                                              'CallNoteName': 'whisper.json'
                                            },
                                            {
                                              'Id': 2,
                                              'UserName': 'wing',
                                              'FirstName': 'AAA',
                                              'LastName': 'BBBCD',
                                              'PhoneNumber': '0444321321',
                                              'DateOfBirth': '01/01/2000',
                                              'CallNoteName': 'wing.json'
                                            },
                                            {
                                              'Id': 3,
                                              'UserName': 'water',
                                              'FirstName': 'CC',
                                              'LastName': 'BC',
                                              'PhoneNumber': '0422159753',
                                              'DateOfBirth': '01/01/1995',
                                              'CallNoteName': 'water.json'
                                            }
                                          ]
                                        }");

            _callNotesService = new CallNotesService(_customersFilePath, _customersFileName);

            Directory.CreateDirectory(_filePath);

            File.Delete(Path.Combine(_customersFilePath, _customersFileName));
            // Create customers file for the use of CallNotesService
            CreateAndWriteCustomersFile();

            // Delete current call note json file if exist
            File.Delete(Path.Combine(_filePath, _fileName));

            // Write the call notes to file before doing next operation
            WriteCallNotesToFile(_filePath, _fileName, _callNotes);
        }



        [TestMethod]
        public void GetAllCallNotes_ExistingUserName_GetAllCallNotesCorrectly()
        {
            var allCallNotes = _callNotesService.GetAllCallNotes(_userName);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void GetAllCallNotes_NullUserName_ThrowBusinessLogicException()
        {
            _callNotesService.GetAllCallNotes(null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void GetAllCallNotes_NotExistingUserName_ThrowBusinessLogicException()
        {
            _callNotesService.GetAllCallNotes(_notExistingUserName);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void GetAllCallNotes_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_customersFilePath, _customersFileName));
            _callNotesService.GetAllCallNotes(_userName);
        }

        [TestMethod]
        public void GetAllCallNotes_CallNotesFileNotFound_GetEmptyCallNotes()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            var callNotes = _callNotesService.GetAllCallNotes(_userName);
            Assert.IsNotNull(callNotes);
            Assert.IsTrue(callNotes.Count == 0);
        }



        [TestMethod]
        public void WriteCallNotes_ExistingUserNameAndNotNullCallNote_WriteCorrectly()
        {
            _callNotesService.WriteCallNote(_userName, _callNote1);

            var allCallNotes = ReadAllCallNotesFromFile();

            _callNotes.Add(_callNote1);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteCallNotes_NullUserName_ThrowBusinessLogicException()
        {
            _callNotesService.WriteCallNote(null, _callNote1);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteCallNotes_NullCallNote_ThrowBusinessLogicException()
        {
            _callNotesService.WriteCallNote(_userName, null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteCallNotes_NotExistingUserName_ThrowBusinessLogicException()
        {
            _callNotesService.WriteCallNote(_notExistingUserName, _callNote2);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteCallNotes_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_customersFilePath, _customersFileName));
            _callNotesService.WriteCallNote(_userName, _callNote2);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void WriteCallNotes_CallNotesFileNotFound_ThrowFileNotFoundException()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _callNotesService.WriteCallNote(_userName, _callNote1);
        }



        [TestMethod]
        public void WriteChildCallNotes_ExistingUserNameAndChildNotesInCallNotesNotNull_WriteCorrectly()
        {
            _callNotesService.WriteChildCallNote(_userName, _childCallNote1);

            var allCallNotes = ReadAllCallNotesFromFile();

            _callNotes[_callNotes.Count - 1].ChildCallNotes.Add(_childCallNote1);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        public void WriteChildCallNotes_ExistingUserNameAndTheLastChildNotesInCallNotesIsNull_WriteCorrectly()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            // Let ChildNotes in CallNotes[1] to be null
            _callNotes[_callNotes.Count - 1] = _callNote2;
            WriteCallNotesToFile(_filePath, _fileName, _callNotes);

            _callNotesService.WriteChildCallNote(_userName, _childCallNote2);

            var allCallNotes = ReadAllCallNotesFromFile();

            _callNote2.ChildCallNotes = new List<ChildCallNote> { _childCallNote2 };
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, allCallNotes));
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteChildCallNotes_NullUserName_ThrowBusinessLogicException()
        {
            _callNotesService.WriteChildCallNote(null, _childCallNote1);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteChildCallNotes_NullCallNote_ThrowBusinessLogicException()
        {
            _callNotesService.WriteChildCallNote(_userName, null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteChildCallNotes_NotExistingUserName_ThrowBusinessLogicException()
        {
            _callNotesService.WriteChildCallNote(_notExistingUserName, _childCallNote2);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void WriteChildCallNotes_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_customersFilePath, _customersFileName));
            _callNotesService.WriteChildCallNote(_userName, _childCallNote2);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void WriteChildCallNotes_CallNotesFileNotFound_ThrowFileNotFoundException()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _callNotesService.WriteChildCallNote(_userName, _childCallNote1);
        }


        [TestMethod]
        public void CreateCallNotesFile_ExistingUserName_FileCreated()
        {
            File.Delete(Path.Combine(_filePath, _fileName));

            _callNotesService.CreateCallNotesFile(_userName);
            Assert.IsTrue(File.Exists(Path.Combine(_filePath, _fileName)));

            var allCallNotes = ReadAllCallNotesFromFile();
            Assert.IsNotNull(allCallNotes);
            Assert.IsTrue(allCallNotes.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void CreateCallNotesFile_NullUserName_ThrowBusinessLogicException()
        {
            _callNotesService.CreateCallNotesFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void CreateCallNotesFile_NotExistingUserName_ThrowBusinessLogicException()
        {
            _callNotesService.CreateCallNotesFile(_notExistingUserName);
        }



        [TestMethod]
        public void DeleteCallNotes_ExistingUserName_WriteSuccessfully()
        {
            _callNotesService.DeleteCallNotes(_userName);

            Assert.IsFalse(File.Exists(Path.Combine(_filePath, _fileName)));
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void DeleteCallNotes_NullUserName_ThrowBusinessLogicException()
        {
            _callNotesService.DeleteCallNotes(null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void DeleteCallNotes_NotExistingUserName_ThrowBusinessLogicException()
        {
            _callNotesService.DeleteCallNotes(_notExistingUserName);
        }

        


        private IList<CallNote> ReadAllCallNotesFromFile()
        {
            JObject jObject;
            using (StreamReader file = File.OpenText(Path.Combine(_filePath, _fileName)))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObject = (JObject)JToken.ReadFrom(reader);
                }
            }

            return jObject["CallNotes"].Select(JTokenToCallNote).ToList();
        }


        private void CreateAndWriteCustomersFile()
        {
            using (StreamWriter file = File.CreateText(Path.Combine(_customersFilePath, _customersFileName)))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    _jObject.WriteTo(writer);
                }
            }
        }



        private static void WriteCallNotesToFile(string filePath, string fileName, IList<CallNote> callNotes)
        {
            var jArray = new JArray();
            var jObject = new JObject { ["CallNotes"] = jArray };

            foreach (var callNote in callNotes)
            {
                if (callNote != null)
                {
                    jArray.Add(CallNoteToJToken(callNote));
                }
            }

            Directory.CreateDirectory(filePath);

            var fullFileName = Path.Combine(filePath, fileName);
            using (StreamWriter file = File.CreateText(fullFileName))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    jObject.WriteTo(writer);
                }
            }
        }

        private static JToken CallNoteToJToken(CallNote callNote)
        {
            if (callNote == null)
            {
                return null;
            }

            var jArray = new JArray();
            var jObject = new JObject
            {
                ["NoteTime"] = callNote.NoteTime.ToString(Constants.DateTimeFormat),
                ["NoteContent"] = callNote.NoteContent,
                ["ChildCallNotes"] = jArray
            };

            var callNoteChildCallNotes = callNote.ChildCallNotes;
            if (callNoteChildCallNotes == null)
            {
                return jObject;
            }

            foreach (var callNoteChildCallNote in callNoteChildCallNotes)
            {
                if (callNoteChildCallNote != null)
                {
                    jArray.Add(new JObject
                    {
                        ["NoteTime"] = callNoteChildCallNote.NoteTime.ToString(Constants.DateTimeFormat),
                        ["NoteContent"] = callNoteChildCallNote.NoteContent
                    });
                }
            }

            return jObject;
        }

        private static CallNote JTokenToCallNote(JToken callNoteJObject)
        {
            if (callNoteJObject == null)
            {
                return null;
            }

            var callNote = new CallNote
            {
                NoteTime = DateTime.ParseExact((string)callNoteJObject["NoteTime"], Constants.DateTimeFormat, null),
                NoteContent = (string)callNoteJObject["NoteContent"],
                ChildCallNotes = new List<ChildCallNote>()
            };

            var jArray = (JArray)callNoteJObject["ChildCallNotes"];
            foreach (var jToken in jArray)
            {
                var childCallNote = new ChildCallNote
                {
                    NoteTime = DateTime.ParseExact((string)jToken["NoteTime"], Constants.DateTimeFormat, null),
                    NoteContent = (string)jToken["NoteContent"]
                };
                callNote.ChildCallNotes.Add(childCallNote);
            }

            return callNote;
        }
    }
}
