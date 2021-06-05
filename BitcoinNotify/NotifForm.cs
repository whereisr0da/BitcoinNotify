/*

MIT License

Copyright (c) 2021 r0da [r0da@protonmail.ch]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BitcoinNotify
{
    public partial class NotifForm : Form
    {
        private Timer fadeIn;
        private Timer fadeOut;
        private Timer visible;

        /// <summary>
        /// Init the NotifForm, create NotifControl and start fade in timer
        /// </summary>
        /// <param name="size"></param>
        public NotifForm(Size size)
        {
            InitializeComponent();

            // set form size
            this.Width = size.Width;
            this.Height = size.Height;

            // get shifted pos from screen 
            this.Left = getLeftFormPosition();
            // windows 10 taskbar size
            this.Top = 70;

            this.Opacity = 0;
            base.Text = "BitcoinNotifiy";
            base.Icon = Properties.Resources.icon;

            NotifControl control = new NotifControl();

            control.Dock = DockStyle.Fill;

            this.Controls.Add(control);

            SetupTimers();

            // start to fade the form in
            fadeIn.Start();
        }

        // prevent focus stealing
        // from : https://stackoverflow.com/questions/3729899/opening-a-winforms-form-with-topmost-true-but-not-having-it-steal-focus
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private const int WS_EX_TOPMOST = 0x00000008;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }

        /// <summary>
        /// Setup timers, fadeIn, fadeOut, visible
        /// </summary>
        private void SetupTimers()
        {
            fadeIn = new Timer();
            fadeIn.Enabled = true;
            fadeIn.Interval = 1;
            fadeIn.Tick += FadeIn_Tick;

            fadeOut = new Timer();
            fadeOut.Enabled = false;
            fadeOut.Interval = 1;
            fadeOut.Tick += FadeOut_Tick;

            visible = new Timer();
            visible.Enabled = false;
            visible.Interval = MainController.config.visibleInterval;
            visible.Tick += Visible_Tick;
        }

        /// <summary>
        /// Visible tick handle, stop visible timer, start fade out timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Visible_Tick(object sender, EventArgs e)
        {
            // stop the visible timer
            visible.Enabled = false;
            visible.Stop();

            // start the fade out timer
            fadeOut.Enabled = true;
            fadeOut.Start();
        }

        /// <summary>
        /// Fade out tick handle, decrease the opacity, stop fade out timer, close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FadeOut_Tick(object sender, EventArgs e)
        {
            if (Opacity - 0.04 <= 0)
            {
                Opacity = 0;

                // stop the fade out timer
                fadeOut.Enabled = false;
                fadeOut.Stop();

                // close the form
                this.Close();
            }
            else
            {
                this.Opacity -= 0.04;
            }
        }

        /// <summary>
        /// Fade in tick handle, increase the opacity, stop fade in timer, start visible timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FadeIn_Tick(object sender, EventArgs e)
        {
            if (Opacity + 0.04 >= 1)
            {
                Opacity = 1;

                // stop the fade in timer
                fadeIn.Enabled = false;
                fadeIn.Stop();

                // start the visible timer
                visible.Enabled = true;
                visible.Start();
            }
            else
            {
                this.Opacity += 0.04;
            } 
        }

        /// <summary>
        /// Get x position from screen size
        /// </summary>
        /// <returns>The x location</returns>
        private int getLeftFormPosition()
        {
            // TODO : get the main working screen
            Screen rightmost = Screen.AllScreens[0];

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            return rightmost.WorkingArea.Right - this.Width;
        }
    }
}
