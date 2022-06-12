using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetPropertiesService<T> : IGetPropertiesService
    {
        /// <summary>
        /// Get props from a T
        /// </summary>
        /// <returns></returns>
        public PropertyInfo[] getProps()
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            return props;
        }
    }
}
