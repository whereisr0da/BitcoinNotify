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
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace BitcoinNotify
{ 
    public class BitcoinInformation
    {
        public double price { get; set; }

        private static string PRICE_API_URL = "https://api.coindesk.com/v1/bpi/currentprice/"; 

        /// <summary>
        /// Init btc object by getting the price once
        /// </summary>
        public BitcoinInformation()
        {
            // update the bitcoin info once
            if (!Update()) {
                MessageBox.Show("An error acured while using the coindesk.com API. Please check your internet connection, or update this software", "Error while getting Bitcoin informations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Update the btc price
        /// </summary>
        /// <returns>True if success</returns>
        public bool Update()
        {
            string rawJsonData = string.Empty;

            try
            {
                // get price from api
                rawJsonData = new WebClient().DownloadString(PRICE_API_URL + MainController.config.currency + ".json");
            }
            catch
            {
                // if there is an error will resolving the value
                return false;
            }

            // if there is an error in api data
            if (string.IsNullOrEmpty(rawJsonData))
                return false;

            JObject jsonData = new JObject();

            try
            {
                // resolve json from raw string
                jsonData = JObject.Parse(rawJsonData);
            }
            catch
            {
                // if there is an error in json parsing
                return false;
            }

            rawJsonData = null;

            try
            {
                // get the current price from json parsing
                this.price = jsonData["bpi"][MainController.config.currency]["rate_float"].Value<float>();
            }
            catch
            {
                // if price object is not found
                return false;
            }

            jsonData.RemoveAll();
            jsonData = null;

            return true;
        }
    }
}
