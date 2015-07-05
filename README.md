# Tempest Watch companion app/bot

Companion app/bot for https://github.com/jayvan/tempest-watch.
The idea is this will run on a dedicated machine with a Path of Exile client and account that is always online.
Players will be able PM this account and it will forward their weather reports to the Tempest Watch web app for processing.
This app only reads off the Client.txt file and does not interact with the Path of Exile client in any way.

# FAQ

## How does it work?
The app monitors the specified Client.txt file. When the PoE client writes to the Client.txt, this will read the new chat message and forward the data to the Tempest Watch API.
The app keeps track of player names and will not accept more than 1 report per player per hour to prevent trolling/griefing.
Due to how big the Client.txt can get, its contents will be cleared every 30 seconds.
This app does not hold any data, it is merely a relay.

## How do I use it in-game?
Simple, just send a PM to <CHARACTER NAME PENDING> with the following format:

####!report <Map>, <Prefix>, <Suffix>

Example, you notice the Crypt has a Tempest of Suffering. You send the following PM:

####!report Crypt, , Suffering

Bear in mind the bot does not interact with the PoE client so it will not respond to you.

## Can I run this on my machine?
You can, if you tweak the configuration file.

## SHOULD I run this on my machine?
No. It will screw up your Client.txt file and won't give you any benefit over PMing your report or simply going over to http://poetempest.com/ 

## I want to run this on my machine anyway!
You're welcome to download, compile and run the Visual Studio solution. Just don't expect me to care if you start blaming me for any random problems you start having because they won't be my fault.

## Can I help out with development?
I'm open to any suggestions, but that's it. The project is rather simple and I don't need random strangers touching my code.

