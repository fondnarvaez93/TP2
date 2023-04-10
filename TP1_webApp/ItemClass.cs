namespace TP1_webApp
{
    public class ItemClass
    {
        // Attributes
        public string ID;
        public string IDClase;
        public string Name;
        public string Price;

        // Init
        public ItemClass()
        {
            ID = "";
            IDClase = "";
            Name = "";
            Price = "";
        }
        public ItemClass(String newID, String newIDClase, String newName, String newPrice)
        {
            ID = newID;
            IDClase = newIDClase;
            Name = newName;
            Price = newPrice;
        }

        // Methods

        // ... to get a char from ASCII int code
        public void setID(String newID)
        {
            ID = newID;
        }
    }
}
