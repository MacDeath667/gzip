using System;
using System.IO;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Validators
{
	internal static class ArgumentsValidateHelper
	{
		internal static bool IsInputFileExists(string filepath)
		{
			var fileInfo = new FileInfo(filepath);
			Console.WriteLine(fileInfo.Exists
				? $"Found file with size {fileInfo.Length}"
				: $"File not found on the filepath: {filepath}. Check input filepath an arguments");
			return fileInfo.Exists;
		}

		internal static bool IsValidOutpath(string filepath)
		{
			try
			{
				File.Create(filepath).Close();
				File.Delete(filepath);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Output file can not be write on the filepath: {filepath} by reason: {e.Message}" +
				                  $"{Environment.NewLine}Check input filepath an arguments");

				Console.WriteLine(e.Message);
				return false;
			}
		}

		internal static bool IsValidGzipActionType(GzipActionType parsedArgsGzipActionType)
		{
			var isValidType = parsedArgsGzipActionType != GzipActionType.Unknown;
			Console.WriteLine(isValidType
				? $"Selected file will be processed with {parsedArgsGzipActionType.ToString()} option."
				: "Check action option in CLI arguments. Must be \"Compress\" or \"Decompress\" option.");
			return isValidType;
		}
	}
}