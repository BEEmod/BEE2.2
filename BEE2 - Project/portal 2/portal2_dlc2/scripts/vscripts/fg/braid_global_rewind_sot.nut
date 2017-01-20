//Braid Cubes! And more Braid Cubes!

rewindCounter <- 60; //How many steps to go back.

::button_size <- 64.0; //Used for button detections.

::braid_cubes <- [];
::sot_particles_spawner <- Entities.FindByName(null,"@sot_particles_spawner");

function Rewind(){	
	foreach(cube in braid_cubes){
		EntFireByHandle(cube,"RunScriptCode","SetBack("+rewindCounter+")",0.01,null,null); //Set each cube back
	}
	
	printl("BRAID: Shadows of Time Rewind!");
	EntFire("@sot_fade","Fade","",0.0,null);
	EntFire("@sot_fade_2","Fade","",10.0,null);
	EntFire("@sot_sound","PlaySound","",0.0,null);
	
	EntFireByHandle(GetPlayer(),"RunScriptCode","fork();",0.02,null,null); //Set up the fork...
	EntFireByHandle(::braid_fork_guy,"RunScriptCode","fork();",0.03,null,null); //...then do it!
}
