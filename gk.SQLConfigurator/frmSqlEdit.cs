using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using System.IO;

namespace gk.SQLConfigurator
{
    public partial class frmSqlEdit : Form
    {
        public string sql{ get { return editor.Text; } set { editor.Text = value; } }
        private ThisAddIn addin;
        public frmSqlEdit(ThisAddIn _addin)
        {
            addin = _addin;
            InitializeComponent();

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\gk.SQLConfigurator\";
            FileSyntaxModeProvider fsmProvider; // Provider

            if (Directory.Exists(dir))
            {
                string path = dir + "SQL-Mode.xshd";
                if (!File.Exists(path))
                {
                    File.WriteAllBytes(path, global::gk.SQLConfigurator.Properties.Resources.SQL_Mode);
                }
                fsmProvider = new FileSyntaxModeProvider(dir); // Create new provider with the highlighting directory.
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
                editor.SetHighlighting("SQL"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            }

            editor.ShowEOLMarkers = false;
            editor.ShowSpaces = false;
            editor.ShowTabs = false;
            editor.ShowInvalidLines = false;
            editor.AllowCaretBeyondEOL = true;
            if (addin == null)
            {
                //btnSave.Hide();
                btnCheck.Hide();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (addin == null)
            {
                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.sql)|*.sql|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(sql);
                        myStream.Write(bytes, 0, bytes.Length);
                        myStream.Close();
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            sql = editor.Text;
            this.Close();

        }

        private void frmSqlEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnCheck_Click(sender, null);
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            addin.ribbon_ActionCLick(false, true);
        }

        private void editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnCheck_Click(sender, null);
            }
        }
    }
}
