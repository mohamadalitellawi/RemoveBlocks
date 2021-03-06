﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveBlocks.Lib
{
    public static class RemoveBlockFromCadFile
    {
        public static void RemoveAllBlocks(this Database db, string blkName)
        {
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try
            {
                var blkId = GetBlkId(db, blkName);
                if (blkId.IsNull)
                    ed.WriteMessage(string.Format("\n Block not found: {0}", blkName));

                if (!EraseBlkRefs(blkId))
                    ed.WriteMessage(string.Format("\n Failed to Erase BlockReferences for: {0}", blkName));

                if (!EraseBlk(blkId))
                    ed.WriteMessage(string.Format("\n Failed to Erase Block: {0}", blkName));


                string directory = Path.GetDirectoryName(db.Filename);
                string fileNameOnly = Path.GetFileName(db.Filename);
                directory = Path.Combine(directory, "_out");
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                string outputFile = Path.Combine(directory, fileNameOnly);

                db.SaveAs(outputFile, DwgVersion.Current);

                ed.WriteMessage("\n Block Erased: {0} / {1}", blkName, fileNameOnly);
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\nError: {db.Filename}\n{ex.Message}");
            }
        }


        public static ObjectId GetBlkId(Database db, string blkName)
        {

            ObjectId blkId = ObjectId.Null;

            if (db == null)
                return ObjectId.Null;

            if (string.IsNullOrWhiteSpace(blkName))
                return ObjectId.Null;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                if (bt.Has(blkName))
                    blkId = bt[blkName];
                tr.Commit();
            }
            return blkId;
        }


        public static bool EraseBlkRefs(ObjectId blkId)
        {
            bool blkRefsErased = false;

            if (blkId.IsNull)
                return false;

            Database db = blkId.Database;
            if (db == null)
                return false;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTableRecord blk = (BlockTableRecord)tr.GetObject(blkId, OpenMode.ForRead);
                var blkRefs = blk.GetBlockReferenceIds(true, true);
                if (blkRefs != null && blkRefs.Count > 0)
                {
                    foreach (ObjectId blkRefId in blkRefs)
                    {
                        BlockReference blkRef = (BlockReference)tr.GetObject(blkRefId, OpenMode.ForWrite);
                        blkRef.Erase();
                    }
                    blkRefsErased = true;
                }
                tr.Commit();
            }
            return blkRefsErased;
        }



        public static bool EraseBlk(ObjectId blkId)
        {
            bool blkIsErased = false;

            if (blkId.IsNull)
                return false;

            Database db = blkId.Database;
            if (db == null)
                return false;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                BlockTableRecord blk = (BlockTableRecord)tr.GetObject(blkId, OpenMode.ForRead);
                var blkRefs = blk.GetBlockReferenceIds(true, true);
                if (blkRefs == null || blkRefs.Count == 0)
                {
                    blk.UpgradeOpen();
                    blk.Erase();
                    blkIsErased = true;
                }
                tr.Commit();
            }
            return blkIsErased;
        }
    }
}
