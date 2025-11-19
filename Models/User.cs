namespace RestServiceFinal.Models
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Lname { get; set; }
        public string? Email { get; set; }

        public string? Gender { get; set; }

        public string? Company { get; set; }
    }
}
