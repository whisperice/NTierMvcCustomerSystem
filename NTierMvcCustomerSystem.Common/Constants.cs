using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Common
{
    public static class Constants
    {
        public const int MinValidCustomerId = 1;

        public const int NotExistCustomerId = -1;

        public const int InitIdSeq = 100000;

        public const int AgeLimit = 110;

        public const int PageSize = 10;

        public const int FirstPage = 1;

        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public const string DateOfBirthTimeFormat = "dd/MM/yyyy";

        public const string DataSourcePathSegment = "DataSource";

        public const string DataSourcePathKey = "DataSourcePath";

        public const string CallNoteContentFolderName = "CallNote";

        public const string PostfixOfNoteFile = ".json";

        public const string CustomersFileName = "Customers.json";

        public const string IdSeqFileName = "IdSequence.json";

    }
}
