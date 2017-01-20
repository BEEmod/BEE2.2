doorQueue <- [];
isWaiting <- false;
currentDoor <- -1;
function entry(door)
{
printl("Adding entry door \"" + door + "\"");
doorQueue.append(door);
}

function think()
{
if (isWaiting || currentDoor + 1 == doorQueue.len())
	return;
else
	{
	currentDoor += 1;
	isWaiting = true;
	EntFire(doorQueue[currentDoor] + "_partner_rl", 				"Trigger","",0.0); // trigger exit door
	}
}

function exit(door)
{
printl("Reply recieved from \"" + door + "\"");
printl("Exchanging names with \"" + doorQueue[currentDoor] + "\"");
EntFire(door, "SetPartner", doorQueue[currentDoor], 0.0 );
EntFire(doorQueue[currentDoor], "SetPartner", door, 0.0 );
EntFire(door + "_active", "SetValue", 1, 0.0 );
EntFire(doorQueue[currentDoor] + "_active", "SetValue", 1,0.0 );
isWaiting = false;
}