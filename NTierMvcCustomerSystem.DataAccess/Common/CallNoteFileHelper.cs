﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.DataAccess.Common
{
    /// <summary>
    /// The Helper Class to read and write a call note file which store the call note content of a customer.
    /// </summary>
    public static class CallNoteFileHelper
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static IList<CallNote> ReadContentFile(string filePath, string fileName)
        {
            return null;
        }

        public static void WriteCallNote(string filePath, string fileName, IList<CallNote> callNotes)
        {

        }

        public static void WriteCallNoteAtIndex(string filePath, string fileName, CallNote callNote)
        {

        }

    }
}