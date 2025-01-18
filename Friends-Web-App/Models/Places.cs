namespace Friends_Web_App.Models
{
    public enum Negeri
    {
        Selangor,
        Kelantan,
        Perak,
        Perlis,
        Johor,
        NegeriSembilan,
        Melaka,
        Sabah,
        Sarawak,
        KualaLumpur,
        Kedah,
        PulauPinang,
        Terengganu
    }
    public class Places
    {
        public int PlacesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Negeri Negeri { get; set; }
    }

}
