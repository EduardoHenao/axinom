using DataManagementSystem.Repositories;

namespace AxinomCommon.Business
{
    /*
     * Extension class to inject the tratment date sub folder to the path of a DB file node
     */
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
