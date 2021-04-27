using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManager
{
    class Manager
    {
        /// <summary>
        /// 管理员登录
        /// </summary>
        public void Dl()
        {
            Console.WriteLine("---------------欢迎使用长沙理工大学图书信息管理系统------------------");
            Console.WriteLine("              为保证数据的安全,系统将验证您的身份信息!");
            Console.Write("请输入用户名：");
            string name = Console.ReadLine();
            Console.Write("请输入你的密码：");
            string pwd = Console.ReadLine();
            //非空验证
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                Console.WriteLine("用户名和密码均不能为空值，请重新输入！！\n");
                Dl();
                return;
            }
            //数据验证
            //定义sql语句
            string sql = string.Format("SELECT count(0) FROM Users WHERE LoginID='{0}' AND LoginPwd='{1}'", name, pwd);
            //调用对应的方法
            int res = Convert.ToInt32(DBHelper.ExecuteScalar(sql));
            //打印/判断结果
            if (res >= 1)
            {
                Console.WriteLine("\n登录成功，欢迎你：{0}", name);
                Zcd();
            }
            else
            {
                Console.WriteLine("抱歉，用户名或密码无效，请重新登录！\n");
                Dl();
            }
        }

        /// <summary>
        /// 图书管理系统主菜单
        /// </summary>
        private void Zcd()
        {
            Console.WriteLine("---------------欢迎使用长沙理工大学图书信息管理系统------------------");
            Console.WriteLine("\t1. 查看全部图书信息");
            Console.WriteLine("\t2. 新增图书信息");
            Console.WriteLine("\t3. 修改图书售卖价格");
            Console.WriteLine("\t4. 删除图书信息");
            Console.WriteLine("\t5. 查看图书分类");
            Console.WriteLine("\t6. 新增图书分类");
            Console.WriteLine("\t0. 退出");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("请选择需要执行的菜单编号：");
            int res = Convert.ToInt32(Console.ReadLine());
            switch (res)
            {
                case 1:
                    Console.WriteLine("\n\t正在查看全部图书信息\n");
                    CKbyts();
                    break;
                case 2:
                    Console.WriteLine("\n\t正在新增图书信息\n");
                    Xz1();
                    break;
                case 3:
                    Console.WriteLine("\n\t正在修改图书售卖价格\n");
                    Xg();
                    break;
                case 4:
                    Console.WriteLine("\n\t正在删除图书信息\n");
                    Sc();
                    break;
                case 5:
                    Console.WriteLine("\n\t正在查看图书分类\n");
                    Ckbyfl();
                    break;
                case 6:
                    Console.WriteLine("\n\t正在新增图书分类\n");
                    Xz2();
                    break;
                case 0:
                    Console.WriteLine("\n\t谢谢您的使用，欢迎下次光临！\n已退出。。。\n");
                    return;
                default:
                    Console.WriteLine("\n\t输入其他无关菜单，默认退出。。。\n");
                    return;
            }
            Console.WriteLine("输入任意数返回主菜单：");
            string xz = Console.ReadLine();
            Zcd();
        }

        /// <summary>
        /// 查阅全部图书信息
        /// </summary>
        private void CKbyts()
        {
            string sql = string.Format("SELECT B.ID,B.Name,T.TypeName,B.Number,B.Price FROM Book B INNER JOIN BookType T ON B.TypeID=T.TypeID");
            SqlDataReader s = DBHelper.ExecuteReader(sql);
            Console.WriteLine("\n---------------------------------------------------------------------");
            Console.WriteLine("图书编号\t名称\t类别\t库存数量\t售卖价格");
            Console.WriteLine("\n---------------------------------------------------------------------");
            while (s != null && s.Read() && s.HasRows)
            {
                string bh = s["ID"].ToString();
                string mc = s["Name"].ToString();
                string lb = s["TypeName"].ToString();
                string sl = s["Number"].ToString();
                string jg = s["Price"].ToString();
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",bh,mc,lb,sl,jg);
            }
            Console.WriteLine("\n全部图书信息输出完毕！请您进行查阅。");
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 新增图书信息
        /// </summary>
        private void Xz1()
        {
            Console.Write("请输入您要添加的图书名称：");
            string ts = Console.ReadLine();
            Console.Write("请输入您要添加的图书数量：");
            int sl = Convert.ToInt32( Console.ReadLine());
            Console.Write("请输入您要添加的图书售卖价格：");
            double jg =Convert.ToDouble( Console.ReadLine());
            string sql = string.Format("SELECT * FROM Book WHERE NAME='{0}'",ts);
            SqlDataReader s= DBHelper.ExecuteReader(sql);

            if (s!=null && !s.HasRows)
            {
                string sql1 = string.Format("SELECT TypeID,TypeName FROM BookType");
                SqlDataReader c = DBHelper.ExecuteReader(sql1);
                Console.WriteLine("请选择图书分类编号：");
                Console.WriteLine("---------------------------");
                Console.WriteLine("分类编号\t分类名称");
                Console.WriteLine("------------------------");
                while (c != null && c.Read() && c.HasRows)
                {
                    string bh = c["TypeID"].ToString();
                    string fl = c["TypeName"].ToString();
                    Console.WriteLine("{0}\t{1}", bh, fl);
                }
                Console.Write("请输入编号：");
                int bh1 = Convert.ToInt32(Console.ReadLine());
                string sql2 = string.Format("INSERT INTO Book (Name,TypeID,Number,Price) VALUES ('{0}',{1},{2},{3})", ts, bh1, sl, jg);
                bool pd = DBHelper.ExecuteNonQuery(sql2);
                if (pd)
                {
                    Console.WriteLine("\n新增成功！");
                }
                else
                {
                    Console.WriteLine("\n新增失败！请稍后再试");
                }
            }
            else
            {
                Console.WriteLine("您要新增的图书已存在,请稍后再试!");
                Xz1();
            }
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 修改图书售卖价格
        /// </summary>
        private void Xg()
        {
            Console.Write("请输入您将要修改价格的图书名称：");
            string name = Console.ReadLine();

            string sql = string.Format("SELECT * FROM Book WHERE Name='{0}'", name);
            SqlDataReader s = DBHelper.ExecuteReader(sql);
            if (s != null && !s.HasRows)
            {
                Console.WriteLine("抱歉，您要找的图书不存在！请稍后再试！");
                Xg();
            }
            else
            {
                Console.WriteLine("请输入您要修改的价格：");
                double jgg = Convert.ToDouble(Console.ReadLine());
                string sql1 = string.Format("SELECT ID,Name,Price FROM Book WHERE Name='{0}'", name);
                SqlDataReader s1 = DBHelper.ExecuteReader(sql1);
                Console.WriteLine("---------------------------");
                Console.WriteLine("你要修改价格的图书信息为：");
                Console.WriteLine("图书编号\t图书名称\t售卖价格");
                while(s!=null && s.Read() && s.HasRows)
                {
                    String bh = s["ID"].ToString();
                    String mc = s["Name"].ToString();
                    String jg = s["Price"].ToString();
                    Console.WriteLine("{0}\t{1}\t{2}",bh,mc,jg);
                }
                Console.WriteLine("---------------------------");
                Console.Write("按y继续执行：");
                string pd = Console.ReadLine();
                if (pd.ToLower() == "y")
                {
                    string sql2 = string.Format("UPDATE Book SET Price = {0} WHERE Name='{1}'", jgg, name);
                    bool pd2 = DBHelper.ExecuteNonQuery(sql2);
                    if (pd2)
                    {
                        Console.WriteLine("==============================================================");
                        Console.WriteLine("修改成功！");
                        Console.WriteLine("==============================================================");
                    }
                    else
                    {
                        Console.WriteLine("==============================================================");
                        Console.WriteLine("修改失败");
                        Console.WriteLine("==============================================================");
                    }
                }
                else
                {
                    Console.WriteLine("玩呢！待会再试！！！");
                }
            }
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 删除图书信息
        /// </summary>
        private void Sc()
        {
            Console.Write("请输入您将要删除的图书名称：");
            string name = Console.ReadLine();

            string sql = string.Format("SELECT * FROM Book WHERE Name='{0}'", name);
            SqlDataReader s = DBHelper.ExecuteReader(sql);
            if (s != null && !s.HasRows)
            {
                Console.WriteLine("抱歉，您要删除的图书已不存在！请稍后再试！");
                Sc();
            }
            else
            {
                string sql1 = string.Format("SELECT ID,Name,Price FROM Book WHERE Name='{0}'", name);
                SqlDataReader s1 = DBHelper.ExecuteReader(sql1);
                Console.WriteLine("---------------------------");
                Console.WriteLine("你要删除的图书信息为：");
                Console.WriteLine("图书编号\t图书名称\t售卖价格");
                while (s != null && s.Read() && s.HasRows)
                {
                    String bh = s["ID"].ToString();
                    String mc = s["Name"].ToString();
                    String jg = s["Price"].ToString();
                    Console.WriteLine("{0}\t{1}\t{2}", bh, mc, jg);
                }
                Console.WriteLine("---------------------------");
                String yzm = yam();
                Console.Write("输入验证码（{0}）以继续执行：",yzm);
                string pd = Console.ReadLine();
                if (pd.Equals(yzm))
                {
                    string sql2 = string.Format("DELETE FROM Book WHERE Name='{0}'",  name);
                    bool pd2 = DBHelper.ExecuteNonQuery(sql2);
                    if (pd2)
                    {
                        Console.WriteLine("==============================================================");
                        Console.WriteLine("删除成功！");
                        Console.WriteLine("==============================================================");
                    }
                    else
                    {
                        Console.WriteLine("==============================================================");
                        Console.WriteLine("删除失败");
                        Console.WriteLine("==============================================================");
                    }
                }
                else
                {
                    Console.WriteLine("玩呢！待会再试！！！");
                }
            }
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 生成4个随机数
        /// </summary>
        /// <returns>4位随机数</returns>
        private string yam()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        /// <summary>
        /// 查看图书分类
        /// </summary>
        private void Ckbyfl()
        {
            string sql = string.Format("SELECT TypeID,TypeName FROM BookType");
            SqlDataReader s = DBHelper.ExecuteReader(sql);
            Console.WriteLine("\n---------------------------------------------------------------------");
            Console.WriteLine("图书编号\t图书名字");
            Console.WriteLine("\n---------------------------------------------------------------------");
            while (s != null && s.Read() && s.HasRows)
            {
                string bh = s["TypeID"].ToString();
                string mc = s["TypeName"].ToString();
                Console.WriteLine("{0}\t{1}", bh, mc);
            }
            Console.WriteLine("\n全部图书类别信息输出完毕！请您进行查阅。");
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 新增图书分类
        /// </summary>
        private void Xz2()
        {
            Console.Write("请输入您要添加的图书分类：");
            string fl = Console.ReadLine();

            string sql = string.Format("SELECT * FROM BookType WHERE TypeName='{0}'", fl);
            SqlDataReader s = DBHelper.ExecuteReader(sql);

            if (s != null && !s.HasRows)
            {
                string sql2 = string.Format("INSERT INTO BookType (TypeName) VALUES ('{0}')",fl);
                bool pd = DBHelper.ExecuteNonQuery(sql2);
                if (pd)
                {
                    Console.WriteLine("\n新增成功！");
                }
                else
                {
                    Console.WriteLine("\n新增失败！请稍后再试");
                }
            }
            else
            {
                Console.WriteLine("您要新增的图书类别已存在,请稍后再试!");
                Xz2();
            }
            if (s != null)
            {
                s.Close();
            }
        }
    }

}
