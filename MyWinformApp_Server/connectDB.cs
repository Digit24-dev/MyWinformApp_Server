using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWinformApp_Server
{
    class connectDB
    {
        string connString;
        public SqlConnection conn;

        public connectDB()
        {
            connString = "Server=localhost;Database=chatlog;Uid=root;Pwd=qwe123!@#;";
        }

        public void Open()
        {

            try
            {
                if(conn == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                //Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void Close()
        {
            try
            {
                if(conn != null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataSet(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// DB 트랜잭션 사용하기
        /// Postgresql DB 연동 찾아보기
        /// - Nuget 패키지 관리 > 찾아보기 탭 MySql.Data 설치
        /// 출처 : https://dodo1054.tistory.com/114
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int fn_ExecuteNonQuery(string[] strSql) 
        {
            
            return 0;
        }

    }
}
