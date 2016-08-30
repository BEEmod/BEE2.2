using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BEE2
{
    public partial class PromtForm : Form
    {
        public PromtForm(string label, string windowLabel, string cancelButtonText, string okayButtonText)
        {
            InitializeComponent();
            label1.Text = label;
            this.Text = windowLabel;
            cancelButton.Text = cancelButtonText;
            okayButton.Text = okayButtonText;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PromtForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;//stops from closing
            if (!justClicked)
                SafeData = false;
            else
                justClicked = false;
            textBox1.Text = "";
            this.Hide();
        }
        private bool justClicked = false;
        public bool SafeData = false;
        public string ReturnValue;
        private void okayButton_Click(object sender, EventArgs e)
        {
            SafeData = true;
            justClicked = true;
            ReturnValue = textBox1.Text;
            this.Hide();
        }
    }
}
