# Shootin Zombies
 
For the project I had 5 scripts: 

playercontroller.cs:
This script handles most of the user interactions. This is where the control input goes, where walking is calculated, and 
the crosshair is also in this file

GameRules.cs:
This script handled the zombie spawns. It would roll for a random tile in the playable area and would check if there's a 
structure with walls that you can walk into. It would make sure the zombies don't spawn in the walls and that that they 
wouldn't spawn on top of each other.

UiManager.cs:
This script handles the user interface. Because of that it also is the "end of game" checker. It basically just checks that 
there are no zombies left or that the player has 0 health left.

bulletScript.cs:
This script basically sets the kunai speed and handles when it hits a zombie or a wall

ZombieScript.cs:
This script takes the positions of itself and the player and uses those to calculate the angle that the zombie shoudl move.


Overview:
In this gameplay video I used my xbox controller instead of my mouse.

From the one-pager, there were 3 big things that I didn't have time to get to.
First was the fact that there's only one character. I wanted to have another character ready to go and I 
would definitely be able to if I had another day. But I wanted to be sure I got the project done in time.
Secondly was the waves system. I didn't implement this yet.
And lastly was the visual novel style cutscenes, this one was going to be the biggest struggle out of the three.
Other than that I was able to get everything else working as expected.

I used a lot of publicly available art assets. One of the reasons I switch the throwing kunai instead of guns
is because the sprite genereator I found online didn't have any section to add guns. So instead of having him 
throw bullets I had him switch to kunai.

I came up with the crosshair when I was putting the project together because when I was walking around it was 
hard to tell if I was aiming in the right direction.

