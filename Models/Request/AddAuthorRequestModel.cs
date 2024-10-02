namespace CodePulse.API.Models.Request
{
    public class AddAuthorRequestModel
    {
        public required string AuthorName { get; set; }

        public required string AuthorEmail { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string? Description { get; set; }
    }
}
