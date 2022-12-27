using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace MyWinformApp_Server
{
    class DAO
    {
        public const string dbName = "chatlog";
        public const string tableName = "logs";
        //const string tableName = "chatlog"; 
        
        const string connectionString = "Server=localhost;Database=" + dbName + ";Uid=root;Pwd=qwe123!@#;";

        //public SqlConnection conn;
        private readonly MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;

        public DAO()
        {
            
        }

        public int DataParser(string time, string user, string message)
        {
            if (connection != null)
            {
                return SetData("insert into " + tableName + "values(" + "3" + user + message + time + ");"); // SQL 쿼리 오류
            }
            return -2; // connection loss;
        } 
        public string JsonParser(string time, string user, string message)
        {
            var serializedData = new Main.JSON_Data
            {
                Time = time,
                User = user,
                Message = message
            };

            string data = JsonSerializer.Serialize(serializedData);

            return data;
        }

        public void Open()
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void Close()
        {
            try
            {
                if(connection != null)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         public bool IsOpen()
        {
            if (connection != null)   return true;
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
        
    }
}
