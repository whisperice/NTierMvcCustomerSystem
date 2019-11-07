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
        private static string _idSeqFileName = Constants.IdSeqFileName;
        private static string _idSeqFilePath = ConfigurationHandler.GetDataSourcePath();

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static int GetIdSeq()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[IdSeqGenerator::GetIdSeq] Getting IdSeq.");
            }

            try
            {
                var jObj = JsonFileHelper.ReadJsonFile(_idSeqFilePath, _idSeqFileName);
                var idSeq = (int)jObj["IdSeq"];
                idSeq++;
                jObj["IdSeq"] = idSeq;
                JsonFileHelper.WriteJsonFile(_idSeqFilePath, _idSeqFileName, jObj);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[IdSeqGenerator::GetIdSeq] Get IdSeq successfully. IdSeq: {0}", idSeq);
                }
                return idSeq;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[IdSeqGenerator::GetIdSeq] Get IdSeq error.");
                throw new DataAccessException("IdSeqGenerator::GetIdSeq: Get IdSeq error.", e);
            }
        }
    }
}
