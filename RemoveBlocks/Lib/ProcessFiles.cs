using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
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
        public static Database GetDbFromFile (string dwgFile)
        {
            bool dwgIsOpen = false;

            DocumentCollection docs = Application.DocumentManager;

            // check if dwg is open
            foreach (Document doc  in docs)
            {
                if (doc.Database.Filename == dwgFile)
                {
                    dwgIsOpen = true;

                    //if (docs.MdiActiveDocument != doc)
                    //    docs.MdiActiveDocument = doc;

                    return doc.Database;
                }
            }

            // if dwg is not open then open it
            if (dwgIsOpen == false)
            {
                if (File.Exists(dwgFile))
                {
                    Database exDb = new Database(false, true);
                    exDb.ReadDwgFile(dwgFile, 
                        FileOpenMode.OpenForReadAndWriteNoShare,
                        true, 
                        "");

                    return exDb;
                }
            }

            return null;
        }

        public static List<Database> GetDatabasesFromFiles(List<string> dwgFiles)
        {
            List<Database> result = new List<Database>();
            foreach (var dwgFile in dwgFiles)
            {
                var db = GetDbFromFile(dwgFile);
                if (db != null) result.Add(db);
            }

            return result;
        }
    }
}
