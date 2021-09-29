
namespace TipCalculator
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
            this.totalBillLabel = new System.Windows.Forms.Label();
            this.PreTipAmount = new System.Windows.Forms.TextBox();
            this.PostTipAmount = new System.Windows.Forms.TextBox();
            this.ComputeBillButton = new System.Windows.Forms.Button();
            this.labelTipPercentage = new System.Windows.Forms.Label();
            this.textBoxTipPercentage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // totalBillLabel
            // 
            this.totalBillLabel.AutoSize = true;
            this.totalBillLabel.Location = new System.Drawing.Point(207, 94);
            this.totalBillLabel.Name = "totalBillLabel";
            this.totalBillLabel.Size = new System.Drawing.Size(75, 13);
            this.totalBillLabel.TabIndex = 0;
            this.totalBillLabel.Text = "Enter Total Bill";
            // 
            // PreTipAmount
            // 
            this.PreTipAmount.Location = new System.Drawing.Point(288, 91);
            this.PreTipAmount.Name = "PreTipAmount";
            this.PreTipAmount.Size = new System.Drawing.Size(100, 20);
            this.PreTipAmount.TabIndex = 1;
            this.PreTipAmount.TextChanged += new System.EventHandler(this.PreTipAmount_TextChanged);
            // 
            // PostTipAmount
            // 
            this.PostTipAmount.Location = new System.Drawing.Point(288, 157);
            this.PostTipAmount.Name = "PostTipAmount";
            this.PostTipAmount.Size = new System.Drawing.Size(100, 20);
            this.PostTipAmount.TabIndex = 2;
            this.PostTipAmount.TextChanged += new System.EventHandler(this.PostTipAmount_TextChanged);
            // 
            // ComputeBillButton
            // 
            this.ComputeBillButton.Location = new System.Drawing.Point(207, 155);
            this.ComputeBillButton.Name = "ComputeBillButton";
            this.ComputeBillButton.Size = new System.Drawing.Size(75, 23);
            this.ComputeBillButton.TabIndex = 3;
            this.ComputeBillButton.Text = "Compute  Tip";
            this.ComputeBillButton.UseVisualStyleBackColor = true;
            this.ComputeBillButton.Click += new System.EventHandler(this.computeBill_Click);
            // 
            // labelTipPercentage
            // 
            this.labelTipPercentage.AutoSize = true;
            this.labelTipPercentage.Location = new System.Drawing.Point(204, 127);
            this.labelTipPercentage.Name = "labelTipPercentage";
            this.labelTipPercentage.Size = new System.Drawing.Size(80, 13);
            this.labelTipPercentage.TabIndex = 4;
            this.labelTipPercentage.Text = "Tip Percentage";
            this.labelTipPercentage.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxTipPercentage
            // 
            this.textBoxTipPercentage.Location = new System.Drawing.Point(288, 127);
            this.textBoxTipPercentage.Name = "textBoxTipPercentage";
            this.textBoxTipPercentage.Size = new System.Drawing.Size(100, 20);
            this.textBoxTipPercentage.TabIndex = 5;
            this.textBoxTipPercentage.TextChanged += new System.EventHandler(this.textBoxTipPercentage_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxTipPercentage);
            this.Controls.Add(this.labelTipPercentage);
            this.Controls.Add(this.ComputeBillButton);
            this.Controls.Add(this.PostTipAmount);
            this.Controls.Add(this.PreTipAmount);
            this.Controls.Add(this.totalBillLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label totalBillLabel;
        private System.Windows.Forms.TextBox PreTipAmount;
        private System.Windows.Forms.TextBox PostTipAmount;
        private System.Windows.Forms.Button ComputeBillButton;
        private System.Windows.Forms.Label labelTipPercentage;
        private System.Windows.Forms.TextBox textBoxTipPercentage;
    }
}

