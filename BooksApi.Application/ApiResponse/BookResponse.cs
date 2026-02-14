namespace BooksApi.Entities
{
    public class BookResponse
    {
        public string slug { get; set; }
        public string title { get; set; }
        public string href { get; set; }
        public string simple_thumb { get; set; }
        public string kind { get; set; }
        public string genre { get; set; }
        public string epoch { get; set; }
        //returns cover instead of description, field description does not exist in api response model
        public string cover { get; set; }
        public string author { get; set; }
    }
}
