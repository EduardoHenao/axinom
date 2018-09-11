using Microsoft.AspNetCore.Http;

namespace WebApplication1.Models
{
    public class UploadFileModel
    {
       public IFormFile File { get; set; }
    }
}
