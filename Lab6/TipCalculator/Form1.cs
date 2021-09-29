using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void computeBill_Click(object sender, EventArgs e)
        {
            string pre = PreTipAmount.Text;
            double bill = Double.Parse(pre);
            string percentAmt = textBoxTipPercentage.Text;
            double pAmt = Double.Parse(percentAmt);
            pAmt = pAmt / 100;
            bill *=  (1 + pAmt);
           

            PostTipAmount.Text = bill + "";
        }

        private void PostTipAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PreTipAmount_TextChanged(object sender, EventArgs e)
        {
            if(!double.TryParse(sender.ToString(), out double result))
            {
                ComputeBillButton.Enabled = false;
            }
            
                
            
        }

        private void textBoxTipPercentage_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(sender.ToString(), out double result))
            {
                ComputeBillButton.Enabled = false;
                
            }
            string pre = PreTipAmount.Text;
            double bill = Double.Parse(pre);
            string percentAmt = textBoxTipPercentage.Text;
            double pAmt = Double.Parse(percentAmt);
            pAmt = pAmt / 100;
            bill *= (1 + pAmt);


            PostTipAmount.Text = bill + "";
        }
    }
}
