namespace BooksApi.Dto
{
    public class Book
    {
        public string Id { get; set; } 
        public string Title { get; set; }
        public string Kind { get; set; }
        public string Genre { get; set; }
        public string Epoch { get; set; }
        // Cover instead of description, no description field in api response model
        public string Cover { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public Author Author { get; set; } = new();
    }

}
