using System.Text.Json;

namespace NotesApp.Data
{
    public class NotesService
    {
        private readonly List<Note> _notes = new();
        public List<Note> Notes
        {
            get { return _notes; }
        }

        public NotesService()
        {
            _notes = ReadNotesFromFile("notes.json");
        }

        public void AddNote(Note note)
        {
            _notes.Add(note);
            WriteNotesToFile();
        }

        public void UpdateStoredNotes() => WriteNotesToFile();

        /// <summary>
        /// Removes note from <c>notes.json</c> file and adds it to <c>deletedNotes.json</c>.
        /// </summary>
        /// <param name="note">Note that should be removed.</param>
        public void RemoveNote(Note note)
        {
            var deletedNotes = ReadNotesFromFile("deletedNotes.json");
            deletedNotes.Add(note);
            WriteNotesToFile(deletedNotes, "deletedNotes.json");

            _notes.Remove(note);
            WriteNotesToFile();
        }

        private void WriteNotesToFile()
        {
            string json = JsonSerializer.Serialize(_notes);
            try
            {
                File.WriteAllText(@"./jsonData/notes.json", json);
            }
            catch (DirectoryNotFoundException)
            {
                _ = Directory.CreateDirectory(@"./jsonData");
                File.WriteAllText(@"./jsonData/notes.json", json);
            }
        }

        private static void WriteNotesToFile(List<Note> notes, string filename)
        {
            string json = JsonSerializer.Serialize(notes);
            try
            {
                File.WriteAllText(@"./jsonData/" + filename, json);
            }
            catch (DirectoryNotFoundException)
            {
                _ = Directory.CreateDirectory(@"./jsonData");
                File.WriteAllText(@"./jsonData/" + filename, json);
            }
        }

        private static List<Note> ReadNotesFromFile(string fileName)
        {
            byte[]? fileBytes = null;
            try
            {
                fileBytes = File.ReadAllBytes(@"./jsonData/" + fileName);
            }
            // If dir/file does not exist, it will be created on note creation.
            catch (DirectoryNotFoundException) { }
            catch (FileNotFoundException) { }

            if (fileBytes == null || fileBytes.Length == 0)
            {
                return new();
            }

            return JsonSerializer.Deserialize<List<Note>>(fileBytes) ?? new();
        }

        public Task<List<Note>> GetNotesAsync()
        {
            return Task.FromResult(Notes);
        }

        public Note GetRandomNote(Note? excludedNote = null)
        {
            if (_notes.Count == 0)
            {
                return new Note() { Message = "No notes found" };
            }

            var excludedIndex = -1;
            if (excludedNote != null)
            {
                excludedIndex = _notes.FindIndex((note => note.Message == excludedNote.Message));
            }

            var randomIndex = Random.Shared.Next(_notes.Count);
            while (randomIndex == excludedIndex)
            {
                randomIndex = Random.Shared.Next(_notes.Count);
            }

            return _notes[randomIndex];
        }
    }
}
