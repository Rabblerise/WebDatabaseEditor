namespace WebDatabaseEditor.Models
{
    public class CustomTable
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public List<string> Columns { get; set; } = new List<string>();
        public string SelectedRole { get; set; } = string.Empty;
    }
}
