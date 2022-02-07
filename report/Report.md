# Report: Pawn Game

## 1. Data Structures

### a. Board representation
We chose to represent the board as two bitboards of size 64 (8*8), one for each player, representing the location of all of a player's pawns:  
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L9-L9
We also use a byte to represent the location (if it exists) of an en-Passant Opportunity and a boolean to signal whether it even exists in the context:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L14-L15

### b. Agent data structures
The agent essentially uses a Graph data structure upon which it traverses and only keeps track of the current path to the root and their (all the predecessors) direct children

### c. Move generation
For each pawn, all possible movements are considered and checked whether they are legal in the current context, moves that are legal are stored and passed on to the caller:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L98-L127

### d. Terminal move detection
Terminal moves are detected when a match is lost by a player: 
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L155-L158
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L145-L153
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs#L129-L143

### e. Time allocation
We calculate a maximum time that is given for every move based on the assumption that a match won't take more than a reasonable amount of rounds that we have predetermined, 
under that assumption, we just divide the time evenly among all the moves.
That time limit is upheld by the search being an iteratively deepening search, when time is about to end the search to the last completed depth is already ready and is used:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L39-L49
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L73-L80

### f.
Our agent does not utilize the rival player's time to think as we simply didn't implement that for lack of time.

## 2. Heuristic Function

### a. Description
We used a simple evaluation function that takes into account material differences and how many spaces a player's pawns have pushed forward:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L18-L21
The material difference is just as you would expect and the distanceSum is just calculated as the sum of the row numbers minus 4.5 (from -3.5 to 3.5) for all of a player's pawns.  
In addition, on each layer we multiply the scores by a factor GAMMA smaller than one, to encourage the AI to take the fastest route to victory.
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L92
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/BoardAI.cs#L108

### b. Board features used
We used pawn counts and pawn positions (specifically their row).

### c. Board features extraction
The count and distanceSum variables are constantly maintained as moves are made and simulated (as well as when they are undone), 
so they are alwayes ready to be read with accurate data 
(can be seen in many places in the [board](https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Board.cs) class)

### d. Weighting
The weights are defined as constants:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Const.cs#L12-L13

### e. Result range
For these weights the results range between:  
`1*(0-8)+0.1*(-0-(8*3.5))=-36`  
`1*(8-0)+0.1*(-(8*-3.5)-0)=36`

### f. Terminal states
The terminal states are when someone wins. A win is evaluated as int.MaxValue for black and -int.MaxValue for white:
https://github.com/tom-amit/AI-game/blob/41eecefe21b54d6bc3d4785c8d976204f289e019/Const.cs#L9  

### g. Testing
Tested in games against itself, against us (human rivals), and against other teams' AI, we saw the way it acted and adjusted it accordingly.  
As well as checking the values it gave in specific situations (for example the starting board).

### h. Examples
Let's take the standard starting positioning, in that situation we expect the evaluation to be 0 to express that no one is advantageous, and it does:  
`1*(8-8)+0.1*(-20-(-20))=0`

### i. Used For:
It is used for leaves and for pruning

## 3. Search Algorithm

### a. Description
We used standard alpha-beta pruning with iterative deepening

### b. Search Heuristic?
Yes, as was mentioned in [_2. Heuristic Function_](https://github.com/tom-amit/AI-game/blob/main/report/Report.md#2-heuristic-function)

### c. Transposition table?
No.

### d. Search Depth
The resulting search depth is inherently tied to the time given (and of course the CPU of the computer) but even under extreme time limits such as 4 seconds per move, 
on my computer you could expect a minimum search depth of 7. The depth is capped at 25 so that is the max (usually only reached when the winner becomes obvious).
And on average across a game with an extreme time limit of 4 seconds per move, you can expect (on my computer) a depth of roughly 10-11.

### e. Game branching factor
The game's branching factor is roughly 8

### f. Effective branching factor
Though it changes as the game goes on, we found that the average effective branching factor we had was around 6.1

### g. Pruning
We used alpha-beta which is backwards pruning.  
The pruning saves on a lot of unnecessary searches.

### h. Search expansion?
No.

## 4. Learning and optimizations

### a. Learning algorithms and/or optimizations?
No.

### b. Measuring performance
1. yes, we tried different Heuristics and tested them as described in [_2. g. Testing_](https://github.com/tom-amit/AI-game/blob/main/report/Report.md#g-Testing)
2. Just by observing them and noticing any notable differences in the ways they played

## 5. General Questions

### a. The Development Process
At first, we focused on developing the game itself and on developing a suitable UI for it, to make it nicer for ourselves since we will have to use it a lot in the development process. After that was done we started laying foundations for the searching algorithm by working on things such as the undo move function and the Move class. Then we started working on implementing the search with alpha-beta pruning **without** a real heuristic function. At that point we started working on making a heuristic function, we tried different functions and weights and tested as mentioned above and eventually got to where we are.

### b. Conclusions about the game
First of all, even though it is an extreme simplification of chess, it is still very complex, and the apparent simplicity can make it very hard to know what a good move is even as a human.  
Furthermore, the game's branching factor is high enough that you cannot search deep enough at the beginning to see the path to guaranteed victory outright

### c. Conclusions from the project
We probably should have chosen a better fitting language, although c# is easier to develop in, c++ could have given us much better performance without a real change to our project.

### d. Possible improvements
for the way we worked:   
 - we should have thought about the fact that c++ will give us better performance even though it is harder to develop with.  
  
for the project:
 - using a transposition table
 - optimizing move generation
