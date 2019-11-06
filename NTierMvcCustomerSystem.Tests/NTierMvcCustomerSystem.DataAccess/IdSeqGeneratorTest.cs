using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void GetIdSeq_GivenIdSeqFileExist_GetCorrectIdSeq()
        {
            var idSeq = IdSeqGenerator.GetIdSeq();
            Console.WriteLine(idSeq);
        }
        
    }
}
