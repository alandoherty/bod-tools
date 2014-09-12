using System;
using System.Windows.Forms;
using System.Drawing;
using BodLib;
using System.Collections.Generic;
using System.IO;

namespace BodEditor
{
	partial class Main : Form
	{
		#region Fields
		private ToolStrip _toolStrip;
		private ToolStripButton _toolStripNew;
		private ToolStripButton _toolStripOpen;
		private ToolStripButton _toolStripSave;
		private TreeView _treeView;
        private ContextMenuStrip _treeFolderMenu;
        private ContextMenuStrip _treeFileMenu;

		private BodFile _file = null;
		private bool _changed = false;
        private BodFileEntry _preview;
		private string _filename = null;
		#endregion

		#region Methods
		/// <summary>
		/// Update the UI.
		/// </summary>
		private void UpdateUI() {
			// update tool strip
			if (_file == null) {
				_toolStripSave.Enabled = false;
			} else {
				_toolStripSave.Enabled = true;
			}

			// update title
			if (_filename == null) {
				Text = "Bod Editor - Untitled";
			} else {
				Text = "Bod Editor - " + Path.GetFileName (_filename) + ((_changed == true) ? "*" : "");
			}
		}

		/// <summary>
		/// Builds the tree.
		/// </summary>
		private void BuildTree() {
			_treeView.Nodes.Clear ();

			// add sub files
			foreach (BodEntry dirEntry in _file.Entries) {
				// check if directory
				if (dirEntry.Type != BodEntryType.Directory)
					return;

				// tree nodes
				TreeNode node = new TreeNode (dirEntry.Name) {
					ImageIndex = 0,
                    SelectedImageIndex = 0,
                    ContextMenuStrip = _treeFolderMenu,
                    Tag = dirEntry
				};

				// get children
				foreach (BodFileEntry fileEntry in ((BodDirectoryEntry)dirEntry).Children) {
                    // node image
                    int image = 1;

                    // determine image
                    switch (Path.GetExtension(fileEntry.Name)) {
                        case ".tga":
                            image = 2; break;
                        case ".opt":
                            image = 3; break;
                        case ".txt":
                            image = 4; break;
                        case ".par":
                            image = 4; break;
                        case ".emi":
                            image = 4; break;
                        case ".app":
                            image = 4; break;
                        case ".mov":
                            image = 4; break;
                        case ".col":
                            image = 5; break;
                        case ".cam":
                            image = 6; break;
                        case ".cut":
                            image = 6; break;
                    }

					node.Nodes.Add (new TreeNode(fileEntry.Name) {
						ImageIndex = image,
                        SelectedImageIndex = image,
                        ContextMenuStrip = _treeFileMenu,
                        Tag = fileEntry
					});
				}
				_treeView.Nodes.Add (node);
			}
		}

		/// <summary>
		/// Create a new bod file.
		/// </summary>
		private void New() {
			if (_changed) {
				DialogResult dr = MessageBox.Show ("Are you sure you want to discard your changes?", 
					"Warning", 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question);

				if (dr == DialogResult.No)
					return;
			}

			_file = new BodFile ();
		}

		/// <summary>
		/// Open a bod file.
		/// </summary>
		private void Open() {
			if (_changed) {
				DialogResult dr = MessageBox.Show ("Are you sure you want to discard your changes?", 
					"Warning", 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question);
                
				if (dr == DialogResult.No)
					return;
			}

			// open file dialog
			OpenFileDialog ofd = new OpenFileDialog ();
			ofd.Title = "Open Bod File";
			ofd.Filter = "Bod File (*.bod)|*.bod|All files (*.*)|*.*";
			if (ofd.ShowDialog () == DialogResult.OK) {
                try
                {
                    // load
                    _file = new BodFile();
                    _file.Load(ofd.FileName);

                    // workspace
                    _changed = false;
                    _filename = ofd.FileName;
                } catch (UnauthorizedAccessException) {
                    // unauthorized access
                    Program.Error("Access denied, run as administrator!");
                } catch (Exception) {
                    // unknown exception
                    Program.Error("File is corrupted, try reinstalling the source game");
                }
			}

            // update stuff
			UpdateUI ();
			BuildTree ();
		}

		/// <summary>
		/// Save bod file.
		/// </summary>
		private void Save() {
            // get filename is needed
            if (_filename == null) {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save Bod File";
                sfd.Filter = "Bod File (*.bod)|*.bod|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _filename = sfd.FileName;
                }
            }

            // save
            _file.Save(_filename);
            _changed = false;
            
            // update stuff
            UpdateUI();
            BuildTree();
		}

        /// <summary>
        /// Extract file.
        /// </summary>
        /// <param name="file">File.</param>
        private void Extract(BodFileEntry file) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Extract File";
            sfd.Filter = "All files (*.*)|*.*";
            sfd.FileName = file.Name;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, file.Data);
            }
        }
		#endregion

		#region Constructors
		public Main ()
		{
			// window
			this.Text = "BOD Editor";
			this.Size = new Size (800, 600);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;

			// tool strip
			_toolStrip = new ToolStrip ();
			this.Controls.Add (_toolStrip);

			// tool strip new
			_toolStripNew = new ToolStripButton ();
			_toolStripNew.Image = Program.ImageNew;
			_toolStripNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
			_toolStripNew.Click += new EventHandler(ToolStripNew_Click);
			_toolStrip.Items.Add (_toolStripNew);

			// tool strip open
			_toolStripOpen = new ToolStripButton ();
			_toolStripOpen.Image = Program.ImageOpen;
			_toolStripOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
			_toolStripOpen.Click += new EventHandler(ToolStripOpen_Click);
			_toolStrip.Items.Add (_toolStripOpen);

			// tool strip save
			_toolStripSave = new ToolStripButton ();
			_toolStripSave.Image = Program.ImageSave;
			_toolStripSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
			_toolStripSave.Click += new EventHandler(ToolStripSave_Click);
			_toolStrip.Items.Add (_toolStripSave);

			// tree view
			_treeView = new TreeView ();
			_treeView.Size = new Size (250, 530);
			_treeView.Location = new Point (10, 30);
			_treeView.ImageList = new ImageList ();
            _treeView.ImageList.Images.AddRange(new Image[] { 
                Program.ImageFolder, Program.ImageFileOther,
                Program.ImageFileTexture, Program.ImageFileOptions, 
                Program.ImageFileCode, Program.ImageFileColours,
                Program.ImageFileCamera
            });
			this.Controls.Add (_treeView);

            // tree file menu
            _treeFileMenu = new ContextMenuStrip();

            ToolStripButton btnExtract = new ToolStripButton("Extract");
            btnExtract.Click += new EventHandler(TreeFileMenuExtract_Click);

            _treeFileMenu.Items.AddRange(new ToolStripItem[] {
               btnExtract
            });

			// update
			New ();
			UpdateUI ();
		}
		#endregion

		#region Events
		private void ToolStripNew_Click(object sender, EventArgs args) {
			New ();
		}

		private void ToolStripOpen_Click(object sender, EventArgs args) {
			Open ();
		}

		private void ToolStripSave_Click(object sender, EventArgs args) {
			Save ();
		}

        private void TreeFileMenuExtract_Click(object sender, EventArgs args) {
            TreeView treeView = (TreeView)_treeFileMenu.SourceControl;
            BodFileEntry fileEntry = (BodFileEntry)treeView.SelectedNode.Tag;

            // ensure selection
            if (fileEntry == null)
                return;

            Extract(fileEntry);
        }
		#endregion
	}
}

