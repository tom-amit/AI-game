# _INSTRUCTIONS TO RUN THE CODE AND EXECUTALBES CAN BE FOUND IN THE [README](../README.md)_

# Report: Pawn Game
Made by: 
 - Amit Sivan - [@amits6666](https://github.com/amits6666)
 - Tom Kark - [@tomkark](https://github.com/tomkark)
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
We calculate a maximum time that is given for every move based on the assumption that a match won't take more than a reasonable number of rounds that we have predetermined, 
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
  
  
  
# Example of game (AI is P2(Black)):
![image](https://user-images.githubusercontent.com/29578065/152830158-45164e00-a9a4-4ffd-ac7b-8119f21b63a6.png)
![image](https://user-images.githubusercontent.com/29578065/152830127-8ff233c5-f7e9-49c5-82e8-c0b85f7072d0.png)
![image](https://user-images.githubusercontent.com/29578065/152830107-f1caaabd-7c97-4fc7-a667-972700c49560.png)
![image](https://user-images.githubusercontent.com/29578065/152830081-e9ae8b37-762e-4016-8bc4-f3b42eff45cf.png)
![image](https://user-images.githubusercontent.com/29578065/152830055-e5d09562-2a48-4cc7-b47c-676cceca2ae4.png)
![image](https://user-images.githubusercontent.com/29578065/152830023-49d1b4f3-ffd3-4fd0-ab98-7761921c8b2d.png)
![image](https://user-images.githubusercontent.com/29578065/152829999-1ac285e9-c156-40e3-ac11-61a35708caab.png)
![image](https://user-images.githubusercontent.com/29578065/152829979-26628b61-c015-4893-98f9-ff17b5193b5a.png)
![image](https://user-images.githubusercontent.com/29578065/152829946-dc89bfde-ba96-406e-922e-e3a8f9b7eed0.png)
![image](https://user-images.githubusercontent.com/29578065/152829913-4bd694ac-0445-4b99-a6eb-0c5b3b42f5ed.png)
![image](https://user-images.githubusercontent.com/29578065/152829878-a645241b-072c-4230-a818-82450b4fa0d5.png)
![image](https://user-images.githubusercontent.com/29578065/152829862-745a6b7d-7974-401e-8ad4-96f71b151e72.png)
![image](https://user-images.githubusercontent.com/29578065/152829839-15c9574f-59ce-4091-b8a1-f1650ef1c77c.png)
![image](https://user-images.githubusercontent.com/29578065/152829819-2621742a-fbdb-4fcc-8933-4a1558e44a68.png)
![image](https://user-images.githubusercontent.com/29578065/152829795-c1c9ea6b-cdb0-4d2a-b1e0-7cedd8076c24.png)
![image](https://user-images.githubusercontent.com/29578065/152829765-5a77c8c3-0465-47e0-a6e3-7c4ef9669064.png)
![image](https://user-images.githubusercontent.com/29578065/152829728-2b1ca6d1-b57c-4e80-8649-a42e8ce3989f.png)
![image](https://user-images.githubusercontent.com/29578065/152829670-68c633cf-9c75-406c-a142-be7e05392d00.png)
![image](https://user-images.githubusercontent.com/29578065/152829627-ebddcc8e-fae1-4436-a2fc-b0f0d7f4409b.png)
![image](https://user-images.githubusercontent.com/29578065/152829594-c1f58e7d-1c83-459f-b178-51b5108cfa28.png)
![image](https://user-images.githubusercontent.com/29578065/152829555-114c71c5-b1f3-4fd1-8c00-7bb49c5a921a.png)
![image](https://user-images.githubusercontent.com/29578065/152829528-ff17cd98-6b6d-4fb6-aafe-1d119e7551a2.png)
![image](https://user-images.githubusercontent.com/29578065/152829487-fa94ab15-6a86-4ccd-9cf2-4b151fe6d6a9.png)
![image](https://user-images.githubusercontent.com/29578065/152829445-b1adddf3-8210-4c40-8586-00d8f4b30bd7.png)
![image](https://user-images.githubusercontent.com/29578065/152829303-dcb51adb-2f23-44e3-82f0-373c126b23db.png)
![image](https://user-images.githubusercontent.com/29578065/152829274-07cd9e5c-5619-436a-9d44-db8663e9d891.png)
![image](https://user-images.githubusercontent.com/29578065/152829244-9e8b26a9-3b08-4c7a-b247-6fdc6b18aa06.png)
![image](https://user-images.githubusercontent.com/29578065/152829215-6c0d499f-d3be-4d31-8dd0-b419334d3e42.png)
![image](https://user-images.githubusercontent.com/29578065/152829028-ae54c02d-7d68-467c-9036-b6db42088a7a.png)
