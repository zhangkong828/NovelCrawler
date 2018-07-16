namespace NovelCrawler.Rule
{
    partial class TestForm
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
            this.rtb_record = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtb_record
            // 
            this.rtb_record.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_record.Location = new System.Drawing.Point(0, 0);
            this.rtb_record.Name = "rtb_record";
            this.rtb_record.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_record.Size = new System.Drawing.Size(508, 484);
            this.rtb_record.TabIndex = 0;
            this.rtb_record.Text = "";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 484);
            this.Controls.Add(this.rtb_record);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TestForm";
            this.Text = "规则测试";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_record;
    }
}