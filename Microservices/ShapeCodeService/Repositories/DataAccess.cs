using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualBasic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShapeCodeService.Repositories
{
    public class DataAccess
    {
        public static Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("con");

        public static string strpath = "";
        public static DataSet GetDataSet(System.Data.Common.DbCommand dbcom)
        {
            DataSet ds = new DataSet();
            dbcom.CommandTimeout = 0;
            try
            {
                ds = db.ExecuteDataSet(dbcom);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static string GetScalar(System.Data.Common.DbCommand dbcom)
        {
            string result = "";
            dbcom.CommandTimeout = 0;
            try
            {
                result = (string)db.ExecuteScalar(dbcom);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.Trim();
        }

        public static int ExecuteNonQuery(System.Data.Common.DbCommand dbcom)
        {
            int result = 0;
            dbcom.CommandTimeout = 0;
            try
            {
                result = db.ExecuteNonQuery(dbcom);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static IDataReader ExecuteReader(System.Data.Common.DbCommand dbcom)
        {
            IDataReader result = null/* TODO Change to default(_) if this is not a reference type */;
            dbcom.CommandTimeout = 0;
            try
            {
                result = db.ExecuteReader(dbcom);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        internal static SqlDataReader ExecuteReader(byte[]? image)
        {
            throw new NotImplementedException();
        }
    }
}
