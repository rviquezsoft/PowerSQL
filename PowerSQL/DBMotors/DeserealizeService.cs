using Newtonsoft.Json;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors
{
    public class DeserealizeService<T> : IDeserealizeService<T>
    {
        private readonly IPrintExceptions exceptions;

        public DeserealizeService(IPrintExceptions _exceptions)
        {
            exceptions = _exceptions;
        }
        public Task<T> deserealize(string json)
        {
            T converted = default(T);
            try
            {
                converted =string.IsNullOrWhiteSpace(json)? converted: JsonConvert.DeserializeObject<T>(json,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }

            return Task.FromResult(converted);
        }
    }
}
