
namespace SpreadsheetGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveShowingAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveShowingsprdOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.showFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.currentCellLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contentsOfCellLabel = new System.Windows.Forms.Label();
            this.changeCellContentsButton = new System.Windows.Forms.Button();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 50);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1487, 831);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1487, 24);
            this.menuStrip1.TabIndex = 1;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // FileButton
            // 
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.saveMenuItem,
            this.NewMenuItem});
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(37, 20);
            this.FileButton.Text = "File";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openMenuItem.Text = "Open";
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveShowingAllFilesToolStripMenuItem,
            this.saveShowingsprdOnlyToolStripMenuItem});
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveMenuItem.Text = "Save";
            // 
            // saveShowingAllFilesToolStripMenuItem
            // 
            this.saveShowingAllFilesToolStripMenuItem.Name = "saveShowingAllFilesToolStripMenuItem";
            this.saveShowingAllFilesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.saveShowingAllFilesToolStripMenuItem.Text = "Save showing all files";
            // 
            // saveShowingsprdOnlyToolStripMenuItem
            // 
            this.saveShowingsprdOnlyToolStripMenuItem.Name = "saveShowingsprdOnlyToolStripMenuItem";
            this.saveShowingsprdOnlyToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.saveShowingsprdOnlyToolStripMenuItem.Text = "Save showing .sprd only";
            // 
            // NewMenuItem
            // 
            this.NewMenuItem.Name = "NewMenuItem";
            this.NewMenuItem.Size = new System.Drawing.Size(103, 22);
            this.NewMenuItem.Text = "New";
            this.NewMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // showFilesCheckBox
            // 
            this.showFilesCheckBox.Location = new System.Drawing.Point(39, 3);
            this.showFilesCheckBox.Name = "showFilesCheckBox";
            this.showFilesCheckBox.Size = new System.Drawing.Size(126, 21);
            this.showFilesCheckBox.TabIndex = 2;
            this.showFilesCheckBox.Text = "Show only .sprd files";
            this.showFilesCheckBox.UseVisualStyleBackColor = true;
            this.showFilesCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // currentCellLabel
            // 
            this.currentCellLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentCellLabel.Location = new System.Drawing.Point(121, 23);
            this.currentCellLabel.Name = "currentCellLabel";
            this.currentCellLabel.Size = new System.Drawing.Size(90, 20);
            this.currentCellLabel.TabIndex = 3;
            this.currentCellLabel.Text = "Current cell:";
            this.currentCellLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(382, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(115, 20);
            this.textBox1.TabIndex = 4;
            // 
            // contentsOfCellLabel
            // 
            this.contentsOfCellLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contentsOfCellLabel.Location = new System.Drawing.Point(217, 0);
            this.contentsOfCellLabel.Name = "contentsOfCellLabel";
            this.contentsOfCellLabel.Size = new System.Drawing.Size(159, 44);
            this.contentsOfCellLabel.TabIndex = 5;
            this.contentsOfCellLabel.Text = "Cell Value: ";
            this.contentsOfCellLabel.UseCompatibleTextRendering = true;
            // 
            // changeCellContentsButton
            // 
            this.changeCellContentsButton.Location = new System.Drawing.Point(503, 23);
            this.changeCellContentsButton.Name = "changeCellContentsButton";
            this.changeCellContentsButton.Size = new System.Drawing.Size(193, 21);
            this.changeCellContentsButton.TabIndex = 6;
            this.changeCellContentsButton.Text = "Change Contents of Selected Cell";
            this.changeCellContentsButton.UseVisualStyleBackColor = true;
            this.changeCellContentsButton.Click += new System.EventHandler(this.changeCellContentsButton_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1487, 881);
            this.Controls.Add(this.changeCellContentsButton);
            this.Controls.Add(this.contentsOfCellLabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.currentCellLabel);
            this.Controls.Add(this.showFilesCheckBox);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Spreadsheet";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileButton;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem saveShowingAllFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveShowingsprdOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox showFilesCheckBox;
        private System.Windows.Forms.Label currentCellLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label contentsOfCellLabel;
        private System.Windows.Forms.Button changeCellContentsButton;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
    }
}

