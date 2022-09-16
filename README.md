# Monopolyopoloy

### CS4091 Senior Design Project

## Base Requirements

The basic premise of this project is creating a monopoly game. You will do this as a team using whatever technology you would like. You could create a mobile game, you could create a web game using pure JavaScript, you could use Pygame, Unity, even C++ in a terminal... well OK, not that. Pick whatever technology you would like as long as the whole team agrees on it.

While making the game, you are also more than welcome to give it any sort of theme you would like. Maybe it could be Rollaopoly (that actually exists!), Missouri S&T Comp Sciopoly, basic Monopoly, Winteropoly, etc.

You have a lot of freedom with this project as long as the game satisfies the following requirements (a slightly simplified version of normal Monopoly):

- The game allows up to **four players** with **auto-play** options for all four. This means you should be able to start the game with four auto-players (so no actual humans are playing), and we can just sit back and watch the computer play.
- There must be **two dice** that are rolled to determine how far a player moves each round.
- There must be at least **6 distinct "Chance" cards** and **6 distinct "Community Chest" cards** (although you can call these by different names and increase the number if that fits your theme better).
- Each player starts the game with **$1500** (the original game divided as follows: 2 x $500, 2 x $100, 2 x $50, 6 x $20, 5 x $10, 5 x $5, and 5 x $1). The base requirement is that you keep track of funds and do not allow spending of funds if funds are not sufficient.
- The board is setup in **40 spaces** as follows:
  - 4 Corners:
    - **Go** - This is the starting point of the game. While playing, if a player passes over or lands on Go, they receive $200.
    - **Jail** - If a player lands on Jail - or passes over Jail - nothinghappens. However, if a player is "Sent to Jail", then they are now in Jail. There are three ways to get out of Jail:
      - Pay $50 to the Bank before throwing dice in the first or second turn in jail.
      - Roll Doubles (both dice show the same number) in any of the next three turns.
      - Use a "Get out of jail free" card. These can be purchased from other players at a negotiated price.
    - **Go to Jail** - If a player lands here, they are sent to Jail. This means they have to move over to the Jail square.
    - **Free Parking** - Nothing happens here
  - **3 Chance spaces** - If a player lands here, they draw a Chance card and follow directions. (These traditionally will either move the player, send the player to jail, gain the player money, or force the player to pay some fees. YOUR behavior can be your own.)
  - **3 Community Chest spaces** - If a player lands here, they draw a Community Chest card and follow directions. (These traditionally will all either gain the player money, or cause them to lose money. YOUR behavior can be your own.)
  - 28 Properties:
    - **22 Streets** grouped into **8 distinct color categories**
    - **4 Railroads** - Rent will increase based on the number of railroads owned. This will be explained in the "Title Deed" card for the railroad.
    - **2 Utilities** - Rent is determined by the player's roll that got them to this utility. If one player owns both utilities, the rent will be calculated as the dice roll times 10. For example, if the player rolled 7, Rent would be 7 x 10 = 70. If a player only owns one utility, Rent is calculated as the dice roll times 4. (The "Title Deed" card for each property should state this)
  - **1 Luxury Tax space** - If a player lands here, they pay the Bank $100
  - **1 Income Tax space** - If a player lands here, they pay the Bank $200
- When a player lands on a square that isn't currently owned by anyone, **they can purchase that square**. If purchased, the player will pay the amount listed on the square to the Bank and receive the **"Title Deed"** card for that property.
  - **OPTIONAL** - If a player owns all of the properties for a given color, they can then begin to purchase **houses/hotels**. If a house is purchased, it can be placed on any property in that color. All properties of a color must have one house before a second house can be placed on the property (same rule applies for placing a third house). When a player has 3 houses on every property of a color, they can then purchase a hotel and replace the 3 houses with a hotel. Only one hotel can be purchased per property. Rent will increase for a property as the number of houses increases - or if a hotel is built. The prices for building houses/hotels, and the price of rent at each level (number of houses/hotel present) will be on the "Title Deed" card.
- When a player lands on a square that is owned by another player, they must pay the owner **"Rent"**. Rent is determined by the "Title Deed" card.
  - **OPTIONAL** - When a player runs out of money, they become **"Bankrupt"**. When this happens, your game mechanics will need to determine the behavior to deal with this (e.g., returning property to the Bank, selling back at a loss to continue on, etc.) If this happens when a player is on another player's property, the owner will inherit all of the other player's properties. Otherwise, all of the property is returned to the Bank.
- The game ends when all players except one are Bankrupt.

## Resources

- Real Monopoly rules from hasbro.com (the basic requirements for our version exclude some things but you are more than welcome to follow the real rules): https://www.hasbro.com/common/instruct/00009.pdf.
- Fandom: https://monopoly.fandom.com/wiki/Monopoly

## Additional Requirements Overview

- Graphics are required, however, we will implement the game with 3D graphics and Unity
  - 3D individual player pieces
  - 3D table, with 2D board
- Sound Effects
  - Audio cues for in-game events
  - Background music
- Physical dice rolling (if time permitting)
  - Outcome is passed to player behavior
- Real Estate Construction
  - Time permitting
  - Houses, hotels, building on Monopoly management
- Smooth animations for... if time permitting
  - Piece movements
  - Card movements
- Autoplayer AI Difficulty Selection
