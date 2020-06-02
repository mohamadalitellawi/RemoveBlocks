using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveBlocks.Lib
{
    public class ProcessFiles
    {
        public static void RunAction (string dwgFile, Action action)
        {
            bool dwgIsOpen = false;

            DocumentCollection docs = Application.DocumentManager;

            // check if dwg is open
            foreach (Document doc  in docs)
            {
                if (doc.Database.Filename == dwgFile)
                {
                    dwgIsOpen = true;

                    if (docs.MdiActiveDocument != doc)
                        docs.MdiActiveDocument = doc;

                    action();
                    break;
                }
            }

            // if dwg is not open then open it
            if (dwgIsOpen == false)
            {
                if (File.Exists(dwgFile))
                {
                    Document doc = docs.Open(dwgFile, false);
                    docs.MdiActiveDocument = doc;

                    action();
                }
            }
        }

        public static void StartProcessingFiles(List<string> dwgFiles, Action action)
        {
            foreach (var dwgFile in dwgFiles)
            {
                RunAction(dwgFile, action);
            }
        }
    }
}
