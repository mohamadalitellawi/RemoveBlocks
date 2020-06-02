using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using RemoveBlocks.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveBlocks
{
    public class Commands
    {
        [CommandMethod("wwRemoveBlocks")]
        public void RemoveBlocksByName()
        {
            var editor = Application.DocumentManager.MdiActiveDocument.Editor;

            editor.WriteMessage("\nSelect Files To Process : ");
            List<string> dwgFiles = GetFiles.GetAllCadFilesFromDialog();
            if (dwgFiles == null || dwgFiles.Count == 0) return;

            
            string blockName = editor.GetString("\nEnter Block Name : ").StringResult;
            if (string.IsNullOrWhiteSpace(blockName)) return;

            ProcessFiles.GetDatabasesFromFiles(dwgFiles).ForEach(db => db.RemoveAllBlocks(blockName));

            editor.WriteMessage($"\nFinish  /{DateTime.Now.ToShortTimeString()}/ !");
        }
    }
}
