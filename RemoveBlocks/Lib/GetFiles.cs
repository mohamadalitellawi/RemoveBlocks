using Autodesk.AutoCAD.Publishing;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveBlocks.Lib
{
    public class GetFiles
    {
        public static List<string> GetAllCadFilesFromDialog()
        {
            List<string> results = new List<string>();

            OpenFileDialog ofd = new OpenFileDialog("Select Cad Files To Process",
                null, "dwg; dxf; *",
                "SelectFileToProcess",
                 OpenFileDialog.OpenFileDialogFlags.AllowMultiple);

            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                results = ofd.GetFilenames().ToList();
            }
            return results;
        }
    }
}
