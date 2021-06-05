# BitcoinNotify

A simple windows desktop notification app to keep you aware of Bitcoin evolution

![](Img/demo_1.gif)
![](Img/demo_2.gif)

# Features

- Create a desktop notification each 15 minutes (default settings)
- Show the current price of the bitcoin
- Show the difference with the last check bitcoin price (value, percent)
- Show investment rentability regarding the current Bitcoin price (value, percent)

# Investment

You can add an investment record to be informed about the rentability of your investment. By specifing your investments as follow in the `investments.log` file (Right click on tryicon > Investment History)

```
amount of money you invested | bitcoin price at the time | equivalent in bitcoin of your investment
```

Here an example :

```
25,05 | 12516,07 | 0,00031046
500   | 46776,82 | 0,00004297
```

# Settings 

All settings are in the `config.ini` file, you can acces it using the tryicon in your taskbar (Right click on tryicon > Settings)

- `currency` : the currency that you want (default is `EUR`)
- `refreshInterval` : the refresh time in miliseconds
- `visibleInterval` : the notification visible time in miliseconds 

# License

[This code is published on MIT License](https://fr.wikipedia.org/wiki/Licence_MIT)

This is tool uses :

- Web API of coindesk.com
- ini-parser : [MIT License](https://github.com/rickyah/ini-parser/blob/master/LICENSE)
- Newtonsoft.Json : [MIT License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)