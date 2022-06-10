using Newtonsoft.Json;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors
{
    public class SerializeService : ISerializeService
    {
        private readonly IPrintExceptions exceptions;

        public SerializeService(IPrintExceptions _exceptions)
        {
           exceptions = _exceptions;    
        }
        public Task<string> serialize(object obj)
        {
            string json = "";
            try
            {
                json =obj==null?json:JsonConvert.SerializeObject(obj,
                               Formatting.Indented,
                               new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return Task.FromResult(json);
        }
    }
}
