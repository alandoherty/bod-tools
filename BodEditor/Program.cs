using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace BodEditor
{
	public static class Program
	{
		#region Fields
		public static Image ImageNew;
		public static Image ImageOpen;
		public static Image ImageSave;
		public static Image ImageFolder;
		public static Image ImageFileOther;
		public static Image ImageFileTexture;
        public static Image ImageFileOptions;
        public static Image ImageFileCode;
        public static Image ImageFileColours;
        public static Image ImageFileCamera;
		#endregion

		#region Methods
        /// <summary>
        /// Display an error message.
        /// </summary>
        /// <param name="msg">Message</param>
        public static void Error(string msg) {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
        [STAThread]
		public static void Main(string[] args) {
			// setup
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (true);

			// check for data folder
            if (!Directory.Exists("data")) {
                Program.Error("Unable to start editor, data folder missing");
                return;
            }

			// load images
            try {
                ImageNew = Image.FromFile("data/new.png");
                ImageOpen = Image.FromFile("data/open.png");
                ImageSave = Image.FromFile("data/save.png");
                ImageFolder = Image.FromFile("data/folder.png");
                ImageFileOther = Image.FromFile("data/file_other.png");
                ImageFileTexture = Image.FromFile("data/file_texture.png");
                ImageFileOptions = Image.FromFile("data/file_options.png");
                ImageFileCode = Image.FromFile("data/file_code.png");
                ImageFileColours = Image.FromFile("data/file_colours.png");
                ImageFileCamera = Image.FromFile("data/file_camera.png");
            } catch (Exception) {
                Program.Error("Unable to start editor, data files missing");
                return;
            }

			// run
			Application.Run (new Main ());
		}
		#endregion
	}
}

