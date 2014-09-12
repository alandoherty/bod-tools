using System;

namespace BodLib
{
	public class BodEntry
	{
		#region Fields
		protected string _name;
		protected int _offsetName;
		protected BodEntryType _type;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		/// <summary>
		/// Gets or sets the offset of the name.
		/// </summary>
		/// <value>The name of the offset.</value>
		public int OffsetName {
			get {
				return _offsetName;
			}
			set {
				_offsetName = value;
			}
		}

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public BodEntryType Type {
			get {
				return _type;
			}
			set {
				_type = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new bod entry.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="offsetName">Offset name.</param>
		public BodEntry (string name, int offsetName)
		{
			_name = name;
			_offsetName = offsetName;
		}
		#endregion
	}

	public enum BodEntryType
	{
		File,
		Directory
	}
}

