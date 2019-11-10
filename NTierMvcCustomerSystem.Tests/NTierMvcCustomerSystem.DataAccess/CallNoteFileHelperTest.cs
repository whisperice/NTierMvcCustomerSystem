using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
        private string _filePath;
        private List<CallNote> _callNotes;

        [TestInitialize]
        public void TestInitialize()
        {
            _fileName = "whisper.json";
            _filePath = Path.Combine(TestConstants.DataSourcePath, TestConstants.DataSourcePathSegment, TestConstants.CallNoteContentFolderName);
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

            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));
        }

        [TestMethod]
        public void WriteCallNotes_NotNullPathAndName_WriteCorrectly()
        {
            CallNoteFileHelper.WriteCallNotes(_filePath, _fileName, _callNotes);

            // read the json file after write, in order to do comparison
            var fullFileName = Path.Combine(_filePath, _fileName);
            JObject jObject;
            using (StreamReader file = File.OpenText(fullFileName))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObject = (JObject)JToken.ReadFrom(reader);
                }
            }
            var callNotes = jObject["CallNotes"].Select(JTokenToCallNote).ToList();

            Assert.IsTrue(Utilities.EqualsAll(_callNotes, callNotes));
        }

        [TestMethod]
        public void ReadCallNotes_NotNullPathAndName_ReadCorrect()
        {
            // write the json file before do the reading
            var jArray = new JArray();
            var jObject = new JObject { ["CallNotes"] = jArray };

            foreach (var callNote in _callNotes.Where(callNote => callNote != null))
            {
                jArray.Add(CallNoteToJToken(callNote));
            }

            var fullFileName = Path.Combine(_filePath, _fileName);
            using (StreamWriter file = File.CreateText(fullFileName))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    jObject.WriteTo(writer);
                }
            }



            var readCallNotes = CallNoteFileHelper.ReadCallNotes(_filePath, _fileName);
            Assert.IsTrue(Utilities.EqualsAll(_callNotes, readCallNotes));
        }

        [TestMethod]
        public void DeleteCallNotes_NotNullPathAndName_FileDeleted()
        {
            CallNoteFileHelper.DeleteCallNotes(_filePath, _fileName);
            Assert.IsFalse(File.Exists(Path.Combine(_filePath, _fileName)));
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
    }

}
