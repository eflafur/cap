using System;

namespace GruppoCap
{

	public static class PathUtils
	{

		// COMBINE TOKENs
		public static String CombineTokens(params String[] pathTokens)
		{
			return StringUtils.CombineStringsWithCharSeparator('\\', pathTokens);
		}

		// REMOVE FILE NAME EXTENSION
		public static String RemoveFileNameExtension(String path, Boolean allLevels = false)
		{
			if (path.IsNullOrWhiteSpace())
				return path;

			Int32 pos;

			if (allLevels)
			{
				pos = path.IndexOf('.');
			}
			else
			{
				pos = path.LastIndexOf('.');
			}

			if (pos < 0)
				return path;

			return path.Substring(0, pos);
		}

		// GET PARENT DIRECTORY PATH
		public static String GetParentDirectoryPath(String path)
		{
			if (path.IsNullOrWhiteSpace())
				return path;

			try
			{
				Int32 pos;

				pos = path.LastIndexOfAny(new Char[] { '\\', '/' });

				if (pos >= 0 && path.Length > pos)
					return path.Substring(0, pos);

				return path;
			}
			catch
			{
				return null;
			}
		}

		// EXTRACT FILE NAME
		public static String ExtractFileName(String path, Boolean removeExtension = false, Boolean allLevels = false)
		{
			// CHECK - NULL
			if (path.IsNullOrWhiteSpace())
				return path;

			String res;

			res = path;

			try
			{
				Int32 pos;

				// FIND THE LAST INDEX OF '\' OR '/'
				pos = res.LastIndexOfAny(new Char[] { '\\', '/' });

				// AND CUT
				if (pos >= 0 && res.Length > pos)
					res = res.Substring(pos + 1);
			}
			catch
			{
				res = null;
			}

			// CHECK - NULL
			if (res.IsNullOrWhiteSpace())
				return res;

			if (removeExtension)
			{
				try
				{
					res = RemoveFileNameExtension(res, allLevels);
				}
				catch
				{
					res = null;
				}
			}

			return res;
		}

		// REBASE FILE FULL PATH TO
		public static String RebaseFileFullPathTo(String fileFullpath, String newPath)
		{
			if (newPath.IsNullOrWhiteSpace())
				return null;

			try
			{
				return CombineTokens(newPath, ExtractFileName(fileFullpath));
			}
			catch
			{
				return null;
			}
		}

		// APPEND FILE NAME EXTENSION
		public static String AppendFileNameExtension(String path, String ext)
		{
			if (path.IsNullOrWhiteSpace())
				return path;

			Ensure.Arg(() => ext).IsNotNullOrWhiteSpace();

			return path.TrimEnd('.') + "." + ext.TrimStart('.');
		}

		// SET FILE NAME EXTENSION
		public static String SetFileNameExtension(String path, String ext, Boolean allLevels = false)
		{
			return AppendFileNameExtension(RemoveFileNameExtension(path, allLevels), ext);
		}

		// REMOVE TRAILING PATH SEPARATOR
		public static String RemoveTrailingPathSeparator(String path)
		{
			if (path.IsNullOrWhiteSpace())
				return path;

			return path.TrimEnd('\\');
		}

		// UP ONE LEVEL FOLDER
		public static String UpOneLevelFolder(String path)
		{
			if (path.IsNullOrWhiteSpace())
				return path;

			String s;
			Int32 pos;

			s = RemoveTrailingPathSeparator(path);
			pos = s.LastIndexOf('\\');

			if (pos < 0)
				return s;

			return s.Substring(0, pos);
		}

	}

}