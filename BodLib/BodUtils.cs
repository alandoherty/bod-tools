using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BodLib
{
	public static class BodUtils
	{
		#region Methods
		/// <summary>
		/// Read until null terminator.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="br">Binary Reader.</param>
		public static string ReadStringNT(BinaryReader br) {
			char c = 'F';
			List<char> str = new List<char> ();

			// read until null terminator
			while (c != 0) {
				c = br.ReadChar ();
				str.Add (c);
			}

			return new string (str.ToArray ());
		}

		/// <summary>
		/// Write with null terminator.
		/// </summary>
		/// <param name="bw">Binary Writer.</param>
		/// <param name="str">The string.</param>
		public static void WriteStringNT(BinaryWriter bw, string str) {
			List<byte> arr = new List<byte>(Encoding.ASCII.GetBytes(str));

			bw.Write(arr.ToArray());
		}

        /// <summary>
        /// Clean file names due to compatability issues
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Clean Name</returns>
        public static string CleanName(string name) {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            name = r.Replace(name, "");
            return name;
        }
		#endregion
	}
}

