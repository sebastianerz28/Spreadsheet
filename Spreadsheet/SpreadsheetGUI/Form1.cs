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
        //Backing spreadsheet
        private Spreadsheet spreadsheet;
        private bool showAllFiles;
        public Form1()
        {
            spreadsheet = new Spreadsheet(s => Regex.IsMatch(s, @"[A-Z][1-9][0-9]?"), s=>s.ToUpper(), "ps6");
            InitializeComponent();
            showAllFiles = false;
            spreadsheetPanel1.SelectionChanged += setCellText;
            currentCellLabel.Text = "Current cell: A1";
        }


        /// <summary>
        /// Sets the text of the labels when a cell is selected with a single click
        /// </summary>
        /// <param name="panel"></param>
        private void setCellText(global::SS.SpreadsheetPanel panel)
        {
            int row, col;
            String value;
            panel.GetSelection(out col, out row);
            panel.GetValue(col, row, out value);
            
            currentCellLabel.Text = "Current cell: " + ((char)(col + 65)) + (row +1);
            textBox1.Text = "" + spreadsheet.GetCellContents(""+((char)(col + 65)) + (row + 1));
            contentsOfCellLabel.Text = "Cell value: " + spreadsheet.GetCellValue("" + ((char)(col + 65)) + (row + 1));

        }



        /// <summary>
        /// When the new button in file is clicked creates a new spreadsheet window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.getAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// Toggles if all files will be shown when saving and opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            showAllFiles = !showAllFiles;
        }
        /// <summary>
        /// Saves an item by opening savefiledialog and uses backing Spreadsheet.Save to save the spreadsheet as a .sprd document in xml formatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if(showAllFiles == true)
            {
                saveFileDialog.Filter = "Speadsheet files|*.sprd";
            }
            else
            {
                saveFileDialog.Filter = "";
            }
            saveFileDialog.CheckFileExists = false;
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
        /// <summary>
        /// Opens .sprd file and displays it in the spreadsheet.
        /// popup window will occur if error occurs while reading the file and building the spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (showAllFiles == true)
            {
                openFileDialog1.Filter = "Speadsheet files|*.sprd";
            }
            else
            {
                openFileDialog1.Filter = "";
            }
            openFileDialog1.CheckFileExists = false;
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
                    Spreadsheet newSpread = new Spreadsheet(filename, s => Regex.IsMatch(s, @"[A-Z][1-9][0-9]?"), s => s.ToUpper(), "ps6");
                    spreadsheet = newSpread;
                    foreach(string cell in newSpread.GetNamesOfAllNonemptyCells())
                    {
                        int c = cell[0] - 65;
                        int r = int.Parse(getRow(cell)) - 1;
                        this.spreadsheetPanel1.SetValue(c, r, spreadsheet.GetCellValue(cell).ToString());
                    }
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
                        Spreadsheet newSpread = new Spreadsheet(filename + defaultExtension, s => Regex.IsMatch(s, @"[A-Z][1-9][0-9]?"), s => s.ToUpper(), "ps6");
                        spreadsheet = newSpread;
                        foreach (string cell in newSpread.GetNamesOfAllNonemptyCells())
                        {
                            int c = cell[0] - 65;
                            int r = int.Parse(getRow(cell)) - 1;
                            this.spreadsheetPanel1.SetValue(c, r, spreadsheet.GetCellValue(cell).ToString());
                        }
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
                    MessageBox.Show("Error Opening Spreadsheet");
                }
               
            }


            saveFileDialog.Dispose();
        }
        /// <summary>
        /// Overrides the form closing button to prompt you to save your code, retains the ability to close without saving and to cancel the current
        /// window and continue editing the spreadsheet
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Would you like to save your work before closing", "Closing", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    save();
                    break;
                case DialogResult.No:
                    break;
                default:
                    e.Cancel = true;
                    break;
            }

        }
        /// <summary>
        /// Helper method for both buttons that save
        /// </summary>
        private void save()
        {

            if (showAllFiles == true)
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
        /// <summary>
        /// closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(spreadsheet.Changed == true)
            {
                MessageBox.Show("You haven't saved click red X if you would like to close without saving.");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }
        /// <summary>
        /// Updates all cell values when change contents of cell button is clicked
        /// Updates labels to changes
        /// If any error occurs during change nothing will change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeCellContentsButton_Click(object sender, EventArgs e)
        {
            try
            {
                string contents = textBox1.Text;
                this.spreadsheetPanel1.GetSelection(out int col, out int row);
                string cellName = "" + ((char)(col + 65)) + (row + 1);
                IList<string> cells = spreadsheet.SetContentsOfCell(cellName, contents);
                foreach (string cell in cells)
                {

                    int c = cell[0] - 65;
                    int r = int.Parse(getRow(cell)) - 1;
                    this.spreadsheetPanel1.SetValue(c, r, spreadsheet.GetCellValue(cell).ToString());

                }
                contentsOfCellLabel.Text = "Value of cell: " + spreadsheet.GetCellValue(cellName).ToString();
            }
            catch(Exception exc)
            {
                contentsOfCellLabel.Text = exc.Message;
                MessageBox.Show(exc.Message + "\nNothing was changed");
            }
            
        }
        /// <summary>
        /// Gets the row of a cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string getRow(string cell)
        {
            string s = "";
            for(int i = 1; i < cell.Length; i++)
            {
                s += cell[i];
            }
            return s;
        }
        /// <summary>
        /// Displays Info panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }

        
    }
}
