using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap
{
    public static class IOUtils
    {
        // COPY DIRECTORY
        public static Boolean CopyDirectory(String sourceFolderPath, String destinationFolderPath, Boolean overwriteExistingFiles)
        {
            Boolean res = false;

            try
            {
                sourceFolderPath = sourceFolderPath.EndsWith(@"\") ? sourceFolderPath : sourceFolderPath + @"\";
                destinationFolderPath = destinationFolderPath.EndsWith(@"\") ? destinationFolderPath : destinationFolderPath + @"\";

                if (Directory.Exists(sourceFolderPath))
                {
                    if (Directory.Exists(destinationFolderPath) == false)
                        Directory.CreateDirectory(destinationFolderPath);

                    FileInfo fi;

                    foreach (String fls in Directory.GetFiles(sourceFolderPath))
                    {
                        fi = new FileInfo(fls);
                        fi.CopyTo(destinationFolderPath + fi.Name, overwriteExistingFiles);
                    }

                    DirectoryInfo di;
                    foreach (String drs in Directory.GetDirectories(sourceFolderPath))
                    {
                        di = new DirectoryInfo(drs);

                        if (CopyDirectory(drs, destinationFolderPath + di.Name, overwriteExistingFiles) == false)
                            res = false;
                    }
                }

                res = true;
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        // SET DIRECTORY ATTRIBUTEs
        public static void SetDirectoryAttributes(String folderPath, FileAttributes fileAttributes)
        {
            try
            {
                // SET THE FOLDER'S ATTRIBUTEs
                new DirectoryInfo(folderPath).Attributes = fileAttributes;

                // LOOP TROUGH THE FILEs
                foreach (String file in Directory.GetFiles(folderPath))
                {
                    // SET THE FILE'S ATTRIBUTEs
                    File.SetAttributes(file, fileAttributes);
                }

                // LOOP TROUGH HE FOLDERs
                foreach (String dir in Directory.GetDirectories(folderPath))
                {
                    SetDirectoryAttributes(dir, fileAttributes);
                }
            }
            catch (Exception)
            {
                // EMPTY
            }
        }

        // RESET DIRECTORY ATTRIBUTEs
        public static void ResetDirectoryAttributes(String folderPath)
        {
            SetDirectoryAttributes(folderPath, FileAttributes.Normal);
        }

        // TRY FILE DELETE
        public static Boolean TryFileDelete(String filename)
        {
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                return File.Exists(filename) == false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // TRY DIRECTORY DELETE
        public static Boolean TryDirectoryDelete(String path, Boolean recursive)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, recursive);

                return Directory.Exists(path) == false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // IS FILE AVAILABLE FOR WRITE
        public static Boolean IsFileAvailableForWrite(String filename)
        {
            Boolean res;

            try { using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite)) { res = fs.CanWrite; } }
            catch { res = false; }

            return res;
        }

        // GET FIRST AVAILABLE FILE FOR WRITE
        public static String GetFirstFileAvailableForWrite(String folderPath, String searchPattern, SearchOption searchOption)
        {
            // CHECK - FOLDER EXISTs
            if (Directory.Exists(folderPath) == false)
                return null;

            String[] files;

            // GET THE FILEs IN THE FOLDER
            files = Directory.GetFiles(folderPath, searchPattern, searchOption);

            // CHECK - THERE ARE FILEs
            if (files.Length == 0)
                return null;

            foreach (String file in files)
            {
                if (IsFileAvailableForWrite(file))
                    return file;
            }

            return null;
        }

        // GET FIRST FILENAME NOT IN USE
        public static String GetFirstFilenameNotInUse(String fileFullPath, Int32 min, Int32 max)
        {
            Ensure.Arg(() => fileFullPath).IsNotNullOrWhiteSpace();
            Ensure.Arg(() => max).IsGreaterThan(min, inclusive: true);

            String basePath;
            String filenamePattern;
            String fn;

            basePath = PathUtils.GetParentDirectoryPath(fileFullPath);
            filenamePattern = PathUtils.ExtractFileName(fileFullPath);

            // PLACEHOLDER CHECK
            if (filenamePattern.IndexOf(@"{0}") < 0)
            {
                // PLACEHOLDER NOT THERE -> APPEND IT
                String leftPart;
                String rightPart;

                leftPart = PathUtils.ExtractFileName(filenamePattern, true, true);
                rightPart = filenamePattern.Substring(leftPart.Length);

                filenamePattern = leftPart + @"-{0}" + rightPart;
            }

            for (int i = min; i <= max; i++)
            {
                // BUILD THE FILENAME
                fn = PathUtils.CombineTokens(basePath, String.Format(filenamePattern, i));

                // CHECK IF THE FILE EXISTs
                if (File.Exists(fn) == false)
                    return fn;
            }

            return null;
        }

        // READ ALL BYTEs OR NULL
        public static Byte[] ReadAllBytesOrNull(String path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch
            {
                return null;
            }
        }

        // GET LAST WRITE TIME UTC OR NULL
        public static DateTime? GetLastWriteTimeUtcOrNull(String path)
        {
            try
            {
                return File.GetLastWriteTimeUtc(path);
            }
            catch
            {
                return null;
            }
        }

    }
}
