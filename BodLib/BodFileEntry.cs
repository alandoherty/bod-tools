using System;

namespace BodLib
{
	public class BodFileEntry : BodEntry
	{
		#region Fields
		protected int _fileOffset;
		protected byte[] _data;
		protected int _fileSize;
		protected BodDirectoryEntry _parent;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the offset of the data.
		/// </summary>
		/// <value>The offset.</value>
		public int FileOffset {
			get {
				return _fileOffset;
			}
			set {
				_fileOffset = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the file.
		/// </summary>
		/// <value>The size of the file.</value>
		public int FileSize {
			get {
				return _fileSize;
			}
			set {
				_fileSize = value;
			}
		}

		/// <summary>
		/// Gets or sets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public BodDirectoryEntry Parent {
			get {
				return _parent;
			}
			set {
				_parent = value;
			}
		}

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public byte[] Data {
			get {
				return _data;
			}
			set {
				_data = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new file entry.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="offsetName">Offset name.</param>
		/// <param name="fileOffset">File offset.</param>
		/// <param name="fileSize">File size.</param>
		/// <param name="parent">Parent.</param>
		public BodFileEntry (string name, int offsetName, int fileOffset, int fileSize, BodDirectoryEntry parent)
			: base (name, offsetName)
		{
			_type = BodEntryType.File;
			_data = new byte[] { };
			_fileOffset = fileOffset;
			_fileSize = fileSize;
			_parent = parent;
		}
		#endregion
	}
}

