namespace CodePulse.API.Models.Response
{
    public class AddAuthorResponseModel
    {
        public Guid ID { get; set; }
        public required string AuthorName { get; set; }

        public required string AuthorEmail { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string? Description { get; set; }
    }
}
