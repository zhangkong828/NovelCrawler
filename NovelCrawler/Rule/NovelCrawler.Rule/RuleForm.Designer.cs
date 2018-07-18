namespace NovelCrawler.Rule
{
    partial class RuleForm
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
            this.comboxRules = new System.Windows.Forms.ComboBox();
            this.listBoxRule = new System.Windows.Forms.ListBox();
            this.btnTestRule = new System.Windows.Forms.Button();
            this.btnSaveRule = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtxtRuleDescription = new System.Windows.Forms.RichTextBox();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRulePattern = new System.Windows.Forms.TextBox();
            this.txtRuleFilter = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboxRules
            // 
            this.comboxRules.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboxRules.FormattingEnabled = true;
            this.comboxRules.ItemHeight = 19;
            this.comboxRules.Location = new System.Drawing.Point(18, 13);
            this.comboxRules.Name = "comboxRules";
            this.comboxRules.Size = new System.Drawing.Size(243, 27);
            this.comboxRules.TabIndex = 1;
            this.comboxRules.SelectedIndexChanged += new System.EventHandler(this.comboxRules_SelectedIndexChanged);
            // 
            // listBoxRule
            // 
            this.listBoxRule.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxRule.FormattingEnabled = true;
            this.listBoxRule.ItemHeight = 16;
            this.listBoxRule.Location = new System.Drawing.Point(18, 49);
            this.listBoxRule.Name = "listBoxRule";
            this.listBoxRule.Size = new System.Drawing.Size(243, 388);
            this.listBoxRule.TabIndex = 0;
            this.listBoxRule.SelectedIndexChanged += new System.EventHandler(this.listBoxRule_SelectedIndexChanged);
            // 
            // btnTestRule
            // 
            this.btnTestRule.Location = new System.Drawing.Point(283, 13);
            this.btnTestRule.Name = "btnTestRule";
            this.btnTestRule.Size = new System.Drawing.Size(75, 23);
            this.btnTestRule.TabIndex = 3;
            this.btnTestRule.Text = "测试规则";
            this.btnTestRule.UseVisualStyleBackColor = true;
            this.btnTestRule.Click += new System.EventHandler(this.btnTestRule_Click);
            // 
            // btnSaveRule
            // 
            this.btnSaveRule.Location = new System.Drawing.Point(646, 12);
            this.btnSaveRule.Name = "btnSaveRule";
            this.btnSaveRule.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRule.TabIndex = 4;
            this.btnSaveRule.Text = "保存规则";
            this.btnSaveRule.UseVisualStyleBackColor = true;
            this.btnSaveRule.Click += new System.EventHandler(this.btnSaveRule_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtxtRuleDescription);
            this.groupBox1.Controls.Add(this.txtRuleName);
            this.groupBox1.Location = new System.Drawing.Point(282, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 131);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "规则名称";
            // 
            // rtxtRuleDescription
            // 
            this.rtxtRuleDescription.Location = new System.Drawing.Point(9, 49);
            this.rtxtRuleDescription.Name = "rtxtRuleDescription";
            this.rtxtRuleDescription.ReadOnly = true;
            this.rtxtRuleDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtRuleDescription.Size = new System.Drawing.Size(424, 74);
            this.rtxtRuleDescription.TabIndex = 1;
            this.rtxtRuleDescription.Text = "";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(9, 21);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.ReadOnly = true;
            this.txtRuleName.Size = new System.Drawing.Size(218, 21);
            this.txtRuleName.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtRulePattern);
            this.groupBox2.Location = new System.Drawing.Point(282, 188);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 122);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集规则";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtRuleFilter);
            this.groupBox3.Location = new System.Drawing.Point(282, 316);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(439, 122);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "替换规则";
            // 
            // txtRulePattern
            // 
            this.txtRulePattern.Location = new System.Drawing.Point(9, 21);
            this.txtRulePattern.Multiline = true;
            this.txtRulePattern.Name = "txtRulePattern";
            this.txtRulePattern.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRulePattern.Size = new System.Drawing.Size(430, 95);
            this.txtRulePattern.TabIndex = 0;
            // 
            // txtRuleFilter
            // 
            this.txtRuleFilter.Location = new System.Drawing.Point(9, 21);
            this.txtRuleFilter.Multiline = true;
            this.txtRuleFilter.Name = "txtRuleFilter";
            this.txtRuleFilter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRuleFilter.Size = new System.Drawing.Size(430, 95);
            this.txtRuleFilter.TabIndex = 0;
            // 
            // RuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSaveRule);
            this.Controls.Add(this.btnTestRule);
            this.Controls.Add(this.listBoxRule);
            this.Controls.Add(this.comboxRules);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RuleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "采集规则";
            this.Load += new System.EventHandler(this.RuleForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboxRules;
        private System.Windows.Forms.ListBox listBoxRule;
        private System.Windows.Forms.Button btnTestRule;
        private System.Windows.Forms.Button btnSaveRule;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.RichTextBox rtxtRuleDescription;
        private System.Windows.Forms.TextBox txtRulePattern;
        private System.Windows.Forms.TextBox txtRuleFilter;
    }
}