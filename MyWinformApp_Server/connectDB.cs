﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace MyWinformApp_Server
{
    class connectDB : IDataBase_Connection
    {
        string connString = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", "127.0.0.1", "chatlogs", "root", "qwe123!@#");
        public SqlConnection conn;
        
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
