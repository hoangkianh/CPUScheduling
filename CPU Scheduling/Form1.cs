/************************************************************************/
/*
CHƯƠNG TRÌNH LẬP LỊCH CPU
 * 
 * NGUYỄN HOÀNG ANH - CÔNG NGHỆ PHẦN MỀM K51 - ĐH GTVT
 * Hoàn thành tháng 2 / 2012.
          _____                    _____                    _____          
         /\    \                  /\    \                  /\    \         
        /::\____\                /::\____\                /::\    \        
       /:::/    /               /:::/    /               /::::\    \       
      /:::/    /               /:::/    /               /::::::\    \      
     /:::/    /               /:::/    /               /:::/\:::\    \     
    /:::/____/               /:::/____/               /:::/__\:::\    \    
   /::::\    \              /::::\    \              /::::\   \:::\    \   
  /::::::\    \   _____    /::::::\____\________    /::::::\   \:::\    \  
 /:::/\:::\    \ /\    \  /:::/\:::::::::::\    \  /:::/\:::\   \:::\    \ 
/:::/  \:::\    /::\____\/:::/  |:::::::::::\____\/:::/  \:::\   \:::\____\
\::/    \:::\  /:::/    /\::/   |::|~~~|~~~~~     \::/    \:::\  /:::/    /
 \/____/ \:::\/:::/    /  \/____|::|   |           \/____/ \:::\/:::/    / 
          \::::::/    /         |::|   |                    \::::::/    /  
           \::::/    /          |::|   |                     \::::/    /   
           /:::/    /           |::|   |                     /:::/    /    
          /:::/    /            |::|   |                    /:::/    /     
         /:::/    /             |::|   |                   /:::/    /      
        /:::/    /              \::|   |                  /:::/    /       
        \::/    /                \:|   |                  \::/    /        
         \/____/                  \|___|                   \/____/         
																		   
																		   
																	 
/************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CPU_Scheduling
{
    /// <summary>
    /// Main Form
    /// </summary>
	public partial class MainForm : Form
	{
		private LinkedList list = null;
		private int NumberOfProcess = 0;    //  Số tiến trình
        public GanttForm gf;		
		
		public MainForm()
		{
			InitializeComponent();
			this.list = new LinkedList();
			this.components = null;
		}

		private void pushProcesses()
		{
			this.NumberOfProcess = this.dtgvInput.RowCount - 1;
			this.list = new LinkedList();

			try
			{
				for (int i = 0; i < this.NumberOfProcess; i++)
				{
					Node newNode = new Node
					{
						Name = this.dtgvInput.Rows[i].Cells[0].Value.ToString(),
						ArrivalTime = int.Parse(this.dtgvInput.Rows[i].Cells[1].Value.ToString()),
						CPUBurstTime = int.Parse(this.dtgvInput.Rows[i].Cells[2].Value.ToString())
					};

					this.list.addNodeAtTheEnd(newNode);
				}

				this.list.quickSort();
				this.list.Numbering();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("Không thể tính toán được.\n\rHãy nhập dữ liệu trước", "Lỗi",
								 MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
#region Các chức năng

        // Tính toán
		private void button1_Click(object sender, EventArgs e)
		{
           try
           {
               this.pushProcesses();
               new Algorithm(this.list).FCFS(this);
               new Algorithm(this.list).SJF(this);
               new Algorithm(this.list).SRTF(this);
               new Algorithm(this.list).RoundRobin(this);
           }
           catch (System.Exception ex)
           {
           }
		}
		// Biểu đồ Gantt
		private void button2_Click(object sender, EventArgs e)
		{
			gf = new GanttForm();
			gf.FCFS.Image = pictureBox1.Image;
            gf.SJF.Image = pictureBox2.Image;
            gf.SRTF.Image = pictureBox3.Image;
            gf.RR.Image = pictureBox4.Image;
			gf.ShowDialog();
		}
		// xóa dữ liệu
		private void button3_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("Bạn muốn xóa dữ liệu vừa tính toán ?", "Chú ý",
								MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
			{
				dtgvInput.Rows.Clear();
				dtgvFCFS.Rows.Clear();
                dtgvSJF.Rows.Clear();
                dtgvSRTF.Rows.Clear();
                dtgvRR.Rows.Clear();
				this.NumberOfProcess = 0;
				this.list = null;
                this.FCFSTbx.Text = "";
                this.SJFTbx.Text = "";
                this.SRTFTbx.Text = "";
                this.RRTbx.Text = "";
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
                pictureBox4.Image = null;

				MessageBox.Show("Xóa dữ liệu thành công. \r\nHãy nhập vào dữ liệu mới", "Xóa thành công",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		// thoát
		private void button4_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		// ghi file kết quả
		private void BtnGhiFile_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Text file (*.txt) | *.txt";
			sfd.RestoreDirectory = true;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.Unicode);
				string Line = "     ";

                //  In đầu vào
				sw.WriteLine("----------LẬP LỊCH CPU----------\n");
                sw.WriteLine("*_____ĐẦU VÀO_____*");
				sw.WriteLine("Tên tiến trình    Thời gian chờ (ms)  Thời gian chạy (ms)");
				for (int i = 0; i < dtgvInput.Rows.Count - 1; i++)
				{
					if (i == dtgvInput.NewRowIndex)
					{
						break;
					}
					Line = "    ";
					for (int j = 0; j < dtgvInput.Columns.Count; j++ )
					{
						Line += dtgvInput[j, i].Value + "               ";
					}
					sw.WriteLine(Line);
				}
				sw.WriteLine("***************************************************");
                // In đầu ra với 4 thuật toán
				sw.WriteLine("***************************************************");
                sw.WriteLine("*_____KẾT QUẢ_____*");
                // FCFS
                sw.WriteLine("***************************************************");
                sw.WriteLine("THUẬT TOÁN FCFS");
				sw.WriteLine("Tên tiến trình    Khoảng thời gian (ms)");
				for (int i = 0; i < dtgvFCFS.Rows.Count - 1; i++ )
				{
					if (i == dtgvFCFS.NewRowIndex)
					{
						break;
					}
					Line = "    ";
					for (int j = 0; j < dtgvFCFS.Columns.Count; j++)
					{
						Line += dtgvFCFS[j, i].Value + "              ";
					}
					sw.WriteLine(Line);
				}
				sw.WriteLine("Thời gian chờ trung bình: " + FCFSTbx.Text + "(ms)");
                // SJF
                sw.WriteLine("***************************************************");
                sw.WriteLine("THUẬT TOÁN SJF - Không ưu tiên");
                sw.WriteLine("Tên tiến trình    Khoảng thời gian (ms)");
                for (int i = 0; i < dtgvSJF.Rows.Count - 1; i++)
                {
                    if (i == dtgvSJF.NewRowIndex)
                    {
                        break;
                    }
                    Line = "    ";
                    for (int j = 0; j < dtgvSJF.Columns.Count; j++)
                    {
                        Line += dtgvSJF[j, i].Value + "              ";
                    }
                    sw.WriteLine(Line);
                }
                sw.WriteLine("Thời gian chờ trung bình: " + SJFTbx.Text + "(ms)");
                // SRTF
                sw.WriteLine("***************************************************");
                sw.WriteLine("THUẬT TOÁN SJF (SRTF) - Có ưu tiên");
                sw.WriteLine("Tên tiến trình    Khoảng thời gian (ms)");
                for (int i = 0; i < dtgvSRTF.Rows.Count - 1; i++)
                {
                    if (i == dtgvSRTF.NewRowIndex)
                    {
                        break;
                    }
                    Line = "    ";
                    for (int j = 0; j < dtgvSRTF.Columns.Count; j++)
                    {
                        Line += dtgvSRTF[j, i].Value + "              ";
                    }
                    sw.WriteLine(Line);
                }
                sw.WriteLine("Thời gian chờ trung bình: " + SRTFTbx.Text + "(ms)");
                // RR
                sw.WriteLine("***************************************************");
                sw.WriteLine("THUẬT TOÁN RR");
                sw.WriteLine("Tên tiến trình    Khoảng thời gian (ms)");
                for (int i = 0; i < dtgvRR.Rows.Count - 1; i++)
                {
                    if (i == dtgvRR.NewRowIndex)
                    {
                        break;
                    }
                    Line = "    ";
                    for (int j = 0; j < dtgvRR.Columns.Count; j++)
                    {
                        Line += dtgvRR[j, i].Value + "              ";
                    }
                    sw.WriteLine(Line);
                }
                sw.WriteLine("Thời gian chờ trung bình: " + RRTbx.Text + "(ms)");
				sw.Close();
			}
		}
		// mở file 
		private void BtnMoFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog opd = new OpenFileDialog();
			opd.Filter = "Input file (*.inp) | *.inp";
			opd.RestoreDirectory = true;
			if (opd.ShowDialog() == DialogResult.OK)
			{
				StreamReader sr = new StreamReader(opd.FileName);
				string Line = sr.ReadLine();
				while (Line != null && Line != " ")
				{
					string[] array = Line.Split(';');  // xóa những dấu ; sau đó nối thành xâu
					dtgvInput.Rows.Add(array);
					Line = sr.ReadLine();                    
				}
				sr.Close();
			}
		}
        // ghi file dữ liệu đầu vào
		private void BtnLuuMau_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Input file (*.inp) | *.inp";
			sfd.RestoreDirectory = false;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				StreamWriter sw = new StreamWriter(sfd.FileName);
				string Line = "";

				for (int i = 0; i < dtgvInput.Rows.Count - 1; i++)
				{
					if (i == dtgvInput.NewRowIndex)
					{
						break;
					}
					Line = " ";
					for (int j = 0; j < dtgvInput.Columns.Count; j++)
					{
						Line += dtgvInput[j, i].Value + ";";    // ngăn cách mỗi column bằng ;
					}
					sw.WriteLine(Line); // hết 1 row thì viết tiếp dòng mới
				}
				sw.Close();
			}
		}
        /// <summary>
        /// Tool Strip Menu
        /// </summary>
		private void nhậpTừTệpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BtnMoFile_Click(sender, e);
		}

		private void lưuDữLiệuVàoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BtnLuuMau_Click(sender, e);
		}

		private void lưuKếtQuảToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BtnGhiFile_Click(sender, e);
		}

		private void biểuĐồGanttToolStripMenuItem_Click(object sender, EventArgs e)
		{
			button2_Click(sender, e);
		}

		private void xóaDữLiệuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			button3_Click(sender, e);
		}

		private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void tácGiảToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutmeFrm abm = new AboutmeFrm();
			abm.ShowDialog();
		}

		private void tàiLiệuThamKhảoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TaiLieuThamkhaoFrm tltk = new TaiLieuThamkhaoFrm();
			tltk.ShowDialog();
		}

		private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://fituct.org");
		}
	}

#endregion

    /// <summary>
    /// Class Node [1 Node là 1 tiến trình]
	/// </summary>
	internal class Node
	{
        private int arrivalTime;    // thời gian chờ
        private int cpuBurstTime;   // thời gian chạy
        private string name;        // tên tiến trình
		private Node next;
        private int number;         // số thứ tự tiến trình

        /*Các phương thức*/
		public int ArrivalTime
		{
			get { return this.arrivalTime; }
			set { this.arrivalTime = value; }
		}

		public int CPUBurstTime
		{
			get { return this.cpuBurstTime; }
			set { this.cpuBurstTime = value; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public Node Next
		{
			get { return this.next; }
			set { this.next = value; }
		}

		public int Number
		{
			get { return this.number; }
			set { this.number = value; }
		}

		/*Hàm tạo*/
		public Node() { }
  
		public Node(string nodeName, int arrival, int burstTime)
		{
			this.Name = nodeName;
			this.ArrivalTime = arrival;
			this.CPUBurstTime = burstTime;
			this.Number = 0;
			this.Next = null;
		}
        /*Sao chép*/
		public Node Clone()
		{
			return new Node
			{
				ArrivalTime  = this.ArrivalTime,
				CPUBurstTime = this.CPUBurstTime,
				Name         = this.Name,
				Number       = this.Number,
				Next         = null,
			};
		}
	}

	/// <summary>
    /// Danh sách liên kết [thật ra là danh sách liên kết vòng]
    /// <see cref="Quản lý các tiến trình"/>
	/// </summary>
	internal class LinkedList
	{
		private Node head;
		private Node tail;

        /*Các phương thức*/
		public Node Head
		{
			get { return this.head; }
			set { this.head = value; }
		}

		public Node Tail
		{
			get { return this.tail; }
			set { this.tail = value; }
		}

		public LinkedList()
		{
			Node node;
			this.Tail = (Node)(node = null);
			this.Head = node;
		}

		// Chèn node vào đầu
		public bool addNodeAtTheBegin(Node newNode)
		{
			if (this.Head == null)
			{
				this.Head = this.Tail = newNode;
				this.Tail.Next = this.Head;
			} 
			else
			{
				newNode.Next = this.Head;
				this.Tail.Next = newNode;
				this.Head = newNode;
			}

			return true;
			
		}

		// Chèn cuối
		public bool addNodeAtTheEnd(Node newNode)
		{
			if (this.Head == null)
			{
				this.Head = this.Tail = newNode;
				this.Tail.Next = this.Head;
			} 
			else
			{
				newNode.Next = this.Head;
				this.Tail.Next = newNode;
				this.Tail = newNode;
			}
			return true;
		}

		// Kiểm tra rỗng
		public bool IsEmpty()
		{
			return (this.Head == null);
		}

        // Lấy ra 1 Node
		public Node GetNodeP(ref Node p)
		{
			if (this.IsEmpty())
			{
				MessageBox.Show("Danh sách rỗng.", "Lỗi",
								 MessageBoxButtons.OK, MessageBoxIcon.Error);

				return p;
			}

            if ((this.Head == p) && (this.Tail == p))
            {
                Node node;
                this.Tail = (Node)(node = null);
                this.Head = node;
            }
            else
            {
                if (p == this.Head)
                {
                    this.Head = this.Head.Next;
                    this.Tail.Next = this.Head;
                }
                else
                {
                    Node head = this.Head;
                    while (head.Next != p)
                    {
                        head = head.Next;
                    }

                    head.Next = p.Next;
                    if (p == this.Tail)
                    {
                        this.Tail = head;
                    }
                }
            }
			p.Next = null;
			return p;
		}

		// Độ dài DSLK
		public int Length()
		{
			Node head = this.Head;
			int num = 0;
			if (head == null)
			{
				return 0;
			}

			while (head != this.Tail)
			{
				num++;
				head = head.Next;
			}

			num++;
			return num;
		}

        // Số lượng tiến trình
		public void Numbering()
		{
			int num = 0;
			Node head = this.Head;
			while (head != this.Tail)
			{
				head.Number = num;
				head = head.Next;
				num++;
			}
			head.Number = num;
		}

		// Sắp xếp
		public void quickSort()
		{
			if (this.Head != this.Tail)
			{
				Node node;
				LinkedList list = new LinkedList();
				LinkedList list2 = new LinkedList();
				node = this.Head;
				Node p = new Node();

				this.GetNodeP(ref node);

				while (this.Head != null)
				{
					p = this.Head;
					this.GetNodeP(ref p);
					if (p.ArrivalTime < node.ArrivalTime)
					{
						list.addNodeAtTheEnd(p);
					}
					else
					{
						list2.addNodeAtTheEnd(p);
					}
				}

				list.quickSort();
				list2.quickSort();

				if (list.Head == null)
				{
					this.Head = node;
				}
				else
				{
					this.Head = list.Head;
					list.Tail.Next = node;
					node.Next = list2.Head;
				}
				if (list2.Head == null)
				{
					this.Tail = node;
				}
				else
				{
					this.Tail = list2.Tail;
					node.Next = list2.Head;
				}
				this.Tail.Next = this.Head;
			}
		}

        // Xem DSLK
		public void visitLinkedList(ref DataGridView dtgv)
		{
			for (int i = 1; i < dtgv.RowCount; i++)
			{
				dtgv.Rows.Clear();
			}
			Node head = new Node();
			head = this.Head;
			int num2 = 0;
			while (head != this.Tail)
			{
				dtgv.Rows.Add();
				dtgv.Rows[num2].Cells[0].Value = head.ArrivalTime.ToString();
				dtgv.Rows[num2].Cells[1].Value = head.Number.ToString();
				num2++;
				head = head.Next;
			}
			dtgv.Rows.Add();
			dtgv.Rows[num2].Cells[0].Value = head.ArrivalTime.ToString();
			dtgv.Rows[num2].Cells[1].Value = head.Number.ToString();
		}
	}
	
	/// <summary>
    /// Vẽ biểu đồ Gantt và tính toán kết quả để show lên DataGridView output
	///</summary>
	internal class Gantt
	{
		private int bottom;
		private float coeffician;
		private int top;

        /*Các phương thức*/
		public int Bottom
		{
			get { return this.bottom; }
			set { this.bottom = value; }
		}

		public float Coefficient
		{
			get { return this.coeffician; }
			set { this.coeffician = value; }
		}

		public int Top
		{
			get { return this.top; }
			set { this.top = value; }
		}

		public Gantt()
		{
            this.Coefficient = 60;  // hệ số 
			this.Top = 30;  // hàng trên 
			this.Bottom = this.Top + 40;  // hàng dưới
		}

        /* Vẽ biểu đồ Gantt, ghi kết quả lên DataGridView Output và Textbox */
        public void DrawGantt(LinkedList processesList,     // danh sách các tiến trình đã hoàn thành
                              int[] timeLine,               // 
                              LinkedList arrangedProcesses, // danh sách các tiến trình đã được sắp xếp
                              PictureBox pic,               // picture box để show ảnh
                              DataGridView dtgv,            // Datagridview Output
                              TextBox tbx                   // textbox thời gian chờ trung bình
                             )
		{
			for (int i = 1; i < dtgv.RowCount; i++)
			{
				dtgv.Rows.Clear();  // làm sạch DataGridView dtgvOuput
			}

            int index = arrangedProcesses.Length(); // số tiến trình đã được sắp xếp
            int p_num = processesList.Length();      // số lượng tiến trình đầu vào
            int[] numArray = new int[p_num];        // thời gian chờ của các tiến trình
			int width = ((int)(timeLine[index] * this.Coefficient)) + 30;
			int height = pic.Height;
			// tạo ảnh bitmap
			Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
			// tạo font chữ
			Font font = new Font("Tahoma", 13, FontStyle.Regular, GraphicsUnit.Point, 0);
			Graphics graphics = Graphics.FromImage(image);

			// vẽ các viền trên và dưới
			graphics.DrawLine(new Pen(Brushes.Black, 1.5f), new Point(0, this.Top), new Point(width, this.Top));
			graphics.DrawLine(new Pen(Brushes.Black, 1.5f), new Point(0, this.Bottom), new Point(width, this.Bottom));
			graphics.DrawLine(new Pen(Brushes.Black, 1.5f), new Point(0, this.Top), new Point(0, this.Bottom));

			// vẽ trục thời gian
			for (int j = 0; j <= index; j++)
			{
				graphics.DrawLine(new Pen(Brushes.Black, 1.5f), new Point((int)(this.Coefficient * timeLine[j]), this.Top), 
								  new Point((int)(this.Coefficient * timeLine[j]), this.Bottom));
				// ghi các mốc thời gian
				graphics.DrawString(timeLine[j].ToString(), font, Brushes.Black, 
									(PointF)new Point((int)((this.Coefficient * timeLine[j]) - 3), this.Bottom + 2));
			}
			
            Node head = new Node();
			head = processesList.Head;	   
			Node next = new Node();
			next = arrangedProcesses.Head;
            int row_num = 0;                   // số hàng datagridview
            Point point = new Point();      // điểm nhằm ghi tên các tiến trình

            while (next != arrangedProcesses.Tail)  // chừng nào danh sách đã sắp xếp chưa hết
			{
				point = new Point((int)(this.Coefficient * (timeLine[row_num] + ((timeLine[row_num + 1] - timeLine[row_num]) / 4))), 
								   this.Top + ((-this.Top + this.Bottom) / 4));
                // ghi tên các tiến trình lên ảnh
				graphics.DrawString(next.Name.ToString(), font, Brushes.Red, (PointF)point);
				// ghi kết quả lên datagridviewOutput
				dtgv.Rows.Add();
				dtgv.Rows[row_num].Cells[0].Value = next.Name.ToString();
				dtgv.Rows[row_num].Cells[1].Value = timeLine[row_num].ToString() + " -> " + timeLine[row_num + 1].ToString();
				next = next.Next;
				row_num++;
			}

			// tiến trình cuối cùng
			point = new Point((int)(this.Coefficient * (timeLine[row_num] + ((timeLine[row_num + 1] - timeLine[row_num]) / 4))),
								   this.Top + ((-this.Top + this.Bottom) / 4));
			graphics.DrawString(next.Name.ToString(), font, Brushes.Red, (PointF)point);
			
            /* ghi kết quả lên datagridviewOutput */
			dtgv.Rows.Add();
			dtgv.Rows[row_num].Cells[0].Value = next.Name.ToString();
			dtgv.Rows[row_num].Cells[1].Value = timeLine[row_num].ToString() + " -> " + timeLine[row_num + 1].ToString();

            int arrivalTime = 0;        // thời gian tới
            int arv_num = 0;            // thời gian tới
            int total_num = 0;          // tổng thời gian chờ
			string str = null;
			
			for (int k = 0; k < p_num; k++)
			{
				row_num = 0;
				Node node1 = processesList.Head;
				while (row_num != p_num)
				{
					if (node1.Number == k)
					{
						arrivalTime = node1.ArrivalTime;
						arv_num = arrivalTime;
						break;
					}
					node1 = node1.Next;
					row_num++;
				}

                // ghi lên text box thời gian chờ trung bình
				row_num = 0;
				numArray[k] = 0;
				Node node2 = arrangedProcesses.Head;

				while (node2 != arrangedProcesses.Tail)
				{
					if (node2.Number == k)
					{
						arv_num = timeLine[row_num];
						numArray[k] += arv_num - arrivalTime;
						arrivalTime = timeLine[row_num + 1];
					}
					node2 = node2.Next;
					row_num++;
				}
				if (node2.Number == k)
				{
					arv_num = timeLine[row_num];
					numArray[k] += arv_num - arrivalTime;
				}
				str = str + numArray[k].ToString() + " + ";
				total_num += numArray[k];
			}

			// ghi kết quả lên textbox kết quả (thời gian chờ trung bình)

			str = str.Remove(str.Length - 3);
			string str2 = "(" + str;
			string[] strArray = new string[] { str2, ")/", p_num.ToString(), " = ", (((double)total_num) / ((double)p_num)).ToString() };
			str = string.Concat(strArray);
			pic.Width = width + 10;
			pic.Image = image;
			tbx.Text = str;
		}
	}

	/// <summary>
    /// Các thuật toán
	/// </summary>
	internal class Algorithm
    {
 #region Các biến

        private LinkedList Process_List;        //  Danh sách các tiến trình đầu vào   
        private LinkedList ready;               //  Danh sách các tiến trình đã sẵn sàng
        private LinkedList arrangedProcesses;   //  tiến trình đã được sắp xếp
        private Gantt gantt;                    //  biểu đồ Gantt
        private int NumberOfProcess;            //  số lượng tiến trình
        private int ProcessingTime;             //  thời gian chạy
        private int[] RemainingTime;            //  thời gian còn lại
        private int[] TimeLine;                 //  thời gian chờ
        private int TotalTime;                  //  tổng thời gian để chạy hết các tiến trình
        private bool[] Processed;               //  tiến trình đã chạy xong = true, chưa xong = false
        public GanttForm gf;
 #endregion
        /* Hàm tạo */
		public Algorithm(LinkedList processesList)
		{
			this.NumberOfProcess = processesList.Length();
			this.Processed = new bool[this.NumberOfProcess];

			for (int i = 0; i <= this.NumberOfProcess - 1; i++)
			{
                this.Processed[i] = false;      // tất cả các tiến trình chưa chạy
			}

			this.RemainingTime = new int[this.NumberOfProcess];
			Node head = processesList.Head;

			while (head != processesList.Tail)
			{
				this.RemainingTime[head.Number] = head.CPUBurstTime;
				head = head.Next;
			}
			this.RemainingTime[head.Number] = head.CPUBurstTime;
			this.ready = new LinkedList();
			this.arrangedProcesses = new LinkedList();
			this.Process_List = processesList;
			this.TimeLine = new int[300];
			this.ProcessingTime = 0;
			this.TotalTime = 0;
			this.gantt = new Gantt();
		}

        /*Thuật toán FCFS*/
		public void FCFS(MainForm mainfrm)
		{
			int index = -1;             
			Node head = new Node();
			Node newNode = new Node();

			for (int i = 0; i < this.NumberOfProcess; i++)
			{
                if (!this.Processed[i])             // nếu 1 tiến trình i chưa chạy xong
				{
                    // kiểm tra CPU rỗi không
                    if (i != 0)
                    {
                        Node noProcess = new Node("CPU rỗi", -1, 0);
                        this.arrangedProcesses.addNodeAtTheEnd(noProcess);
                    }
                    bool flag = true;       // biến kiểm tra 1 tiến trình đã được sắp xếp chưa

					for (int j = 0; j < this.NumberOfProcess; j++)
					{
                        if (!this.Processed[j])         //  nếu 1 tiến trình j chưa chạy xong
						{
							head = this.Process_List.Head;
                            while (head.Number != j)    // tìm tiến trình thứ j trong danh sách đầu vào
							{
								head = head.Next;
							}

                            if (flag)       // nếu đã được sắp xếp thì thực hiện tính toán
							{
								this.TotalTime = head.ArrivalTime;
								index++;
                                this.TimeLine[index] = this.TotalTime; // cộng vào tổng thời gian
                                flag = false;       // set lại bằng false
							}

                            if (head.ArrivalTime == this.TotalTime) 
							{
								Node node_1 = head.Clone();
                                this.ready.addNodeAtTheEnd(node_1);  // chuyển vào danh sách các tiến trình đã sẵn sàng
                                this.Processed[j] = true;           // đã chạy xong
							}
						}
					}

                    while (!this.ready.IsEmpty())       // chừng nào danh sách sẵn sàng vẫn còn
					{
						Node next = this.ready.Head;
						Node p = new Node();
						
                        newNode = next.Clone();
						this.arrangedProcesses.addNodeAtTheEnd(newNode);
						p = next;
						this.ProcessingTime = next.CPUBurstTime;
						this.TotalTime += this.ProcessingTime;
						index++;
						this.TimeLine[index] = this.TotalTime;
						next = next.Next;
                        this.ready.GetNodeP(ref p);         // lấy Node p ở danh sách sẵn sàng
						Node node_2 = this.Process_List.Head;

						for (int k = 0; k < this.NumberOfProcess; k++)
						{
							if (!((node_2.ArrivalTime > this.TotalTime) || this.Processed[node_2.Number]))
							{
								Node node_3 = new Node();
								node_3 = node_2.Clone();
                                this.ready.addNodeAtTheEnd(node_3);     // add vào cuối danh sách sẵn sàng (quay vòng lại)
                                this.Processed[node_3.Number] = true;   // đã chạy xong
							}
							node_2 = node_2.Next;
						}
					}
				}
			}

            // vẽ biểu đồ Gantt
			this.gantt = new Gantt();
			this.gantt.DrawGantt(this.Process_List, this.TimeLine, this.arrangedProcesses, 
                                 mainfrm.pictureBox1, mainfrm.dtgvFCFS, mainfrm.FCFSTbx);
		}

        /*Thuật toán SJF*/
		public void SJF(MainForm mainfrm)
		{
			int index = -1;
			Node head = new Node();
			Node newNode = new Node();

			for (int i = 0; i < this.NumberOfProcess; i++)
			{
				if (!this.Processed[i])
				{
                    // kiểm tra CPU rỗi không
                    if (i != 0)
                    {
                        Node noProcess = new Node("CPU rỗi", -1, 0);
                        this.arrangedProcesses.addNodeAtTheEnd(noProcess);
                    }
					bool flag = true;

                    // tương tự như FCFS
					for (int j = 0; j < this.NumberOfProcess; j++)
					{
						if (!this.Processed[j])
						{
							head = this.Process_List.Head;
							while (head.Number != j)
							{
								head = head.Next;
							}
							if (flag)
							{
								this.TotalTime = head.ArrivalTime;
								index++;
								this.TimeLine[index] = this.TotalTime;
								flag = false;
							}
							if (head.ArrivalTime == this.TotalTime)
							{
								Node node_1 = head.Clone();
								this.ready.addNodeAtTheEnd(node_1);
								this.Processed[j] = true;
							}
						}
					}

                    while (!this.ready.IsEmpty())       // chừng nào DS SS chưa hết
					{
						Node next = this.ready.Head;
						Node p = next;
						int cpuBurstTime = p.CPUBurstTime;

                        // tìm kiếm & sắp xếp các tiến trình 
						while (next != this.ready.Tail)
						{
							if (next.CPUBurstTime < cpuBurstTime)
							{
								cpuBurstTime = next.CPUBurstTime;
								p = next;
							}
							next = next.Next;
						}
						if (next.CPUBurstTime < cpuBurstTime)
						{
							cpuBurstTime = next.CPUBurstTime;
							p = next;
						}

                        // tương tự FCFS
						this.ProcessingTime = cpuBurstTime;
						this.TotalTime += this.ProcessingTime;
						index++;
						this.TimeLine[index] = this.TotalTime;
						newNode = p.Clone();
						this.arrangedProcesses.addNodeAtTheEnd(newNode);
						this.ready.GetNodeP(ref p);
						Node node_2 = this.Process_List.Head;
						
                        for (int k = 0; k < this.NumberOfProcess; k++)
						{
							if (!((node_2.ArrivalTime > this.TotalTime) || this.Processed[node_2.Number]))
							{
								Node node_3 = new Node();
								node_3 = node_2.Clone();
								this.ready.addNodeAtTheEnd(node_3);
								this.Processed[node_3.Number] = true;
							}
							node_2 = node_2.Next;
						}
					}
				}
			}
			this.gantt = new Gantt();
			this.gantt.DrawGantt(this.Process_List, this.TimeLine, this.arrangedProcesses, 
                                 mainfrm.pictureBox2,mainfrm.dtgvSJF, mainfrm.SJFTbx);
		}

        /*Thuật toán SJF có ưu tiên*/
        public void SRTF(MainForm mainfrm)
        {
            int index = -1;
            Node head = new Node();
            Node newNode = new Node();

            for (int i = 0; i < this.NumberOfProcess; i++)
            {
                if (!this.Processed[i])
                {
					// kiểm tra CPU rỗi không
					if (i != 0)
					{
						Node noProcess = new Node("CPU rỗi", -1, 0);
						this.arrangedProcesses.addNodeAtTheEnd(noProcess);
					}

                    bool flag = true;
                    // tương tự như 2 thuật toán FCFS & SJF
                    for (int j = 0; j < this.NumberOfProcess; j++)
                    {
                        if (!this.Processed[j])
                        {
                            head = this.Process_List.Head;
                            while (head.Number != j)
                            {
                                head = head.Next;
                            }
                            if (flag)
                            {
                                this.TotalTime = head.ArrivalTime;
                                index++;
                                this.TimeLine[index] = this.TotalTime;
                                flag = false;
                            }
                            if (head.ArrivalTime == this.TotalTime)
                            {
                                Node node_1 = head.Clone();
                                this.ready.addNodeAtTheEnd(node_1);
                                this.Processed[j] = true;
                            }
                        }
                    }

                    while (!this.ready.IsEmpty())   // chừng nào DS SS chưa hết
                    {
                        Node next = this.ready.Head;
                        Node p = next;
                        int remaining = this.RemainingTime[p.Number];     

                        while (next != this.ready.Tail)
                        {
                            if (this.RemainingTime[next.Number] < remaining)    
                            {
                                remaining = this.RemainingTime[next.Number];
                                p = next;
                            }
                            next = next.Next;
                        }
                        if (this.RemainingTime[next.Number] < remaining)
                        {
                            remaining = this.RemainingTime[next.Number];
                            p = next;
                        }

                        int total = this.TotalTime + remaining;
                        Node node_2 = this.Process_List.Head;
                        bool flag2 = false;             // kiểm tra đã được sắp xếp chưa?    

                        for (int k = 0; k < this.NumberOfProcess; k++)
                        {
                            if (!((node_2.ArrivalTime > total) || this.Processed[node_2.Number]))
                            {
                                Node node_3 = new Node();
                                node_3 = node_2.Clone();
                                this.ready.addNodeAtTheEnd(node_3);
                                this.Processed[node_3.Number] = true;
                                this.ProcessingTime = node_2.ArrivalTime - this.TotalTime;
                                this.TotalTime += this.ProcessingTime;
                                index++;
                                this.TimeLine[index] = this.TotalTime;
                                this.RemainingTime[p.Number] -= this.ProcessingTime;
                                newNode = p.Clone();
                                this.arrangedProcesses.addNodeAtTheEnd(newNode);    // copy vào danh sách các tiến trình đã sắp xếp
                                flag2 = true;
                                break;
                            }
                            node_2 = node_2.Next;
                        }

                        if (!flag2) // nếu chưa được sắp xếp thì sắp xếp
                        {
                            this.ProcessingTime = this.RemainingTime[p.Number];
                            this.RemainingTime[p.Number] = 0;
                            this.TotalTime += this.ProcessingTime;
                            index++;
                            this.TimeLine[index] = this.TotalTime;
                            newNode = p.Clone();
                            this.arrangedProcesses.addNodeAtTheEnd(newNode);
                            this.ready.GetNodeP(ref p);
                        }
                    }
                }
            }
            this.gantt = new Gantt();
            this.gantt.DrawGantt(this.Process_List, this.TimeLine, this.arrangedProcesses, 
                                 mainfrm.pictureBox3, mainfrm.dtgvSRTF, mainfrm.SRTFTbx);
        }

        /*Thuật toán RR*/
		public void RoundRobin(MainForm mainfrm)
		{
			int index = -1;
			Node head = new Node();
			Node newNode = new Node();
            int quantum = int.Parse(mainfrm.QuantumTbx.Text); // lượng tử thời gian
            // hàm Parse ép kiểu String => số nguyên
			for (int i = 0; i < this.NumberOfProcess; i++)
			{
				if (!this.Processed[i])
				{
                    // kiểm tra CPU rỗi không
                    if (i != 0)
                    {
                        Node noProcess = new Node("CPU rỗi", -1, 0);
                        this.arrangedProcesses.addNodeAtTheEnd(noProcess);
                    }

					head = this.Process_List.Head;
					while (head.Number != i)
					{
						head = head.Next;
					}
					newNode = head.Clone();
                    this.ready.addNodeAtTheEnd(newNode);    // chuyển vào danh sách đã sẵn sàng
					this.Processed[i] = true;
					this.TotalTime = newNode.ArrivalTime;
					index++;
					this.TimeLine[index] = this.TotalTime;
					while (!this.ready.IsEmpty())
					{
						Node p = null;
						if (this.RemainingTime[newNode.Number] <= quantum)
						{
							this.ProcessingTime = this.RemainingTime[newNode.Number];
							p = newNode;
						}
						else
						{
							this.ProcessingTime = quantum;
						}
						Node node_1 = newNode.Clone();
						this.arrangedProcesses.addNodeAtTheEnd(node_1);
						this.TotalTime += this.ProcessingTime;
						index++;
						this.TimeLine[index] = this.TotalTime;
						this.RemainingTime[newNode.Number] -= this.ProcessingTime;
						Node next = this.Process_List.Head;
						for (int j = 0; j < this.NumberOfProcess; j++)
						{
							if (!((next.ArrivalTime > this.TotalTime) || this.Processed[next.Number]))
							{
								Node node_2 = new Node();
								node_2 = next.Clone();
								this.ready.addNodeAtTheEnd(node_2);
								this.Processed[node_2.Number] = true;
							}
							next = next.Next;
						}
						newNode = newNode.Next;
						if (p != null)
						{
							this.ready.GetNodeP(ref p);
						}
					}
				}
			}
			this.gantt = new Gantt();
			this.gantt.DrawGantt(this.Process_List, this.TimeLine, this.arrangedProcesses, mainfrm.pictureBox4, 
								 mainfrm.dtgvRR, mainfrm.RRTbx);
		}
   }   
}