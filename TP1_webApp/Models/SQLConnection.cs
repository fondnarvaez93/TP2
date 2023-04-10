﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;



namespace TP1_webApp.Models
{

    public class SQLConnection
    {
        // Attribute
        public List<ItemClass> ItemsList = new List<ItemClass>();
        public int ItemsListCount;
        // ... DB credentials
        public String DBCredentials;
        public SqlConnection Connection;
        public String Name { get; set; }
        public int Price { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String LogIn_msg { get; set; }
        public bool LogIn_Result { get; set; }



        // Init
        // ... this inicialize the table
        public SQLConnection()
        {
            //public String DBCredentials = "Data Source=ec2-54-160-71-139.compute-1.amazonaws.com;Initial Catalog=TareaConcepto;Persist Security Info=True;User ID=sa;Password=Guachin321?";
            DBCredentials = "Data Source=ec2-18-217-119-78.us-east-2.compute.amazonaws.com;Initial Catalog=TP1;Persist Security Info=True;User ID=sa;Password=Admin1234";
            Connection = new SqlConnection(DBCredentials);
            Name = "";
            Price = 0;
            ItemsListCount = 0;
            ItemClass articuloInfo = new ItemClass();
            ItemsList.Add(articuloInfo);

            UserName = "";
            Password = "";
            LogIn_msg = "";
            LogIn_Result = false;
        }


        // Methods

        // ... Get info from DB
        public void Get()
        {
            try
            {
                // ... open connection, send request and read responce
                Connection.Open();

                // ... using the stored procedure
                SqlCommand SelectCommand = new SqlCommand("GetItems", Connection);
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlDataReader Reader = SelectCommand.ExecuteReader();
                
                // ... collect the items from the DB
                while (Reader.Read())
                {
                    String articuloID = "" + Reader["Id"].ToString();
                    String articuloName = "" + Reader["Nombre"].ToString();
                    String articuloPrice = "" + Reader["Precio"].ToString();

                    ItemClass articuloInfo = new ItemClass(articuloID, articuloName, articuloPrice);
                    ItemsList.Add(articuloInfo);
                    ItemsListCount++;
                }
                
                // ... close connection
                var sortedList = ItemsList.OrderBy(p => p.Name).ToList();
                ItemsList = sortedList;
                Reader.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }



        // ... Add item to DB
        public void Add(String valName, int valPrice)
        {
            try
            {

                SqlConnection Connection = new SqlConnection(DBCredentials);

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("AddNewItem", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@newName", valName);
                InsertCommand.Parameters.AddWithValue("@newPrice", valPrice);

                // ... open connection and send new item
                try
                {
                    Connection.Open();
                    InsertCommand.ExecuteNonQuery();
                    Connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }


        // ... Add item to DB
        public void LogIn(String UserN, String Pass)
        {
            try
            {
                // ... open connection, send request and read responce
                SqlConnection Connection = new SqlConnection(DBCredentials);

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("LogIn_Procedure", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@UserName", UserN);
                InsertCommand.Parameters.AddWithValue("@Password", Pass);
                //SqlDataReader Reader = InsertCommand.ExecuteReader();

                
                // @ReturnVal could be any name
                var returnParameter = InsertCommand.Parameters.Add("@ReturnVal", SqlDbType.VarChar);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                
                // ... open connection and send new item
                try
                {
                    Connection.Open();
                    InsertCommand.ExecuteNonQuery();
                    // ... collect the items from the DB
                    var result = returnParameter.Value.ToString();
                    if (result == "1")
                    {
                        LogIn_msg = "";
                        LogIn_Result = true;
                    }
                    else
                    {
                        LogIn_msg += "Combinación de usuario/password no existe en la BD";
                        LogIn_Result = false;
                    }

                    // ... close connection
                    Connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
}
