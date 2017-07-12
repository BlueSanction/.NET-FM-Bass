namespace NETFM
{
    partial class NETFM
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NETFM));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.VListView = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.metroL = new TSkin.MetroLoading();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.trackD = new TSkin.Controls.VProgs();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(922, 66);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(78, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 66);
            this.label1.TabIndex = 1;
            this.label1.Text = ".NET FM";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NETFM.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(32, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // VListView
            // 
            this.VListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VListView.BackColor = System.Drawing.Color.White;
            this.VListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.VListView.Font = new System.Drawing.Font("微軟正黑體", 24F);
            this.VListView.FullRowSelect = true;
            this.VListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.VListView.Location = new System.Drawing.Point(0, 216);
            this.VListView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.VListView.MouseState = MaterialSkin.Controls.MaterialListView.MouseStates.OUT;
            this.VListView.Name = "VListView";
            this.VListView.OwnerDraw = true;
            this.VListView.Size = new System.Drawing.Size(922, 481);
            this.VListView.TabIndex = 1;
            this.VListView.UseCompatibleStateImageBehavior = false;
            this.VListView.View = System.Windows.Forms.View.Details;
            this.VListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VListView_KeyDown);
            this.VListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.VListView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 900;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(868, 730);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(10, 10);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoEllipsis = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 16F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(898, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "S02E05 软记文娱播报​";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 25F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(51, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 60);
            this.label3.TabIndex = 5;
            this.label3.Text = "12";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(26, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mar/2017";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoEllipsis = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(1)))), ((int)(((byte)(17)))));
            this.label5.Location = new System.Drawing.Point(133, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(763, 60);
            this.label5.TabIndex = 7;
            this.label5.Text = "“什么，你不知道Xamarin.Forms要支持WPF？什么，你不知道Visual Studio 2017就要发布了？什么，你不知道.NET刚过完15岁生日？”别" +
    "担心，试试我们的新节目“软记文娱播报​”，这些你都会轻松了解，并且可以开心的分享给自己的小伙伴们哦！本期值班主播吕鹏，正在加拿大蒙特利儿找住处的Lex童鞋有望在" +
    "下个月重新回到节目当中，敬请期待";
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // metroL
            // 
            this.metroL.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.metroL.Location = new System.Drawing.Point(0, 66);
            this.metroL.Name = "metroL";
            this.metroL.Size = new System.Drawing.Size(922, 5);
            this.metroL.TabIndex = 10;
            this.metroL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // trackD
            // 
            this.trackD.BackColor = System.Drawing.Color.Transparent;
            this.trackD.DM_BackColor = System.Drawing.Color.Silver;
            this.trackD.DM_BlockColor = System.Drawing.Color.DodgerBlue;
            this.trackD.DM_Value = 0D;
            this.trackD.Location = new System.Drawing.Point(136, 178);
            this.trackD.Name = "trackD";
            this.trackD.Size = new System.Drawing.Size(339, 25);
            this.trackD.TabIndex = 11;
            this.trackD.ScrollValueChange += new TSkin.Controls.VProgs.ScrollValueEventHandler(this.trackD_ScrollValueChange);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(495, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 18);
            this.label6.TabIndex = 12;
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            // 
            // NETFM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CanResize = true;
            this.ClientSize = new System.Drawing.Size(922, 700);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackD);
            this.Controls.Add(this.metroL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.VListView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("微軟正黑體", 10.5F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = global::NETFM.Properties.Resources.favicon;
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Name = "NETFM";
            this.Text = ".NET FM";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NETFM_FormClosed);
            this.Load += new System.EventHandler(this.NETFM_Load);
            this.SizeChanged += new System.EventHandler(this.NETFM_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Maind);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialListView VListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private TSkin.MetroLoading metroL;
        private System.Windows.Forms.Timer timer1;
        private TSkin.Controls.VProgs trackD;
        private System.Windows.Forms.Label label6;
    }
}

