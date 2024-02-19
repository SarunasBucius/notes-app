namespace NotesApp.Data
{
    public class Note
    {
        public string? Message { get; set; }

        public string? Source { get; set; }

        public string? Tags { get; set; }

        public Note SimpleClone()
        {
            return new Note
            {
                Message = Message,
                Source = Source,
                Tags = Tags
            };
        }
    }
}
