using System;
using System.Data;
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Configuration;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dapper;



namespace DBAccess
{
    public class DataAccess
    {
        string jsonFile = "appsetting.json";
        private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Taha\Term 4\Ap\Project\AP-Project-402-2\RestaurantDB\RestaurantDB.mdf;Integrated Security=True;Connect Timeout=30";
        
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

        //Inserting data to DB
        public void SaveData<T>(string sqlStatement, T parameters)
        {
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sqlStatement, parameters); 
            }
        } 

        

    }
}
  