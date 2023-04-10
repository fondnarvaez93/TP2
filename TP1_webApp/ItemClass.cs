namespace TP1_webApp
{
    public class ItemClass
    {
        // Attributes
        public string ID;
        public string Name;
        public string Price;

        // Init
        public ItemClass()
        {
            ID = "";
            Name = "";
            Price = "";
        }
        public ItemClass(String newID, String newName, String newPrice)
        {
            ID = newID;
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
