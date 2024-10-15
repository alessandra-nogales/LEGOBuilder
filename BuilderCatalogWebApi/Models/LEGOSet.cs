namespace BuilderCatalogWebApi.Models
{
    public class LEGOCollection { 
        public List<LEGOSet> Sets { get; set; }
    }

    public class LEGOSet
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string SetNumber { get; set; }
        public List<SetPart> Pieces { get; set; } = new List<SetPart>();
        public int TotalPieces { get; set; }
    }


    public class SetPart
    {
        public Part Part { get; set; }
        public int Quantity { get; set; }
    }

    public class Part
    {
        public string DesignID { get; set; }
        public int Material { get; set; }
        public string PartType { get; set; }
    }
}
