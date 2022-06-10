
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors
{
    public class DBMotorBase<T>
    {
        protected string connection;
        public DBMotorBase(string _connection)
        {
            connection= _connection;
           
        }

       
    }
}
