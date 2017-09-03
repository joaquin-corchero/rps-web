# Rock, Paper, Scissors #

Game Rules
A match takes place between 2 players and is made up of 3 games, with the overall winner being the first player to win 2 games (i.e. best of 3).
Each game consists of both players selecting one of Rock, Paper or Scissors; the game winner is determined based on the following rules:

- Rock beats scissors
- Scissors beats paper
- Paper beats rock

## Requirements ##
Your application must support three types of player:
- Human Player: must be prompted for a selection of Rock, Paper or Scissors for each turn
- Random Computer Player: should automatically select one of Rock, Paper or Scissors at random for each turn.
- Tactical Computer Player: should always select the choice that would have beaten its last choice, e.g. if it played Scissors in game 2, it should play Rock in game 3.


You must include a user interface of some kind, but the choice of how this is implemented is up to you: console application, web site, WPF application, etc. – any are acceptable.
You should supply unit tests with your solution.  You may use any unit testing framework/mocking framework with which you are comfortable.

## Extensions ##
The following are some of the possible extensions that may be made to the application at a later date.  You do not need to implement these, but they should be considered in your design.
- New player types: may want to add new computer player implementations as tactics improve
- Longer matches:  may want to change the match format to “best of 5” at a later date
- New “moves”: may expand the possible moves that each player can make (e.g. Rock, Paper, Scissors, Lizard, Spock)