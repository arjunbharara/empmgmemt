namespace EmplyeMgm.Models
{
    public class Employee  
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Emial { get; set; }
        public DateOnly DOB { get; set; }
        public string City { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}
