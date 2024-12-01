namespace note_backend.Models
{
    public class JWTOption
    {

        public string SigningKey { get; set; }
        public string ExpireSeconds { get; set; }
    }
}
