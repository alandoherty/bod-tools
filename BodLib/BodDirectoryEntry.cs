using System;
using System.Collections.Generic;

namespace BodLib
{
	public class BodDirectoryEntry : BodEntry
	{
		#region Fields
		private List<BodFileEntry> _children;
		private int _unknown;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the children.
		/// </summary>
		/// <value>The children.</value>
		public List<BodFileEntry> Children {
			get {
				return _children;
			}
			set {
				_children = value;
			}
		}

		/// <summary>
		/// Gets or sets the unknown.
		/// </summary>
		/// <value>The unknown.</value>
		public int Unknown {
			get {
				return _unknown;
			} set {
				_unknown = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new directory entry.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="offsetName">Offset name.</param>
		/// <param name="unknown">Unknown.</param>
		public BodDirectoryEntry (string name, int offsetName, int unknown)
			:base(name, offsetName)
		{
			_children = new List<BodFileEntry> ();
			_type = BodEntryType.Directory;
			_unknown = unknown;
		}
		#endregion
	}
}

