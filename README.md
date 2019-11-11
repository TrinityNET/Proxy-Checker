# Proxy-Checker
Multi-Threaded Proxy Checker written in C#

![Example image](https://i.imgur.com/JPZTA2N.png)

Features:
 - Checks availability of the proxy (whether or not it responds to a ICMP ping). **Note this doesn't necessarily mean the proxy doesn't work, it just doesn't respond to an ICMP ping request!**
 - Grabs proxy country of origin.
 - Calculates round-trip time if proxy responds (in milliseconds).
 - Saves working and non-working proxies in seperate text files.
 - Writes logs to new folders labelled as the current date.

Uses http://ip-api.com/ API to locate origin of proxy. You can also use the API to grab more geographical information on the proxy.

More work will be done on this project, I just wanted to create this repo so I could push updates easier.

Excuse the awful code, I wrote this small project about two years ago and I recently stumbled upon it whilst looking through my old projects, so I decided to revamp it and improve the performance.
