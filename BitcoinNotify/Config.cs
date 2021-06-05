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
using System.IO;
using System.Windows.Forms;

using IniParser;
using IniParser.Model;

namespace BitcoinNotify
{
    public class Config
    {
        // file config
        public string currency = "EUR";
        public string currencySymbol = "€";
        public int refreshInterval = 900000;
        public int visibleInterval = 12000;
        public string soundPath = "";
        public double lastPriceChecked = 0;
        public bool investmentEnabled = false;
        public List<Investment> investments;

        // TODO : save config

        /// <summary>
        /// Check if config exists and could be parsed, and load it if so
        /// </summary>
        public Config()
        {
            investments = new List<Investment>();

            // check that the config file exists
            if (!File.Exists("config.ini"))
            {
                MessageBox.Show("The config.ini file doesn't exists", "Error while loading config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            // load config once
            if (!LoadConfig())
            {
                MessageBox.Show("An error acured while loading config", "Error while loading config",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            if (File.Exists("investments.log"))
            {
                LoadInvestments();
            }
        }

        private void LoadInvestments()
        {
            string[] invest = File.ReadAllLines("investments.log");

            foreach (string item in invest)
                investments.Add(new Investment(item));

            investmentEnabled = investments.Count > 0;
        }

        /// <summary>
        /// Load the config file
        /// </summary>
        /// <returns>True if success</returns>
        private bool LoadConfig()
        {
            FileIniDataParser parser = new FileIniDataParser();

            bool result = true;

            try
            {   
                // TODO : from appdata
                IniData data = parser.ReadFile("config.ini");

                currency = data["config"]["currency"];

                // TODO
                currencySymbol = "€";

                refreshInterval = int.Parse(data["config"]["refreshInterval"]);
                visibleInterval = int.Parse(data["config"]["visibleInterval"]);
                lastPriceChecked = double.Parse(data["config"]["lastPriceChecked"]);
            }
            catch
            {
                result = false;
            }

            return result; 
        }

        /// <summary>
        /// Save last btc state to config file
        /// </summary>
        /// <returns>True if success</returns>
        public bool SaveLastStateConfig()
        {
            FileIniDataParser parser = new FileIniDataParser();

            bool result = true;

            try
            {
                // TODO : from appdata
                IniData data = parser.ReadFile("config.ini");

                data["config"]["lastPriceChecked"] = lastPriceChecked.ToString();

                parser.WriteFile("config.ini", data);
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
