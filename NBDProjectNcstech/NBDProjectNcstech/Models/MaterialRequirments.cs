namespace NBDProjectNcstech.Models
{
    public class MaterialRequirments
    {
        public int ID { get; set; }

        public Materials Materials { get; set; }

        public Pottery Pottery { get; set; }

        public Plants Plants { get; set; }
    }
}
