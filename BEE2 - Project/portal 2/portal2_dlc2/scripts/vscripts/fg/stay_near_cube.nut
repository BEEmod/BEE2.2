/*
EntityGroup[0] = ui
EntityGroup[1] = cube
EntityGroup[2] = relay
*/

game_ui_offset <- Vector(0,0,20); //The offset, to prevent glitching.
disabled <- false;

function game_ui_think(){
	
	if(disabled == false){ //So that nothing happens when the player's holding it.
		
		if(EntityGroup[0].GetOrigin() != EntityGroup[1].GetOrigin()){
//			printl("[Moved]");
			EntityGroup[0].SetOrigin(EntityGroup[1].GetOrigin() - game_ui_offset);
		}
		EntFireByHandle(EntityGroup[0],"Activate","",0.0,GetPlayer(),GetPlayer());
//		EntFireByHandle(EntityGroup[2],"Disable","",0.19,null,null); //A little bit of mutexing on the particles.
//		EntFireByHandle(EntityGroup[2],"Enable","",0.21,null,null); //Prevents giving false positives to the players.
		
//		printl("THOUGHT");
		
	}
	
	EntFireByHandle(self,"RunScriptCode","game_ui_think();",0.2,null,null);
}
