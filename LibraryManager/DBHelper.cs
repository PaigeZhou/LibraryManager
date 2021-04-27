using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManager
{
        class DBHelper
        {
            /// <summary>
            /// 数据库连接字符串
            /// </summary>
            public static string constr = "Data Source=LAPTOP-PDKJ1K0Q;Initial Catalog=Library;Persist Security Info=True;User ID=sa;Password=cssl#123";

            /// <summary>
            /// 查询数据结果中的第一行第一列
            /// </summary>
            /// <param name="sql">需要执行的sql语句</param>
            /// <returns>数据结果集中的第一行第一列</returns>
            public static object ExecuteScalar(string sql)
            {
                //创建连接对象
                SqlConnection con = new SqlConnection(constr);
                //创建命令对象
                SqlCommand cmd = new SqlCommand(sql, con);

                try
                {
                    //可能出现异常的代码
                    con.Open();
                    return cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    //处理异常
                    Console.WriteLine("程序发生错误，错误信息是：" + ex.Message);
                }
                finally
                {
                    //不管上面的代码是否发生异常，都将执行这里的代码
                    con.Close();
                }
                return null;
            }

            /// <summary>
            /// 读取一行数据
            /// </summary>
            /// <param name="sql">需要执行的sql语句</param>
            /// <returns>读取器</returns>
            public static SqlDataReader ExecuteReader(string sql)
            {
                //创建连接对象
                SqlConnection con = new SqlConnection(constr);
                //创建命令对象
                SqlCommand cmd = new SqlCommand(sql, con);

                try
                {
                    //可能出现异常的代码
                    con.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    //处理异常
                    Console.WriteLine("程序发生错误，错误信息是：" + ex.Message);
                }
                //注意，这个方法不要写finally
                return null;
            }

            /// <summary>
            /// 实现数据增删改
            /// </summary>
            /// <param name="sql">需要执行的sql语句</param>
            /// <returns>true：执行成功  false:执行失败</returns>
            public static bool ExecuteNonQuery(string sql)
            {
                //创建连接对象
                SqlConnection con = new SqlConnection(constr);
                //创建命令对象
                SqlCommand cmd = new SqlCommand(sql, con);

                try
                {
                    //可能出现异常的代码
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    //处理异常
                    Console.WriteLine("程序发生错误，错误信息是：" + ex.Message);
                }
                finally
                {
                    //不管上面的代码是否发生异常，都将执行这里的代码
                    con.Close();
                }
                return false;
            }
        }
    }

