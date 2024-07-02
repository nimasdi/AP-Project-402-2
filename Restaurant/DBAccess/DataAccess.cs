using System;
using System.Data;
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Configuration;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dapper;
using System.Net;



namespace DBAccess
{
    public class DataAccess
    {
        string jsonFile = "appsetting.json";
        private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=E:\TAHA\TERM 4\AP\PROJECT\AP-PROJECT-402-2\RESTAURANTDB\RESTAURANTDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        
        //SqlConnection con;

        //Loading data from DB
        public List<T> LoadData<T, U>(string sqlStatement, U parameters)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }
        }

        public int SaveData<T>(string sqlStatement, T parameters, bool returnId = false)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (returnId) 
                {
                    return connection.QuerySingle<int>(sqlStatement, parameters);
                }
                else
                {
                    connection.Execute(sqlStatement, parameters);
                    return 0;
                }
            }
        }



    }
}
  