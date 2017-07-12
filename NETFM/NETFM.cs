/********************************************************************
 * * 使本项目源码前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能
 * * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
 * * 该源码提供了.NET FM 解析方法  Copyright © 2016 .NET FM • Lex Li • Rebornix
 * * 该源码只提供解析方法、以及Bass内核的使用方法
 * * 作者：Tom QQ：173796200 出品 Copyright (C) 2017-? Tom Corporation All rights reserved.
 * * 请保留以上版权信息，否则作者将保留追究法律责任。
 * * 创建时间：2017-03-14
 * 
 * * Please carefully read the following agreement before the source of the project, 
 * * If you agree to the following agreement to use all the features of the project
 * * Otherwise, if you violate the following agreements, there may be legal disputes and compensation, 
 * * The author reserves the right to pursue legal responsibility.
 * * The source provides .NET FM analytical methods  Copyright © 2016 .NET FM • Lex Li • Rebornix
 * * The source only provides analytical methods, as well as the use of Bass
 * * Author：Tom QQ：github.com/BlueSanction  Copyright (C) 2017-? Tom Corporation All rights reserved.
 * * Please retain the copyright information, otherwise the author will retain the legal responsibility.
 * * Create time：2017-03-14
********************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NETFM
{
    public partial class NETFM : TSkin.Main
    {
        #region 拖动窗口 Drag window
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        /// <summary>
        /// 调用Win32实现拖动窗口 Call Win32 to achieve drag window
        /// </summary>
        public void Maind(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF010 + 0x0002, 0);
        }
        #endregion
        public NETFM()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 全局的BASS内核 Global BASS
        /// </summary>
        public BassEngine Player = null;

        /// <summary>
        /// 静态的句柄 Static handle
        /// </summary>
        public static IntPtr hd;
        private void NETFM_Load(object sender, EventArgs e)
        {
            hd = Handle;//赋值窗口句柄 Assignment window handle
            this.Player = BassEngine.Instance;//初始化BASS Initialization BASS
            this.Player.Volume = 1000;//设置音量 Set Volume [1-1000]
            Player.TrackEnded += Player_TrackEnded;
            Player.OpenSucceeded += Player_OpenSucceeded;
            Player.OpenFailed += Player_OpenFailed;
            //http://dotnet.fm/
            //Test
            richTextBox1.Text = Api.GetHTML("http://dotnet.fm/").Replace("\t", " ").Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").Replace("&lt;", "").Replace("&gt;", "").Replace(" ", "").Replace("\"><divclass=\"post-", "<li><divclass=\"post-");
            Loaddata();
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 537://设备改动触发 Device change trigger
                    try
                    {
                        Player.ChangeDevice(Player.Device);
                    }
                    catch { }
                    break;
                    //default:break;
            }
            base.WndProc(ref m);
        }
        private void Player_TrackEnded(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        public void Play(string url)
        {
            Player.OpenFile(url);
        }

        void Player_OpenSucceeded(object sender, EventArgs e)
        {
            metroL.Stops(); timer1.Enabled = true;
            Player.Play();
        }
        private void Player_OpenFailed(object sender, string e)
        {
            metroL.Stops();
            MessageBox.Show(e + "播放出错 Play Err");
        }

        /// <summary>
        /// 加载GetData
        /// </summary>
        private void Loaddata()
        {
            try
            {
                string nei = richTextBox1.Text.Replace("\"><divclass=\"post-", "<li><divclass=\"post-");
                //<divclass="home">
                nei = nei.Substring(nei.IndexOf("<divclass=\"home\">") + 17);
                nei = nei.Substring(0, nei.LastIndexOf("</div></div></div>"));
                //richTextBox1.Text = nei;
                //richTextBox1.Visible = true;

                string[] Array = Regex.Split(nei, "<li><divclass=\"post-", RegexOptions.IgnoreCase);
                {
                    if (Array.Length > 1)
                    {
                        for (int i = 1; i < Array.Length; i++)
                        {
                            string neic = Array[i];
                            try
                            {
                                string day = neic.Substring(neic.IndexOf("day\">") + 5);
                                day = day.Substring(0, day.IndexOf("</div>"));
                                string month = neic.Substring(neic.IndexOf("month\">") + 7);
                                month = month.Substring(0, month.IndexOf("</div>"));
                                //MessageBox.Show(day+ " "+month);

                                //MessageBox.Show(neic);

                                string url = neic.Substring(neic.IndexOf("href=\"") + 6);
                                string name = url;
                                string musicurl = name;
                                int i1 = url.IndexOf("\">");
                                url = url.Substring(0, i1);
                                //MessageBox.Show(url);
                                name = name.Substring((i1 + 2));
                                int l2 = name.IndexOf("</a>");
                                name = name.Substring(0, l2);
                                //MessageBox.Show(musicurl);
                                musicurl = musicurl.Substring(musicurl.IndexOf("href=\"") + 6);
                                int i3 = musicurl.IndexOf("\">");
                                musicurl = musicurl.Substring(0, i3);

                                string demo = neic.Substring(neic.IndexOf("\"><p>") + 5);
                                demo = demo.Substring(0, demo.LastIndexOf("</p>"));

                                Items p = new Items();
                                p.Day = day;
                                p.Month = month;
                                p.Name = name;
                                p.Url = url;
                                p.MusicUrl = musicurl;
                                p.Demo = demo;


                                ListViewItem item = new ListViewItem(name);
                                item.Tag = p;
                                VListView.Items.Add(item);
                                // MessageBox.Show(name + "\n" + url + "\n" + musicurl + "\n" + demo);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }

        }

        public class Items
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string MusicUrl { get; set; }
            public string Demo { get; set; }
            public string Day { get; set; }
            public string Month { get; set; }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var schedule = (Player.ChannelPosition.TotalMilliseconds / Player.ChannelLength.TotalMilliseconds) * 100;
                string tia = Player.ChannelPosition.ToString();
                string tias = Player.ChannelLength.ToString();
                //this.lbTime.Text = tia.Substring(0, tia.LastIndexOf("."));
                label6.Text = tia.Substring(0, tia.LastIndexOf(".")) + "/" + tias.Substring(0, tias.LastIndexOf("."));
                if (!trackD.IsMouseDown)
                {
                    trackD.DM_Value = schedule;
                }
            }
            catch { }
        }

        private void trackD_ScrollValueChange(object sender, double e)
        {
            double timeall = Player.ChannelLength.TotalSeconds;
            double k = timeall * (Convert.ToDouble(trackD.DM_Value / 100.00));
            Player.PlayTo(k);
        }

        private void VListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            metroL.Start();
            ListViewItem it = VListView.SelectedItems[0];
            Items p = (Items)it.Tag;
            label3.Text = p.Day;
            label4.Text = p.Month;
            label2.Text = p.Name;
            label5.Text = p.Demo; Play(p.MusicUrl);
        }

        private void VListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { VListView_MouseDoubleClick(sender, null); }
        }

        private void NETFM_FormClosed(object sender, FormClosedEventArgs e)
        {
            Player.Stop(); Player.Dispose();
        }

        private void NETFM_SizeChanged(object sender, EventArgs e)
        {
            columnHeader1.Width = Width-22;
        }
    }
}
