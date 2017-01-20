//Shadows...oooh...spoooky...

timeFlow <- 0.5; //Time between position updates
defaultPos <- Vector(0,-1024,0);

function vector_length(v){
    // Return the length of a vector.
    return sqrt(pow(v.x, 2) + pow(v.y, 2) + pow(v.z, 2));
}

playerWasNear <- false;

function fork(){
	if(::braid_fork.len() <= 0){
		self.SetOrigin(defaultPos);
	//	EntFire("@sot_fade","FadeReverse","",0.0,null);
		EntFire("@sot_sound","PlaySound","",0.0,null);
		return;
	}
	
	self.SetOrigin(::braid_fork[0]);
	self.SetAngles(0,180+GetPlayer().GetAngles().y,0);
	EntFireByHandle(::sot_particles_spawner,"ForceSpawn","",timeFlow,null,null); //Effects! Yay!
	
	local button = Entities.FindByClassnameWithin(null,"prop_floor_button",self.GetOrigin(),::button_size);
	if(button != null){
	//	printl("BRAID: Shadow Found Button!");
		
		if(vector_length(GetPlayer().GetOrigin()-self.GetOrigin()) > ::button_size){
			
			if(::braid_fork.len() > 1){
				if(vector_length(::braid_fork[1] - ::braid_fork[0]) > ::button_size){
					EntFireByHandle(button,"PressOut","",timeFlow-0.01,null,null);
					printl("    Pressed Button Out!");
				}
			}else{
				EntFireByHandle(button,"PressOut","",timeFlow-0.01,null,null);
				printl("    Pressed Button Out (to)!");
			}
			
			if(vector_length(temp - ::braid_fork[0]) > ::button_size || playerWasNear){ //The second term catches the case where the player just walked off the button, leaving the Shadow there.
				EntFireByHandle(button,"PressIn","",0.0,null,null);
				printl("    Pressed Button In!");
			}
			
			playerWasNear <- false;
		
		}else{
			
			playerWasNear <- true;
			
		}
	}
	temp <- ::braid_fork[0];
	::braid_fork.remove(0);
	EntFireByHandle(self,"RunScriptCode","fork();",timeFlow,null,null); //Schedule next step
}

::braid_fork_guy <- self;
printl("BRAID: Shadow Ready!");