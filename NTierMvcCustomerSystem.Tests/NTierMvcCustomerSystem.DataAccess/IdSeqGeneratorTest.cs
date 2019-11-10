using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.DataAccess.Models;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class IdSeqGeneratorTest
    {

        private readonly string _idSeqFilePath = ConfigurationHandler.GetDataSourcePath();
        private readonly string _idSeqFileName = Constants.IdSeqFileName;

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory(_idSeqFilePath);
            File.Delete(Path.Combine(_idSeqFilePath, _idSeqFileName));
        }


        [TestMethod]
        public void GetIdSeq_IdSeqFileNotExist_GetCorrectIdSeq()
        {
            Assert.AreEqual(Constants.InitIdSeq + 1, IdSeqGenerator.GetIdSeq());
        }

        [TestMethod]
        public void GetIdSeq_IdSeqFileExist_GetCorrectIdSeq()
        {

            var initIdSeq = Constants.InitIdSeq + 100;

            var jObject = new JObject { ["IdSeq"] = initIdSeq };
            var fullFileName = Path.Combine(_idSeqFilePath, _idSeqFileName);
            using (StreamWriter file = File.CreateText(fullFileName))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    jObject.WriteTo(writer);
                }
            }

            Assert.AreEqual(initIdSeq + 1, IdSeqGenerator.GetIdSeq());
        }

    }
}
