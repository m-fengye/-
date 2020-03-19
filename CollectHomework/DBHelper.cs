using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CollectHomework
{
    public class DBHelper
    {
        static string connStr;
        //构造函数 数据库连接字符串                
        static DBHelper()
        {
            connStr = ConfigurationManager.ConnectionStrings["WorkDB"].ConnectionString.ToString();
        }
        /// <summary>
        /// 初始化sqlcommand
        /// </summary>
        /// <param name="comm">sqlcommand</param>
        /// <param name="conn">连接connection</param>
        /// <param name="sql">命令字符串</param>
        /// <param name="comType">解释命令字符串</param>
        /// <param name="comParms">数据库参数列表</param>
        static void PrepareCommand(SqlCommand comm, SqlConnection conn, string sql, CommandType comType, SqlParameter[] comParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            comm.Connection = conn;
            comm.CommandText = sql;
            comm.CommandType = comType;
            if (comParms != null)
            {
                foreach (SqlParameter p in comParms)
                {
                    comm.Parameters.Add(p);
                }
            }
        }

        /// <summary>
        /// 读取数据库到DataSet
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="comType">解释命令字符串</param>
        /// <param name="comPrams">数据库参数列表</param>
        /// <returns>DataSet</returns>
        public static DataSet SelectToDS(string sql, CommandType comType, params SqlParameter[] comPrams)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    PrepareCommand(comm, conn, sql, comType, comPrams);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            adapter.Fill(ds);
                            comm.Parameters.Clear();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            return null;
                            //throw ex;
                        }
                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// 执行数据库操作:增删改
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="comType">解释命令字符串</param>
        /// <param name="comPrams">数据库参数列表</param>
        /// <returns>返回受影响行数</returns>
        public static int ExecuteSql(string sql, CommandType comType, params SqlParameter[] comPrams)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(comm, conn, sql, comType, comPrams);
                        int rows = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                        conn.Close();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        return 0;
                        //throw ex;
                    }
                }
            }
        }
    }
}
