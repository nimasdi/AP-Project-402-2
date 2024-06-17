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
        private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Taha\Term 4\Ap\Project\AP-Project-402-2\RestaurantDB\RestaurantDB.mdf;Integrated Security=True;Connect Timeout=30";


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
  