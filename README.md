Realization: Procedural content within a Unity simulation. Developing behaviour for different kinds of agent, based on the steering forces. Developed under Unity in C# for COMP521(Modern Computer Games) at Mcgill.

Travelling agents, shown in red, spawn from the rightmost doorway and attempt to get to one of the exit doorways on the left. They move at varying, but relatively fast speeds. The choice of exit each aims at is set randomly at spawn time, but if they are unable to reach their intended exit doorway for an extended time they may change their mind.

Some agents simply wander around the level, moving at varying but relatively fast speeds. These agents are shown above in green. A wanderer that is just wandering, and which comes close (within a radius of about a doorway width or two) to a traveller attempt to interfere with the traveller’s goal by interposing itself between the traveller and the traveller’s intended exit.

Social agents, shown in yellow move randomly through the level. When one comes close enough to another social (yellow) agent, it may choose to enter into “conversation” with the agent, either forming or entering the resulting social group.

Licence:

I used arongranberg A* for the AI pathfinding: https://arongranberg.com/astar/docs/getstarted.html

![alt text](https://github.com/carodak/LovelyAgents/blob/master/game.png)

Note: As I wanted to get back into good programming habits, I refactored the agents code (GameAgent, Traveller, Wanderer SocialAgent) to have inheritance, polymorphism and to split the code into smaller methods. However, there would still be some work to do on this side (better encapsulation, assertions, follow the MVC model, perform unit tests). I would apply these good habits to my new projects.
