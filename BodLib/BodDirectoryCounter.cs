using System;

namespace BodLib
{
	public class BodDirectoryCounter
	{
		#region Fields
		private BodDirectoryEntry _entry;
		private int _remaining;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the entry.
		/// </summary>
		/// <value>The entry.</value>
		public BodDirectoryEntry Entry {
			get {
				return _entry;
			}
			set {
				_entry = value;
			}
		}

		/// <summary>
		/// Gets or sets the remaining.
		/// </summary>
		/// <value>The remaining.</value>
		public int Remaining {
			get {
				return _remaining;
			}
			set {
				_remaining = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new directory entry counter.
		/// </summary>
		public BodDirectoryCounter (BodDirectoryEntry entry, int fileCount)
		{
			_entry = entry;
			_remaining = fileCount;
		}
		#endregion
	}
}

