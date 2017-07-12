namespace TSkin
{
    partial class MainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Location = new System.Drawing.Point(-1, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(15, 276);
            this.panel1.TabIndex = 0;
            this.panel1.Tag = "1";
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Location = new System.Drawing.Point(286, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 276);
            this.panel2.TabIndex = 1;
            this.panel2.Tag = "2";
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Location = new System.Drawing.Point(12, -1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(276, 15);
            this.panel3.TabIndex = 2;
            this.panel3.Tag = "3";
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Location = new System.Drawing.Point(12, 286);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(276, 15);
            this.panel4.TabIndex = 3;
            this.panel4.Tag = "6";
            this.panel4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(-1, -1);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(15, 15);
            this.panel5.TabIndex = 4;
            this.panel5.Tag = "4";
            this.panel5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Location = new System.Drawing.Point(286, 286);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(15, 15);
            this.panel6.TabIndex = 5;
            this.panel6.Tag = "8";
            this.panel6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Location = new System.Drawing.Point(286, -1);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(15, 15);
            this.panel7.TabIndex = 6;
            this.panel7.Tag = "5";
            this.panel7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel8.Location = new System.Drawing.Point(-1, 286);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(15, 15);
            this.panel8.TabIndex = 7;
            this.panel8.Tag = "7";
            this.panel8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Con);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion


        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.Panel panel7;
        public System.Windows.Forms.Panel panel8;

    }
}