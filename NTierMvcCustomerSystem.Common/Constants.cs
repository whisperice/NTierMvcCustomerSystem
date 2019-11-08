using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Common
{
    public static class Constants
    {
        public const int MinValidCustomerId = 0;

        public const int NotExistCustomerId = -1;

        public const int AgeLimit = 110;

        public const string DateOfBirthTimeFormat = "dd/MM/yyyy";

        public const string DataSourcePathKey = "DataSourcePath";

        public const string CallNoteContentFolderName = "CallNote";

        public const string PostfixOfNoteFile = ".json";

        public const string CustomersFileName = "Customers.json";

        public const string IdSeqFileName = "IdSequence.json";

    }
}
