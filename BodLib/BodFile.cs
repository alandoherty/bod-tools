using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BodLib
{
	public class BodFile
	{
		#region Fields
		private string _filename;
		private string _filenameData;
		private List<BodEntry> _entries;

		private static byte[] _dirTypeBytes = Encoding.ASCII.GetBytes("DIRY");
		private static byte[] _fileTypeBytes = Encoding.ASCII.GetBytes("FILE");
		#endregion

		#region Properties
		/// <summary>
		/// Gets the filename.
		/// </summary>
		/// <value>The filename.</value>
		public string Filename {
			get {
				return _filename;
			}
		}

		/// <summary>
		/// Gets the entries.
		/// </summary>
		/// <value>The entries.</value>
		public List<BodEntry> Entries {
			get {
				return _entries;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Load the specified filename.
		/// </summary>
		/// <param name="filename">Filename.</param>
		public void Load(string filename) {
			// set filename
			_filename = filename;
			_filenameData = Path.ChangeExtension (_filename, ".bob");

			// load index
			using (FileStream fs = File.Open (filename, FileMode.Open)) {
				BinaryReader br = new BinaryReader (fs);

				// seek to footer
				br.BaseStream.Seek (-16, SeekOrigin.End);

				// footer
				int filesDataOffset = br.ReadInt32 ();
				int filesCount = br.ReadInt32 ();
				br.ReadInt32 ();

				// read files
				br.BaseStream.Seek (filesDataOffset, SeekOrigin.Begin);

				// directory entries
				Queue<BodDirectoryCounter> dirAssign = new Queue<BodDirectoryCounter> ();
				BodDirectoryCounter curAssign = null;

				// check if empty
				if (filesCount == 0)
					return;

				for (int i = 0; i < filesCount; i++) {
					// determine entry type
					string type = Encoding.ASCII.GetString (br.ReadBytes (4));

					if (type == "FILE") {
						// read data
						int fileSize = br.ReadInt32 ();
						int fileOffset = br.ReadInt32 ();
						int offsetName = br.ReadInt32 ();

						// seek and read name
						long oldPos = br.BaseStream.Position;
						br.BaseStream.Seek (offsetName, SeekOrigin.Begin);
						string name = BodUtils.CleanName(BodUtils.ReadStringNT (br));
						br.BaseStream.Seek (oldPos, SeekOrigin.Begin);

						// find parent
						while (dirAssign != null & (curAssign == null || curAssign.Remaining == 0))
							curAssign = dirAssign.Dequeue ();

						curAssign.Remaining--;

						// create entry
						_entries.Add(new BodFileEntry (name, offsetName, fileOffset, fileSize, curAssign.Entry));
						curAssign.Entry.Children.Add ((BodFileEntry)_entries [_entries.Count - 1]);
					} else if (type == "DIRY") {
						// read data
						int fileCount = br.ReadInt32 ();
						int unknown = br.ReadInt32 ();
						int offsetName = br.ReadInt32 ();

						// seek and read name
						long oldPos = br.BaseStream.Position;
						br.BaseStream.Seek (offsetName, SeekOrigin.Begin);
						string name = BodUtils.CleanName(BodUtils.ReadStringNT (br));
						br.BaseStream.Seek (oldPos, SeekOrigin.Begin);

						// create entry
						_entries.Add(new BodDirectoryEntry (name, offsetName, unknown));

						// add assigner
						dirAssign.Enqueue (new BodDirectoryCounter ((BodDirectoryEntry)_entries[_entries.Count - 1], fileCount));
					} else {
						throw new NotImplementedException ("Unable to process entry format '" + type + "', newer format?");
					}
				}
			}

			// check for data file
			if (!File.Exists (_filenameData))
				return;

			// load data
			using (FileStream fs = File.Open (_filenameData, FileMode.Open)) {
				BinaryReader br = new BinaryReader (fs);

				// read
				foreach (BodEntry entry in _entries) {
					if (entry.Type == BodEntryType.File) {
						// cast entry
						BodFileEntry file = (BodFileEntry)entry;

						// seek to data
						br.BaseStream.Seek (file.FileOffset, SeekOrigin.Begin);
						file.Data = br.ReadBytes (file.FileSize);
					}
				}
			}
		}

		/// <summary>
		/// Save index file.
		/// </summary>
		public void Save() {
			// check filename provided from load
			if (_filename == null)
				throw new InvalidOperationException ("Unable to save, no filename provided");

			// save
			Save (_filename);
		}

		/// <summary>
		/// Save the specified filename.
		/// </summary>
		/// <param name="filename">Filename.</param>
		public void Save(string filename) {
			using (FileStream fs = File.Open(filename, FileMode.Create)) {
				BinaryWriter bw = new BinaryWriter (fs);

				// offsets
				int fileDataOffset = 0;
				int filesCount = _entries.Count;
				int fileNameOffset = 0;

				// write file names
				fileNameOffset = (int)bw.BaseStream.Position;
				for (int i = 0; i < _entries.Count; i++) {
					BodUtils.WriteStringNT (bw, _entries [i].Name);
				}

				// write file data
				fileDataOffset = (int)bw.BaseStream.Position;
				for (int i = 0; i < _entries.Count; i++) {
					// write type
					if (_entries [i].Type == BodEntryType.Directory) {
						bw.Write (_dirTypeBytes);

						// write other data
						bw.Write (((BodDirectoryEntry)_entries [i]).Children.Count);
						bw.Write (((BodDirectoryEntry)_entries [i]).Unknown);
						bw.Write (_entries [i].OffsetName);
					} else {
						bw.Write (_fileTypeBytes);

						// write other data
						bw.Write (((BodFileEntry)_entries [i]).FileSize);
						bw.Write (((BodFileEntry)_entries [i]).FileOffset);
						bw.Write (_entries [i].OffsetName);
					}
				}

				// write footer
				bw.Write (fileDataOffset);
				bw.Write (filesCount);
				bw.Write (fileNameOffset);
				bw.Write (fileDataOffset);
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a index file instance.
		/// </summary>
		public BodFile ()
		{
			_entries = new List<BodEntry> ();
			_filename = null;
		}
		#endregion
	}
}

