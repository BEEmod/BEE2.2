namespace BEE2
{
    partial class OptionsForm
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
            this.snapMouseCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // snapMouseCheckBox
            // 
            this.snapMouseCheckBox.AutoSize = true;
            this.snapMouseCheckBox.Location = new System.Drawing.Point(12, 12);
            this.snapMouseCheckBox.Name = "snapMouseCheckBox";
            this.snapMouseCheckBox.Size = new System.Drawing.Size(149, 17);
            this.snapMouseCheckBox.TabIndex = 1;
            this.snapMouseCheckBox.Tag = "Automatically snap mouse to context menu strip click position when the menu strip" +
    " has to be moved inside the boarders of the screen";
            this.snapMouseCheckBox.Text = "Snap Mouse to click point";
            this.snapMouseCheckBox.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.snapMouseCheckBox);
            this.Name = "OptionsForm";
            this.Text = "OptionsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox snapMouseCheckBox;

    }
}