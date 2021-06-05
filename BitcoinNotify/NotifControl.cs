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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace BitcoinNotify
{
    public partial class NotifControl : UserControl
    {
        private Font font;

        private SolidBrush textColorWhite;
        private SolidBrush textColorRed;
        private SolidBrush textColorGreen;
        private SolidBrush textColorLightRed;
        private SolidBrush textColorLightGreen;
        private SolidBrush textColorDark;

        private int topGradientBoxSize = 4;
        private int textLocationBaseX;
        private int textLocationBaseY;
        private int borderSize = 20;

        private Timer showLastValue;

        private bool showInvestment = false;

        /// <summary>
        /// Init notification control, set size and locations
        /// </summary>
        public NotifControl()
        {
            InitializeComponent();

            font = new Font("Segoe UI Light", 17);

            textColorWhite = new SolidBrush(Color.White);
            textColorDark = new SolidBrush(Color.FromArgb(148, 148, 148));
            textColorGreen = new SolidBrush(Color.FromArgb(22, 218, 81));
            textColorRed = new SolidBrush(Color.FromArgb(223, 73, 100));
            textColorLightRed = new SolidBrush(Color.FromArgb(236, 147, 163));
            textColorLightGreen = new SolidBrush(Color.FromArgb(147, 236, 173));

            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(60, 60);
            pictureBox.Image = Properties.Resources.bitcoin_static;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            textLocationBaseX = pictureBox.Size.Width + (borderSize * 2);
            textLocationBaseY = ((this.Height / 2) - topGradientBoxSize) - (pictureBox.Size.Height / 2);

            pictureBox.Location = new Point(borderSize, textLocationBaseY);

            Controls.Add(pictureBox);

            showLastValue = new Timer();
            showLastValue.Enabled = MainController.config.investmentEnabled;
            showLastValue.Interval = MainController.config.visibleInterval / 2;
            showLastValue.Tick += ShowLastValue_Tick;
        }

        private void ShowLastValue_Tick(object sender, EventArgs e)
        {
            showInvestment = true;
            // update UI
            base.Refresh();
        }

        /// <summary>
        /// Paint the notification control
        /// </summary>
        /// <param name="pe">Paint event</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            // setup text smoothing
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.High;

            // setup background color
            this.BackColor = Color.FromArgb(45,45,45);

            Rectangle topGradientBox = new Rectangle(0, 0, this.Width, topGradientBoxSize);

            SolidBrush textColor = textColorWhite;
            SolidBrush lightGradientColor = textColorDark;

            string priceStr = GetBtcPriceString(ref textColor, ref lightGradientColor);

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(topGradientBox, textColor.Color, lightGradientColor.Color, 45f);

            // draw gradient on top
            g.FillRectangle(linearGradientBrush, topGradientBox);

            // draw current btc value
            g.DrawString(priceStr, font, textColor, textLocationBaseX, textLocationBaseY - 5);

            if (!showInvestment)
            {
                string oldPriceStr = "Last value : " + MainController.config.lastPriceChecked.ToString("0.00").Replace(",", ".") + " " + MainController.config.currencySymbol;

                // draw old btc value
                g.DrawString(oldPriceStr, font, textColorDark, textLocationBaseX, textLocationBaseY + 31);
            }
            else
            {
                SolidBrush investmentStrColor = textColorDark;

                string investmentStr = GetInvestmentStr(ref investmentStrColor);

                g.DrawString(investmentStr, font, investmentStrColor, textLocationBaseX, textLocationBaseY + 31);
            }
        }

        /// <summary>
        /// Get Bitcoin price printable string
        /// </summary>
        /// <returns>The printable string of the price</returns>
        private string GetBtcPriceString(ref SolidBrush textColor, ref SolidBrush lightGradientColor)
        {
            string result = "";
            string increaseSymbol = "";
            string increaseSymbolAlgo = "";

            // calc price different
            double diff = MainController.btcInfo.price - MainController.config.lastPriceChecked;

            // if the price is different, set the icon accordingly
            if (diff != 0) {
                increaseSymbol = (diff > 0) ? "▲ " : "▼ ";
                increaseSymbolAlgo = (diff > 0) ? "+" : "-";

                // change the color if value changed
                textColor = (diff > 0) ? textColorGreen : textColorRed;
                lightGradientColor = (diff > 0) ? textColorLightGreen : textColorLightRed;
            }

            result += increaseSymbol + MainController.btcInfo.price.ToString("0.00").Replace(",", ".") + " " + MainController.config.currencySymbol;

            // if the price is different, add the difference string
            if (diff != 0){

                // normalize diff value
                diff = (diff < 0) ? (diff * -1) : diff;

                result += " (" + increaseSymbolAlgo + diff.ToString("0.00") + MainController.config.currencySymbol + ")";

                if (MainController.btcInfo.price != 0 && MainController.config.lastPriceChecked != 0)
                {
                    double diffPrecent = (diff / MainController.config.lastPriceChecked) * 100;

                    if (diffPrecent < 1)
                        result += " (" + increaseSymbolAlgo + diffPrecent.ToString("0.00") + "%)";
                    else
                        result += " (" + increaseSymbolAlgo + diffPrecent.ToString("0") + "%)";
                }
            }

            return result;
        }

        /// <summary>
        /// Get Bitcoin investment string
        /// </summary>
        /// <param name="investmentStrColor"></param>
        /// <returns></returns>
        private string GetInvestmentStr(ref SolidBrush investmentStrColor)
        {
            string result = "";

            string increaseSymbol = "";
            string increaseSymbolAlgo = "";

            double diff = 0;

            double investmentCumul = 0;

            foreach (var item in MainController.config.investments)
            {
                double priceOfBtcValue = MainController.btcInfo.price * item.btcAmount;

                diff += priceOfBtcValue - item.currencyValue;
                investmentCumul += item.currencyValue;
            }

            if (diff != 0)
            {
                increaseSymbol = (diff > 0) ? "▲ " : "▼ ";
                increaseSymbolAlgo = (diff > 0) ? "+" : "-";

                // change the color if value changed
                investmentStrColor = (diff > 0) ? textColorGreen : textColorRed;

                // normalize diff value
                double diffDelta = (diff < 0) ? (diff * -1) : diff;

                result += increaseSymbol + diffDelta.ToString("0.00").Replace(",", ".") + " " + MainController.config.currencySymbol;

                if (investmentCumul != 0)
                {
                    double diffPrecent = (diffDelta / investmentCumul) * 100;

                    if (diffPrecent < 1)
                        result += " (" + increaseSymbolAlgo + diffPrecent.ToString("0.00") + "%)";
                    else
                        result += " (" + increaseSymbolAlgo + diffPrecent.ToString("0") + "%)";

                    result += " (" + investmentCumul.ToString("0.00") + " " + MainController.config.currencySymbol + " > " + (investmentCumul + diff).ToString("0.00") + " " + MainController.config.currencySymbol + ")";
                }
            }
            else
            {
                result = "Nothing changed about your investment";
            }

            return result;
        }
    }
}
