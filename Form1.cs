using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace namik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class WinApi
        {
            [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
            public static extern int GetSystemMetrics(int which);

            [DllImport("user32.dll")]
            public static extern void
                SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                             int X, int Y, int width, int height, uint flags);

            private const int SM_CXSCREEN = 0;
            private const int SM_CYSCREEN = 1;
            private static IntPtr HWND_TOP = IntPtr.Zero;
            private const int SWP_SHOWWINDOW = 64; // 0x0040

            public static int ScreenX
            {
                get { return GetSystemMetrics(SM_CXSCREEN); }
            }

            public static int ScreenY
            {
                get { return GetSystemMetrics(SM_CYSCREEN); }
            }

            public static void SetWinFullScreen(IntPtr hwnd)
            {
                SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
            }
        }

        /// <summary>
        /// Class used to preserve / restore state of the form
        /// </summary>
        public class FormState
        {
            private FormWindowState winState;
            private FormBorderStyle brdStyle;
            private bool topMost;
            private Rectangle bounds;

            private bool IsMaximized = false;

            public void Maximize(Form targetForm)
            {
                if (!IsMaximized)
                {
                    IsMaximized = true;
                    Save(targetForm);
                    targetForm.WindowState = FormWindowState.Maximized;
                    targetForm.FormBorderStyle = FormBorderStyle.None;
                    targetForm.TopMost = true;
                    WinApi.SetWinFullScreen(targetForm.Handle);
                }
            }

            public void Save(Form targetForm)
            {
                winState = targetForm.WindowState;
                brdStyle = targetForm.FormBorderStyle;
                topMost = targetForm.TopMost;
                bounds = targetForm.Bounds;
            }

            public void Restore(Form targetForm)
            {
                targetForm.WindowState = winState;
                targetForm.FormBorderStyle = brdStyle;
                targetForm.TopMost = topMost;
                targetForm.Bounds = bounds;
                IsMaximized = false;
            }
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormState formState = new FormState();
            formState.Maximize(this);
            webBrowser1.Navigate("https://www.youtube.com/watch?v=dQw4w9WgXcQ/");
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             e.Cancel = true;

        }
    }
}
