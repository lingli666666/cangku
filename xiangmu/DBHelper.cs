using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //引用命名空间
using System.Data.SqlClient;//引用命名空间

namespace WindowsFormsApplication1
{
   public  class DBHelper
    {
        private static string DBConnectString = "Data Source=.;Initial Catalog=Students;Integrated Security=True"; //数据库连接字符串
        private static SqlConnection conn;
        private static SqlDataAdapter da;
        private static SqlCommand cmd;
        private static DBHelper dBHelper;

        public DBHelper()
        {
            conn = new SqlConnection(DBConnectString);
        }

        public static DBHelper Instance()
        {
            if (dBHelper == null)
            {
                dBHelper = new DBHelper();
            }
            return dBHelper;
        }
       /// <summary>
       /// 打开数据库连接
       /// </summary>
        void DBOpen()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
       /// <summary>
       /// 关闭数据库练级
       /// </summary>
        void DBClose()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
       /// <summary>
       /// 适合查询select语句，返回表结果集
       /// </summary>
        /// <param name="sql">select语句</param>
       /// <returns>表结果集</returns>

        public DataTable GetDataTableBySql(string sql)
        {
            DBOpen();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(sql, conn);
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                DBClose();
            }
        }
       /// <summary>
       /// 适合增删改
       /// </summary>
       /// <param name="sql">增删改语句</param>
       /// <returns>返回是否影响行数，真为有影响行数，假为没有影响行数</returns>
        public bool ExcuteSql(string sql)
        {
            DBOpen();
            cmd = new SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                DBClose();
            }
        }
       /// <summary>
       /// 执行存储过程
       /// </summary>
       /// <param name="proName">存储过程名</param>
       /// <param name="paras">传入参数</param>
        /// <returns>返回是否影响行数，真为有影响行数，假为没有影响行数</returns>
        public bool ExcuteProcedure(string proName, SqlParameter[] paras)
        {
            DBOpen();
            cmd = new SqlCommand(proName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (paras != null)
            {


                for (int i = 0; i < paras.Length; i++)
                {
                    cmd.Parameters.Add(paras[i]);
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                DBClose();
            }

        }
       /// <summary>
       /// 执行存储过程
       /// </summary>
       /// <param name="proName">存储过程名</param>
       /// <param name="paras">传入参数</param>
       /// <returns>表结果集</returns>
        public DataTable ExcuteProcedureReturnTable(string proName, SqlParameter[] paras)
        {
            DBOpen();
            DataTable dt = new DataTable();
            cmd = new SqlCommand(proName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            if (paras != null)
            {


                for (int i = 0; i < paras.Length; i++)
                {
                    cmd.Parameters.Add(paras[i]);
                }
            }
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                DBClose();
            }

        }
    }
}
