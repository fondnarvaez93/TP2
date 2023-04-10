using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TP1_webApp.Views.Home
{
    public class CreateModel : PageModel
    {
        public ItemClass newItem = new ItemClass();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            newItem.Name = Request.Form["name"];
            newItem.Price = Request.Form["price"];

            if (newItem.Name.Length == 0 || newItem.Price.Length == 0)
            {
                errorMessage = "Rellenar todos los espacios.";
                return;
            }

            //... Saves the new item then empties the field text
            try
            {
                String DBCredentials = "Data Source=ec2-54-160-71-139.compute-1.amazonaws.com;Initial Catalog=TareaConcepto;" +
                    "Persist Security Info=True;User ID=sa;Password=Guachin321?";
                using (SqlConnection connection = new SqlConnection(DBCredentials))
                {
                    connection.Open();
                    String sql = "INSERT INTO dbo.Articulo" +
                                 "(Nombre, Precio) VALUES" + 
                                 "(@Nombre, @Precio);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", newItem.Name);
                        command.Parameters.AddWithValue("@Precio", newItem.Price);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            newItem.Name = "";
            newItem.Price = "";

            successMessage = "Artículo agregado con éxito.";
        }
    }
}
