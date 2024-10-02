namespace CodePulse.API.Models.DataBase
{
    public class AuthorModel
    {
        public Guid ID { get; set; }

        public required string AuthorName { get; set; }

        public required string AuthorEmail { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string? Description { get; set; }
    }
}
