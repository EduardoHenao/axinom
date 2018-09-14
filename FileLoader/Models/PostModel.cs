namespace ControlPanel.Models
{
    public class PostModel
    {
        public string JsonString { get; set; }
        public string AnswerComment { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
