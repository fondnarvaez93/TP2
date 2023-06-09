﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;



namespace TP1_webApp.Models
{

    public class SQLConnection
    {
        // Attribute
        public List<ItemClass> ItemsList = new List<ItemClass>();
        public List<String> ClassesList = new List<String>();
        public int ItemsListCount;
        // ... DB credentials
        public String DBCredentials;
        public SqlConnection Connection;
        public String Class { get; set; }
        public String Name { get; set; }
        public int Price { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String myIP { get; set; }
        public String LogIn_msg { get; set; }
        public String Add_msg { get; set; }
        public bool LogIn_Result { get; set; }
        public String NameFilter_txt { get; set; }
        public int CountFilter_txt { get; set; }
        public String ClassFilter_txt { get; set; }
        public List<SelectListItem> ListClass { get; set; }



        // Init
        // ... this inicialize the table
        public SQLConnection()
        {
            //public String DBCredentials = "Data Source=ec2-54-160-71-139.compute-1.amazonaws.com;Initial Catalog=TareaConcepto;Persist Security Info=True;User ID=sa;Password=Guachin321?";
            DBCredentials = "Data Source=ec2-3-16-154-37.us-east-2.compute.amazonaws.com;Initial Catalog=TP1;Persist Security Info=True;User ID=sa;Password=Admin1234";
            Connection = new SqlConnection(DBCredentials);
            Class = "";
            Name = "";
            Price = 0;
            ItemsListCount = 0;
            ItemClass articuloInfo = new ItemClass();
            ItemsList.Add(articuloInfo);

            UserName = "";
            Password = "";
            myIP = "";
            LogIn_msg = "";
            Add_msg = "";
            LogIn_Result = false;
            NameFilter_txt = "";
            CountFilter_txt = 0;
            ClassFilter_txt = "";
            ListClass = new List<SelectListItem>() { new SelectListItem { Text = "Ferreteria", Value = "Ferreteria" } };
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
                    String articuloID = "" + Reader["id"].ToString();
                    String articuloIDClase = "" + Reader["NombreClase"].ToString();
                    String articuloName = "" + Reader["Nombre"].ToString();
                    String articuloPrice = "" + Reader["Precio"].ToString();

                    ItemClass articuloInfo = new ItemClass(articuloID, articuloIDClase, articuloName, articuloPrice);
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



        // ... Get info from DB
        public void GetClasses()
        {
            SqlConnection Connection = new SqlConnection(DBCredentials);
            try
            {
                // ... open connection, send request and read responce
                Connection.Open();

                // ... using the stored procedure
                SqlCommand SelectCommand = new SqlCommand("GetClasses", Connection);
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlDataReader Reader = SelectCommand.ExecuteReader();

                // ... collect the items from the DB
                while (Reader.Read())
                {
                    String articuloNombre = "" + Reader["Nombre"].ToString();
                    ClassesList.Add(articuloNombre);
                }

                // ... close connection
                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }



        // ... Add item to DB
        public void Add(String valClass, String valName, int valPrice, String user, String ip)
        {
            try
            {

                SqlConnection Connection = new SqlConnection(DBCredentials);

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("AddNewItem", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@ArticuloClass", valClass);
                InsertCommand.Parameters.AddWithValue("@newName", valName);
                InsertCommand.Parameters.AddWithValue("@newPrice", valPrice);
                InsertCommand.Parameters.AddWithValue("@User", user);
                InsertCommand.Parameters.AddWithValue("@IP", ip);

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
                        Add_msg += "Articulo con nombre duplicado";
                    }
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
        public void LogIn(String UserN, String Pass, String user, String ip)
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
                InsertCommand.Parameters.AddWithValue("@User", user);
                InsertCommand.Parameters.AddWithValue("@IP", ip);


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


        // ... Filter Name info from DB
        public void FilterName(String str, String user, String ip)
        {
            try
            {
                // ... open connection, send request and read responce
                Connection.Open();

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("FilterByName", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@Item", str);
                InsertCommand.Parameters.AddWithValue("@User", user);
                InsertCommand.Parameters.AddWithValue("@IP", ip);
                SqlDataReader Reader = InsertCommand.ExecuteReader();

                // ... collect the items from the DB
                while (Reader.Read())
                {
                    String articuloID = "" + Reader["id"].ToString();
                    String articuloIDClase = "" + Reader["NombreClase"].ToString();
                    String articuloName = "" + Reader["Nombre"].ToString();
                    String articuloPrice = "" + Reader["Precio"].ToString();

                    ItemClass articuloInfo = new ItemClass(articuloID, articuloIDClase, articuloName, articuloPrice);
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

        // ... Filter Count info from DB
        public void FilterCount(int count, String user, String ip)
        {
            try
            {
                // ... open connection, send request and read responce
                Connection.Open();

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("FilterByCount", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@Count", count);
                InsertCommand.Parameters.AddWithValue("@User", user);
                InsertCommand.Parameters.AddWithValue("@IP", ip);
                SqlDataReader Reader = InsertCommand.ExecuteReader();

                // ... collect the items from the DB
                while (Reader.Read())
                {
                    String articuloID = "" + Reader["id"].ToString();
                    String articuloIDClase = "" + Reader["NombreClase"].ToString();
                    String articuloName = "" + Reader["Nombre"].ToString();
                    String articuloPrice = "" + Reader["Precio"].ToString();

                    ItemClass articuloInfo = new ItemClass(articuloID, articuloIDClase, articuloName, articuloPrice);
                    ItemsList.Add(articuloInfo);
                    ItemsListCount++;
                }

                // ... close connection
                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        // ... Filter Count info from DB
        public void FilterClass(String classID, String user, String ip)
        {
            try
            {
                // ... open connection, send request and read responce
                Connection.Open();

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("FilterByClass", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@ClassName", classID);
                InsertCommand.Parameters.AddWithValue("@User", user);
                InsertCommand.Parameters.AddWithValue("@IP", ip);
                SqlDataReader Reader = InsertCommand.ExecuteReader();

                // ... collect the items from the DB
                while (Reader.Read())
                {
                    String articuloID = "" + Reader["id"].ToString();
                    String articuloIDClase = "" + Reader["NombreClase"].ToString();
                    String articuloName = "" + Reader["Nombre"].ToString();
                    String articuloPrice = "" + Reader["Precio"].ToString();

                    ItemClass articuloInfo = new ItemClass(articuloID, articuloIDClase, articuloName, articuloPrice);
                    ItemsList.Add(articuloInfo);
                    ItemsListCount++;
                }

                // ... close connection
                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }


        // ... Filter Count info from DB
        public void LogOut(String user, String ip)
        {
            try
            {
                // ... open connection, send request and read responce
                SqlConnection Connection = new SqlConnection(DBCredentials);

                // ... using the stored procedure
                SqlCommand InsertCommand = new SqlCommand("SignOut", Connection);
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Parameters.AddWithValue("@User", user);
                InsertCommand.Parameters.AddWithValue("@IP", ip);

                // ... collect the items from the DB
                try
                {
                    Connection.Open();
                    InsertCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }

                // ... close connection
                Connection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }




    }
}
