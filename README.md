# BitcoinNotify

A simple windows desktop notification app to keep you aware of Bitcoin evolution

![](Img/demo_1.gif)
![](Img/demo_2.gif)

# Features

- Create a desktop notification each 15 minutes (default settings)
- Show the current price of the Bitcoin
- Show the difference with the last checked Bitcoin price (value, percent)
- Show investment rentability regarding the current Bitcoin price (value, percent)

# Investment

You can add an investment record to be informed about the rentability of your investment, by specifing your investments as follow in the `investments.log` file (Right click on tryicon > Investment History)

Here is the rows definition of one record :

```
amount of money you invested | bitcoin price at the time | equivalent in bitcoin of your investment
```

Here an example :

```
25,05 | 12516,07 | 0,00031046
500   | 46776,82 | 0,00004297
```

And each time the notification pops-up, the difference between the current Bitcoin value, and the one at the time of your investment (the rentability) is cumulated and shown in the notification (example: 525,05€ > 1089.49€)

# Settings 

All settings are in the `config.ini` file, you can acces it using the tryicon in your taskbar (Right click on tryicon > Settings)

- `currency` : the currency that you want (default is `EUR`)
- `refreshInterval` : the refresh time in miliseconds
- `visibleInterval` : the notification visible time in miliseconds 

# Todo list

- Startup launching
- Installer
- Settings in %AppData%
- Screen selection
- Screen location selection
- Closing button
- Color selection
- Investment loading from CSV files
- Better UI
- Port it to .NetCore to support other OS

# Change log

- 1.0.0.0
  - First release

# License

[This code is published on MIT License](https://fr.wikipedia.org/wiki/Licence_MIT)

This is tool uses :

- Web API of coindesk.com
- ini-parser : [MIT License](https://github.com/rickyah/ini-parser/blob/master/LICENSE)
- Newtonsoft.Json : [MIT License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)