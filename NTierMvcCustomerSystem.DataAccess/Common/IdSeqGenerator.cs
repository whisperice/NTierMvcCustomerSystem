using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;

namespace NTierMvcCustomerSystem.DataAccess.Common
{
    public static class IdSeqGenerator
    {
        private static readonly string IdSeqFileName = Constants.IdSeqFileName;
        private static readonly string IdSeqFilePath = ConfigurationHandler.GetDataSourcePath();

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static int GetIdSeq()
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[IdSeqGenerator::GetIdSeq] Getting IdSeq.");
            }

            try
            {
                var jObj = JsonFileHelper.ReadJsonFile(IdSeqFilePath, IdSeqFileName);
                var idSeq = (int)jObj["IdSeq"];
                idSeq++;
                jObj["IdSeq"] = idSeq;
                JsonFileHelper.WriteJsonFile(IdSeqFilePath, IdSeqFileName, jObj);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("[IdSeqGenerator::GetIdSeq] Get IdSeq successfully. IdSeq: {}", idSeq);
                }
                return idSeq;
            }
            catch (Exception e)
            {
                Logger.Error(e, "[IdSeqGenerator::GetIdSeq] Get IdSeq error.");
                throw new DataAccessException("IdSeqGenerator::GetIdSeq: Get IdSeq error.", e);
            }
        }
    }
}
