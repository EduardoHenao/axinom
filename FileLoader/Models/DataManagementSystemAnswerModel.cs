namespace FileLoader.Models
{
    /*
     * model for the post answer (see UploadFilesController)
     */
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
