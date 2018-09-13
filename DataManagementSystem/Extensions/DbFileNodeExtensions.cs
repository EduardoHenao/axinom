using DataManagementSystem.Repositories;

namespace AxinomCommon.Business
{
    public static class DbFileNodeExtensions
    {
        // here we inject the respective date sub folder
        public static DbFileNode InjectTreatmentDateToRelativePath(this DbFileNode fileNode, string treatmentDate, string fileSeparator)
        {
            fileNode.RelativePath = $"{fileNode.RelativePath}{treatmentDate}{fileSeparator}"; 
            return fileNode;
        }
    }
}
