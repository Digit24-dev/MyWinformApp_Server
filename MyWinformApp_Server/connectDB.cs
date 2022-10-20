﻿using System;
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
        const string dbName = "chatlog";
        const string tableName = "chatlog";

        const string connectionString = "Server=localhost;Database=" + dbName + ";Uid=root;Pwd=qwe123!@#;";

        public SqlConnection conn;
        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;

        public connectDB()
        {
            
        }

        public void Open()
        {

            try
            {
                if(conn == null)
                {
                    //conn = new SqlConnection(connString);
                    //conn.Open();
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                Console.WriteLine(ex.Message);
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

         public bool IsOpen()
        {
            if (conn != null)   return true;
            return false;
        }

        public String GetDataSet(string sql)
        {
            cmd = new MySqlCommand(sql, connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            String line = "";

            while (dr.Read())
            {
                line = dr["clientName"].ToString();
            }
            dr.Close();
            return line;
        }

        public int SetData(string sql)
        {
            try
            {
                cmd = new MySqlCommand(sql, connection);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return -1;  // Sql 쿼리 오류
            }
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
