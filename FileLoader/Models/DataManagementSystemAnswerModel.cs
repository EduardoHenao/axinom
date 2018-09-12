namespace FileLoader.Models
{
    public class DataManagementSystemAnswerModel
    {
        public bool Ok { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $"DataManagementSystemAnswerModel answer: {Ok}";
        }
    }
}
