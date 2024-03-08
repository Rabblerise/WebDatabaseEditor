namespace WebDatabaseEditor.Models
{
    public class DynamicTable
    {
        public string TableName { get; set; }
        public List<string> Headers { get; set; }
        public List<Dictionary<string, string>> Data { get; set; }
    }
}
