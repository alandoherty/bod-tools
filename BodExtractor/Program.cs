using System;
using BodLib;
using System.IO;

namespace BodExtractor
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length == 0) {
				Console.WriteLine ("No file provided for extraction!");
				return;
			}

			// load file
			BodFile file = new BodFile ();
			file.Load (args [0]);

			// extract
			int count = 0;
			foreach (BodEntry entry in file.Entries) {
				if (entry.Type == BodEntryType.File) {
					File.WriteAllBytes (entry.Name, ((BodFileEntry)entry).Data);
					count++;
				}
			}

			Console.WriteLine ("Successfully extracted " + count + " files");
		}
	}
}
