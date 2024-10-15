namespace BuilderCatalogWebApi.Models
{
    public class LEGOPart
    {
        public string PieceId { get; set; }
        public List<LEGODetails> Variants { get; set; }
    }

    public class LEGODetails {
        public int Color { get; set; }
        public int Count { get; set; }
    }

}
