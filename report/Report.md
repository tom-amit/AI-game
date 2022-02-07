# Report: Pawn Game
## Data Structures
### a. Board represenation
We chose to represent the board as two bitboards of size 64 (8*8), one for each player, representing the location of all of a player's pawns:  
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L9-L9
We also use a byte to represent the location (if it exists) of an en-Passent Opportunity and a boolean to signal whether it even exists in the context:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L14-L15

### b. Agent data structures
The agent essentialy uses a Graph data structure upon which it traverses and only keeps track of the current path to the root and their (all the predecessors) direct children

### c. Move generation
For each pawn, all possible movements are considered and checked wether they are legal in the current context, moves that are legal are stored and passed on to the caller:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L98-L127

### d. Terminal move detection
Terminal moves are detected when a match is lost by a player: 
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L155-L158
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L145-L153
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L129-L143

### e. Time allocation
We calculate a time that is given for every move based on the assumption that a match wont take more then a reasonable amount of round that we have predetermined, 
under that assumption we jsut divide the time evenly among all the moves.
That time limit is upheld by the search being an itereativly deepening search, when time is about to end the search to the last completed depth is already ready and is used:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L39-L49
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L73-L80

### f.
Our agent does not utilize the rival player's time to think, we simply didn't implement that for lack of time

## Heuristic Function
### a. Description
We used a simple evaluation function that takes into account material differences and the amount a player's pawns have pushed forward:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L18-L21
The material difference is just as you'd expect and the distanceSum is just calculated as the sum of the row numbers (from 0 to 7) for all of a player's pawns  
In addition, on each layer we multiply the scores by a factor GAMMA smaller then one, to encourage the AI to take the fastest way to victory
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L92
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L108

### b. Board features used
We used pawn counts and pawn positions (specifically their row)

### c. Board features extraction
The count and distanceSum variables are constanly maintained as moves are made and simulated (as well as when they are undone), 
so they are alwayes ready to be read with accurate data 
(can be seen in many places in the [board](https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs) class)

### d. Weighting
The weights are defined as constanst:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Const.cs#L12-L13

### e. Result range
For these weights the results range between:  
`1*(0-8)+0.1*(-0-(8*7))=-13.6`  
`1*(8-0)+0.1*(-(8*0)-0)=8`

### f. Terminal states
The terminal states are when someone wins, a win is evaluated as int.MaxValue for black and -int.MaxValue for white:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Const.cs#L9  

### g.
Tested in games against itself, against us (human rivals), and against other teams' AI, we saw the way it acted and and adjusted it accordingly

### h.


### i.

