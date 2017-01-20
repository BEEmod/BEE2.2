//Braid Cubes!

numCaptures <- 60; //How many captures to save
timeFlow <- 0.5; //How long between captures
pastPlaces <- [];
pastAngles <- [];
disabled <- 0;
immunity <- 0;

function ClearCaptures(){
	printl("BRAID: Clearing Memories");
	pastPlaces = array(numCaptures,self.GetOrigin());
	pastAngles = array(numCaptures,self.GetAngles());
}

function AddToList(){
	if(disabled > 0){
		printl("BRAID: "+self.GetName()+ " (" + self.GetClassname() + ") NOT Captured!");
	}else{
		
		pastPlaces[numCaptures-1] = self.GetOrigin();
		pastAngles[numCaptures-1] = self.GetAngles();
		
		for(local i = 0; i < numCaptures - 1; i++){
			pastPlaces[i] = pastPlaces[i+1];
			pastAngles[i] = pastAngles[i+1];
		}
		
//		printl("BRAID: "+self.GetName()+ " (" + self.GetClassname() + ") Captured!");
	}
	EntFireByHandle(self,"RunScriptCode","AddToList();",timeFlow,null,null); //Schedule next step
}

function SetBack(captures){
	if(immunity > 0){
		printl("BRAID: Resisted Time Shift");
		return;
	}
	printl("BRAID: Teleporting back "+captures+" timecaptures");
	if(captures > numCaptures) return;
	
	local button = Entities.FindByClassnameWithin(null,"prop_floor_button",self.GetOrigin(),::button_size); //The player should unpress a floorbutton when they rewind off it, right?
	if(button != null){
		EntFireByHandle(button,"PressOut","",0.01,null,null);
		printl("    BraidObj Pressed Button Out - Moved Back!");
	}
	
	self.SetOrigin(pastPlaces[numCaptures-captures]);
	self.SetAngles(pastAngles[numCaptures-captures].x,pastAngles[numCaptures-captures].y,pastAngles[numCaptures-captures].z);
}

function AddImmunity(){
	immunity++;
	EntFire(self.GetName()+"_counter","Add","1",0,null);
}
function SubtractImmunity(){
	immunity--;
	EntFire(self.GetName()+"_counter","Subtract","1",0,null);
}
function AddImmunityPlayer(){
	immunity++;
	EntFire("@player_immunity_counter","Add","1",0,null);
}
function SubtractImmunityPlayer(){
	immunity--;
	EntFire("@player_immunity_counter","Subtract","1",0,null);
}
function Enable(){
	disabled--;
	printl("BRAID: "+self.GetName()+ " (" + self.GetClassname() + ") Re-Enabled ("+disabled+")!");
}
function Disable(){
	disabled++;
	printl("BRAID: "+self.GetName()+ " (" + self.GetClassname() + ") Disabled ("+disabled+")!");
}

function fork(){
	::braid_fork <- [];
	for(local i = 0; i < numCaptures; i++){
		::braid_fork.append(pastPlaces[i]);
	}
}

ClearCaptures();
::braid_cubes.append(self);

printl("BRAID: Object " + self.GetName() + " (" + self.GetClassname() + ") Ready!");

AddToList();