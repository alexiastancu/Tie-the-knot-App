using SQLite;

namespace Wedding_Planning_App.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int LocationID { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string FormattedAddress { get; set; }
        public string PlaceID { get; set; }
    }
}
