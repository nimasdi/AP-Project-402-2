using System;
using System.Data;
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Configuration;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace DBAccess
{
    public class DB
    {
        string jsonFile = "appsetting.json";
        private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Taha\Term 4\Ap\Project\AP-Project-402-2\RestaurantDB\RestaurantDB.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection con;
        public DB()
        {
            con = new SqlConnection(_connectionString);
        }

        public void Main(string[] args)
        {

            con.Open();
            Console.WriteLine("Run properly");
            con.Close();   
        }

        

    }
}
  