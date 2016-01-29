using Lucy.Client.Desktop.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Client.Desktop.Service
{
    /// <summary>
    /// Perform Loading and saving data
    /// </summary>
   public class WorkspaceService
    {
       /// <summary>
       /// Store the value of the user conf folder 
       /// </summary>
       public string WorkspaceFolder
       {
           get;
           private set;
       }

       /// <summary>
       /// Create a new instance of <see cref="WorkspaceService"/>
       /// </summary>
       public WorkspaceService()
       {
          string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
          WorkspaceFolder = Path.Combine(
               basePath,
               "Lucy",
               "Workspaces");
       }
        
     
       /// <summary>
       /// Load local workspaces
       /// </summary>
       /// <returns></returns>
       public IList<WorkspaceModel> Load()
       {
           if(!Directory.Exists(WorkspaceFolder))
           {
               Directory.CreateDirectory(WorkspaceFolder);
               return new List<WorkspaceModel>();
           }

           IList<WorkspaceModel> result = new List<WorkspaceModel>();
           IEnumerable<string> workspaces =  Directory.EnumerateDirectories(WorkspaceFolder, "*", SearchOption.TopDirectoryOnly);
           foreach(var workspace in workspaces)
           {
               WorkspaceModel model = new WorkspaceModel();
               DirectoryInfo dir = new DirectoryInfo(workspace);
               model.Name = dir.Name;
               model.LastModified = dir.LastWriteTime;
               result.Add(model);
           }
           return result;
          
       }

       /// <summary>
       /// Check if the worksapce already exist
       /// </summary>
       /// <param name="workspaceName">Name of the workspace</param>
       /// <returns>True if exist, false otherwise</returns>
       public bool Exist(string workspaceName)
       {
           string path = Path.Combine(WorkspaceFolder, workspaceName);
           return Directory.Exists(path);
       }

       /// <summary>
       /// Create a new Workspace
       /// </summary>
       /// <param name="workspaceName">Name of the workspace to create</param>
       public WorkspaceModel Create( string workspaceName)
       {
           string path = Path.Combine(WorkspaceFolder, workspaceName);
           if(Directory.Exists(path))
           {
               throw new ArgumentException(
                   String.Format("The workspace {0} already exist.", workspaceName)
                   , "workspaceName");
           }

          DirectoryInfo workspaceDir =  Directory.CreateDirectory(path);

           WorkspaceModel workspace = new WorkspaceModel();
           workspace.LastModified = workspaceDir.LastWriteTime;
           workspace.Name = workspaceName;

           return workspace;
       }

      /// <summary>
      /// Physicaly remove a workspace
      /// </summary>
      /// <param name="workspace">Workspace to delete</param>
       public void Remove(WorkspaceModel workspace)
       {
           string path = Path.Combine(WorkspaceFolder, workspace.Name);

           if( Directory.Exists(path))
           {
               Directory.Delete(path, true);
           }
       }
    }
}
