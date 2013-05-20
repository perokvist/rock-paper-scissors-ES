#Rock Paper Scissor - Lab #2

### Prerequisites

- VS 2012
- LocalDB
- Nuget 

### Setup

1.1. Clone Repo

1.2. Validate build
- You may need to restore nuget packages
- Set Startup Project to Api project.
- Build

1.3
- Run application
- Verify that the databases are created default instance ((localDB\v11.0))
- Endpoint doc yourhost/Help/Api/POST-api-Game.

1.4 
- Contracts
[Gist of lab classes](https://gist.github.com/perokvist/5597873)
- Create all implementations and contracts in Infrastructure.Signaling folder.

## Task No 1

Create notification for all users when a Game is created.

With two browsers visiting your site, both should get a notification when a new Game is created.

## Task No 2

Create notification for only the user that created the game.

With two browsers visiting your site, only the one that creates the game should get a notification when a new Game is created.

## Resources
[SignalR JS doc](https://github.com/SignalR/SignalR/wiki/SignalR-JS-Client-Hubs)