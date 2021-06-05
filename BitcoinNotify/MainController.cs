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
using System.Threading;
using System.Windows.Forms;

namespace BitcoinNotify
{
    public class MainController
    {
        public static BitcoinInformation btcInfo;
        public static Config config;
        public static bool running = true;
        public static bool inNotification = false;

        private static NotifForm mainForm;

        /// <summary>
        /// Main routine function
        /// </summary>
        public static void Start()
        {
            // init user config
            config = new Config();

            // init the btc interface
            btcInfo = new BitcoinInformation();

            new Thread(() => CreateTrayIcon()).Start();

            // show notification
            ShowNotification();

            // main loop
            while (running)
            {
                Thread.Sleep(config.refreshInterval);

                // start the update timer
                UpdateTimerTick();
            }
        }

        public static void Refrech()
        {
            if (inNotification)
                return;

            config = new Config();
            btcInfo = new BitcoinInformation();
            ShowNotification();
        }

        /// <summary>
        /// Update the btc informations and show the notification
        /// </summary>
        private static void UpdateTimerTick()
        {
            // update the btc information
            if (!btcInfo.Update())
                // TODO : check if it's better to alert the user if the network is not available
                return;

            // show notification
            ShowNotification();
        }

        /// <summary>
        /// Create and show the notification, and saves last state after its end 
        /// </summary>
        private static void ShowNotification()
        {
            // create the notification form
            CreateNotification();

            // save the last checked price 
            config.lastPriceChecked = btcInfo.price;

            // save the last checked price in the config
            config.SaveLastStateConfig();
        }

        /// <summary>
        /// Create the notification form, and show it
        /// </summary>
        private static void CreateNotification()
        {
            mainForm = new NotifForm(new Size(500, 100));

            inNotification = true;

            // show the notification
            mainForm.ShowDialog();

            inNotification = false;

            // free form usages
            mainForm.Dispose();

            // make sure that the form will be in the garbage collector
            mainForm = null;

            // store all useless objects, and remove them from memory
            GC.Collect();

            // wait until garbage collection ends
            GC.WaitForPendingFinalizers();
        }

        private static void CreateTrayIcon()
        {
            new TrayIcon().ShowDialog();
        }
    }
}
