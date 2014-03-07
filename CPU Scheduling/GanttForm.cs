using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CPU_Scheduling
{
    public partial class GanttForm : Form
    {
        public GanttForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // lưu ảnh FCFS
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
                sfd.RestoreDirectory = false;    

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sfd.FileName;
                    Bitmap bmp = new Bitmap(FCFS.Width, FCFS.Height);
                    FCFS.DrawToBitmap(bmp, new Rectangle(0, 0, FCFS.Width, FCFS.Height));
                    FCFS.Image = bmp;
                    FCFS.Image.Save(filename, ImageFormat.Jpeg);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không lưu được. Hãy thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
       }
        // SJF
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
                sfd.RestoreDirectory = false;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sfd.FileName;
                    Bitmap bmp = new Bitmap(SJF.Width, SJF.Height);
                    SJF.DrawToBitmap(bmp, new Rectangle(0, 0, SJF.Width, SJF.Height));
                    SJF.Image = bmp;
                    SJF.Image.Save(filename, ImageFormat.Jpeg);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không lưu được. Hãy thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SRTF
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
                sfd.RestoreDirectory = false;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sfd.FileName;
                    Bitmap bmp = new Bitmap(SRTF.Width, SRTF.Height);
                    SRTF.DrawToBitmap(bmp, new Rectangle(0, 0, SRTF.Width, SRTF.Height));
                    SRTF.Image = bmp;
                    SRTF.Image.Save(filename, ImageFormat.Jpeg);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không lưu được. Hãy thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // RR
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
                sfd.RestoreDirectory = false;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sfd.FileName;
                    Bitmap bmp = new Bitmap(RR.Width, RR.Height);
                    RR.DrawToBitmap(bmp, new Rectangle(0, 0, RR.Width, RR.Height));
                    RR.Image = bmp;
                    RR.Image.Save(filename, ImageFormat.Jpeg);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không lưu được. Hãy thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
