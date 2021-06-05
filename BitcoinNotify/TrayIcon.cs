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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitcoinNotify
{
    public class TrayIcon : Form
    {
        private NotifyIcon notifyIcon;

        public TrayIcon()
        {
            base.Hide();
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Opacity = 0;

            ContextMenu trayIconContextMenu = new ContextMenu();

            trayIconContextMenu.MenuItems.Add("Refresh", RefreshClickHandle);
            trayIconContextMenu.MenuItems.Add("Investment History", InvestmentClickHandle);
            trayIconContextMenu.MenuItems.Add("Settings", SettingsClickHandle);
            trayIconContextMenu.MenuItems.Add("Exit", ExitClickHandle);

            notifyIcon = new NotifyIcon();

            notifyIcon.Visible = true;
            notifyIcon.ContextMenu = trayIconContextMenu;
            notifyIcon.Icon = Properties.Resources.icon;
            notifyIcon.Text = "BitcoinNotifiy";
        }

        private void RefreshClickHandle(object sender, EventArgs e)
        {
            MainController.Refrech();
        }

        private void InvestmentClickHandle(object sender, EventArgs e)
        {
            Process.Start("investments.log");
        }

        private void SettingsClickHandle(object sender, EventArgs e)
        {
            Process.Start("config.ini");
        }

        private void ExitClickHandle(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();

            Environment.Exit(0);
        }
    }
}
