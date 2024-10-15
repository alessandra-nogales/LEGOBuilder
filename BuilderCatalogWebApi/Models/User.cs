namespace BuilderCatalogWebApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public int BrickCount { get; set; }
        public List<LEGOPart> Collection { get; set; }
    }
}
