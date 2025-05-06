namespace BiblioTar.DTOs
{
    public class AddressCreateDto
    {
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Country { get; set; }
        
    }
    


    public class AddressGetDto
    {
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Country { get; set; }
    }
}
