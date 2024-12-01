namespace note_backend.Models
{
    public class UserDTO

    {
        public User user { get; set; }
        public string? Captchakey { get; set; }
        public string? PassWord {  get; set; }
    }
}
