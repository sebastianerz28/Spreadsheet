using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using System.Text.RegularExpressions;
namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        private readonly string defaultExtension = ".sprd";
        private Spreadsheet spreadsheet;
        private bool showAllFiles;
        public Form1()
        {
            spreadsheet = new Spreadsheet(s => Regex.IsMatch(s, @"$[A - Z][1-9][1-9]?"), s=>s.ToUpper(), "ps6");
            InitializeComponent();
            showAllFiles = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.getAppContext().RunForm(new Form1());
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            
            
        }

        private void saveShowingsprdOnlyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "Speadsheet files|*.sprd";

            saveFileDialog.FileName = "untitled";
            saveFileDialog.DefaultExt = "sprd";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();
            string filename = saveFileDialog.FileName;
            if (filename.EndsWith(defaultExtension))
            {
                spreadsheet.Save(filename);
            }
            else
            {
                spreadsheet.Save(filename + defaultExtension);
            }
            
            saveFileDialog.Dispose();
        }

        private void saveShowingAllFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = "untitled";
            saveFileDialog.DefaultExt = "sprd";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();
            
            string filename = saveFileDialog.FileName;
            if (filename.EndsWith(defaultExtension))
            {
                spreadsheet.Save(filename);
            }
            else
            {
                spreadsheet.Save(filename + defaultExtension);
            }

            
            saveFileDialog.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            showAllFiles = !showAllFiles;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(showAllFiles);
            if(showAllFiles == true)
            {
                saveFileDialog.Filter = "Speadsheet files|*.sprd";
            }
            else
            {
                saveFileDialog.Filter = "";
            }
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = "untitled";
            saveFileDialog.DefaultExt = "sprd";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();
            
            string filename = saveFileDialog.FileName;
            if (filename.EndsWith(defaultExtension))
            {
                spreadsheet.Save(filename);
            }
            else
            {
                spreadsheet.Save(filename + defaultExtension);
            }


            saveFileDialog.Dispose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(showAllFiles);
            if (showAllFiles == true)
            {
                openFileDialog1.Filter = "Speadsheet files|*.sprd";
            }
            else
            {
                openFileDialog1.Filter = "";
            }
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = "untitled";
            openFileDialog1.DefaultExt = "sprd";
            openFileDialog1.AddExtension = true;
            openFileDialog1.ShowDialog();
            
            string filename = openFileDialog1.FileName;
            if (filename.EndsWith(defaultExtension))
            {
                if (spreadsheet.GetSavedVersion(filename) == "ps6")
                {
                    Spreadsheet newSpread = new Spreadsheet(filename, s => Regex.IsMatch(s, @"$[A - Z][1-9][1-9]?"), s => s.ToUpper(), "ps6");
                }
                else
                {
                    Error f = new Error();
                    f.setText("Version is not ps6");
                    f.Show();
                }
                
            }
            else
            {
                try
                {
                    if (spreadsheet.GetSavedVersion(filename + defaultExtension) == "ps6")
                    {
                        Spreadsheet newSpread = new Spreadsheet(filename + defaultExtension, s => Regex.IsMatch(s, @"$[A - Z][1-9][1-9]?"), s => s.ToUpper(), "ps6");
                    }
                    else
                    {
                        Error f = new Error();
                        f.setText("Version is not ps6");
                        f.Show();
                    }
                }
                catch (SpreadsheetReadWriteException)
                {
                    Error f = new Error();
                    f.setText("Error opening the spreadsheet");
                    f.Show();
                }
               
            }


            saveFileDialog.Dispose();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(spreadsheet.Changed == true)
            {
                Error f = new Error();
                f.setText("You have not saved!");
                f.Show();
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
