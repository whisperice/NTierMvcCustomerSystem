using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.DataAccess.Common
{
    /// <summary>
    /// The Helper Class to read and write a call note file which store the call note content of a customer.
    /// </summary>
    public static class CallNoteFileHelper
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static IList<CallNote> ReadCallNotes(string filePath, string fileName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[CallNoteFileHelper::ReadCallNotes] Starting reading all CallNotes. FilePath: {}, FileName: {}", filePath, fileName);
            }

            try
            {
                var jObject = JsonFileHelper.ReadJsonFile(filePath, fileName);

                var callNotes = jObject["CallNotes"].Select(JTokenToCallNote).ToList();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug(
                        "[CallNoteFileHelper::ReadCallNotes] Reading all CallNotes Successfully. callNotes: {}",
                        callNotes);
                }

                return callNotes;
            }
            catch (FileNotFoundException e)
            {
                Logger.Error(e,
                    "[CallNoteFileHelper::ReadCallNotes] Reading all CallNotes Failed. Can not find file. FilePath: {}, FileName: {}",
                    filePath, fileName);
                throw new FileNotFoundException("[CallNoteFileHelper::ReadCallNotes] Reading all CallNotes Failed. Can not find file.", e);
            }
            catch (Exception e)
            {
                Logger.Error(e,
                    "[CallNoteFileHelper::ReadCallNotes] Reading all CallNotes Failed. FilePath: {}, FileName: {}",
                    filePath, fileName);
                throw new DataAccessException("[CallNoteFileHelper::ReadCallNotes] Reading all CallNotes Failed.", e);
            }
        }

        public static void WriteCallNotes(string filePath, string fileName, IList<CallNote> callNotes)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[CallNoteFileHelper::WriteCallNotes] Starting writing all CallNotes. FilePath: {}, FileName: {}", filePath, fileName);
            }

            try
            {
                if (callNotes == null)
                {
                    throw new ArgumentNullException(nameof(callNotes), "callNotes should not be null.");
                }

                var jArray = new JArray();
                var jObject = new JObject {["CallNotes"] = jArray};

                foreach (var callNote in callNotes)
                {
                    if (callNote != null)
                    {
                        jArray.Add(CallNoteToJToken(callNote));
                    }
                }

                JsonFileHelper.WriteJsonFile(filePath, fileName, jObject);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug(
                        "[CallNoteFileHelper::WriteCallNotes] Writing all CallNotes Successfully. callNotes: {}",
                        callNotes);
                }
            }
            catch (FileNotFoundException e)
            {
                Logger.Error(e,
                    "[CallNoteFileHelper::WriteCallNotes] Writing all CallNotes Failed. Can not find file. FilePath: {}, FileName: {}",
                    filePath, fileName);
                throw new FileNotFoundException("[CallNoteFileHelper::WriteCallNotes] Writing all CallNotes Failed. Can not find file.", e);
            }
            catch (Exception e)
            {
                Logger.Error(e,
                    "[CallNoteFileHelper::WriteCallNotes] Writing all CallNotes Failed. FilePath: {}, FileName: {}",
                    filePath, fileName);
                throw new DataAccessException("[CallNoteFileHelper::WriteCallNotes] Writing all CallNotes Failed.", e);
            }
        }

        public static void DeleteCallNotes(string filePath, string fileName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[CallNoteFileHelper::DeleteCallNotes] Deleting single CallNotes file. FilePath: {}, FileName: {}", filePath, fileName);
            }

            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath), "The file path can't be Null or Empty.");
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName), "The file path can't be Null or Empty.");
                }

                var fullFileName = Path.Combine(filePath, fileName);
                File.Delete(fullFileName);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug(
                        "[CallNoteFileHelper::DeleteCallNotes] Delete single CallNotes file Successfully. fullFileName: {}",
                        fullFileName);
                }
            }
            catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
            {
                Logger.Error(e,
                    "[CallNoteFileHelper::DeleteCallNotes] Delete single CallNotes file Failed. Can not find file. FilePath: {}, FileName: {}",
                    filePath, fileName);
                throw new FileNotFoundException(
                    "[CallNoteFileHelper::DeleteCallNotes] Delete single CallNotes file Failed. Can not find file.", e);
            }
            catch (Exception e)
            {
                Logger.Error(e,
                    "[CallNoteFileHelper::DeleteCallNotes] Delete single CallNotes file Failed. FilePath: {}, FileName: {}",
                    filePath, fileName);
                throw new DataAccessException(
                    "[CallNoteFileHelper::DeleteCallNotes] Delete single CallNotes file Failed.", e);
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
