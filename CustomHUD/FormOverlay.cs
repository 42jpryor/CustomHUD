using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomHUD
{
    public partial class FormOverlay : Form
    {
        private string windowTitle;
        Color crosshairColor = Color.White;

        
        IntPtr handle;

        public struct RECT
        {
            public int left, top, right, bottom;
        }
        RECT rect;



        // Dll imports
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll")]
        
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        //public static Bitmap PrintWindow(IntPtr hwnd)
        //{
        //    RECT rc;
        //    GetWindowRect(hwnd, out rc);

        //    Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
        //    Graphics gfxBmp = Graphics.FromImage(bmp);
        //    IntPtr hdcBitmap = gfxBmp.GetHdc();

        //    PrintWindow(hwnd, hdcBitmap, 0);

        //    gfxBmp.ReleaseHdc(hdcBitmap);
        //    gfxBmp.Dispose();

        //    return bmp;
        //}



        public FormOverlay(Form1 MainForm, string windowTitle)
        {
            InitializeComponent();


            this.windowTitle = windowTitle;
            handle = FindWindow(null, this.windowTitle);
        }

        private void FormOverlay_Load(object sender, EventArgs e)
        {
            // Set tan to be the transparent color
            this.BackColor = Color.Tan;
            this.TransparencyKey = Color.Tan;
            
            // Set as topmost so it's always seen
            this.TopMost = true;
            
            //this.Activate();
            
            // Get rid of the border for the window
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            // 
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);


            GetWindowRect(handle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Top = rect.top;
            this.Left = rect.left;

        }

        // Set the crosshair color
        public void SetCrosshairColor(Color color)
        {
            crosshairColor = color;

            this.Refresh();
        }

        private void FormOverlay_Paint(object sender, PaintEventArgs e)
        {
            const int radius = 10;
            int windowX = rect.left;
            int windowY = rect.top;


            Color grayColor = Color.LightGray;
            Color blackColor = Color.Black;
            Color lineColor = Color.Red;

            Graphics g;
            Pen grayPen = new Pen(grayColor, 3);
            Pen blackPen = new Pen(blackColor, 3);
            Pen whitePen = new Pen(crosshairColor, 1);
            Pen redPen = new Pen(lineColor, 2);

            g = e.Graphics;

            
            Rectangle circle = new Rectangle((this.Size.Width/2) - (radius/2), (this.Size.Height/2) - (radius / 2), radius, radius);

            // Draw rectangle using newRectangle
            g.DrawEllipse(blackPen, circle);
            g.DrawEllipse(whitePen, circle);




            for (int x = 0; x < 3; x++)
            {
                // g.DrawLine(pen,xStart,yStart,xEnd,yEnd);

                // Horizontal Lines
                g.DrawLine(blackPen, rect.right / 2 - 21, (rect.bottom / 2) + 38 + (66 * x), (rect.right / 2) + 25, (rect.bottom / 2) + 38 + (66 * x));
                g.DrawLine(grayPen, rect.right / 2 - 20, (rect.bottom / 2) + 38 + (66 * x), (rect.right / 2) + 23, (rect.bottom / 2) + 38 + (66 * x));

                g.DrawLine(blackPen, rect.right / 2 - 13, (rect.bottom / 2) + 60 + (66 * x), (rect.right / 2) + 17, (rect.bottom / 2) + 60 + (66 * x));
                g.DrawLine(redPen, rect.right / 2 - 12, (rect.bottom / 2) + 60 + (66 * x), (rect.right / 2) + 15, (rect.bottom / 2) + 60 + (66 * x));

                //g.DrawLine(blackPen, rect.right / 2 - 13, (rect.bottom / 2) + 60 + (66 * x), (rect.right / 2) + 17, (rect.bottom / 2) + 60 + (66 * x));
                //g.DrawLine(grayPen, rect.right / 2 - 12, (rect.bottom / 2) + 60 + (66 * x), (rect.right / 2) + 15, (rect.bottom / 2) + 60 + (66 * x));

                g.DrawLine(blackPen, rect.right / 2 - 13, (rect.bottom / 2) + 82 + (66 * x), (rect.right / 2) + 17, (rect.bottom / 2) + 82 + (66 * x));
                g.DrawLine(grayPen, rect.right / 2 - 12, (rect.bottom / 2) + 82 + (66 * x), (rect.right / 2) + 15, (rect.bottom / 2) + 82 + (66 * x));
            }


            //for (int x = 0; x < 8; x++)
            //{
            //    if(x != 4)
            //    {
            //        // Vertical Lines
            //        g.DrawLine(blackPen, lineIncrement * x, (1080 / 2) - 21, lineIncrement * x, (1080 / 2) + 25);
            //        g.DrawLine(grayPen, lineIncrement * x, (1080 / 2) - 20, lineIncrement * x, (1080 / 2) + 23);
            //    }
            //}

            //Rectangle circleOutline = new Rectangle((1920 / 2), (1080 / 2), radius, radius);
            //g.DrawEllipse(blackPen, circleOutline);


            // Red Circle
            //Rectangle targetCircleOne = new Rectangle((1920 / 2), (rect.bottom / 2) + 82 - (radius / 2), radius, radius);
            //g.DrawEllipse(redPen, targetCircleOne); ;
            //Rectangle targetCircleOneOutline = new Rectangle((1920 / 2), (rect.bottom / 2) + 82 - (radius / 2), radius, radius);
            //g.DrawEllipse(blackPen, targetCircleOneOutline);

        }

        
    }
}
