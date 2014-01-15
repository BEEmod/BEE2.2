//
//  vscripts/fg/portal_deflection_cube.nut
//  ________________________________________________________________________
// 
//  Support script for "Deflector Cube" testing element
//  Use with portal_deflection_cube instances.
//
//  logic_script EntityGroup entries:
//      0:  first env_laser
//      1:  target of first env_laser
//  (same as Sendificator, on which this is based)
//
//  ________________________________________________________________________
//


//  ________________________________________________________________________
//
//                            Static symbols
//  ________________________________________________________________________
//


// Constants

beat_time <- 0.01;
// time between tracing steps; allows game state to update

cube_distance <- 24;
target_distance <- 16384;
// Distance of various things from the beam source

teleport_dest_offset <- 32;
// How far to move back from the final hit point to get the teleport location.

cube_size <- 19;   // size of cube for tracing (from origin)
cube_front <- 21;  // size of cube front; needs to be larger because of lens
cube_bbox <- 26;   // max bounding box around cubes
                   // (pre-rotated, for optimising cube tracing)

cube_horz_cutoff <- sin(5.5 / 180.0 * PI);
// Angle below which the outgoing beam is clamped to XY plane.
// (Prevents inaccuracies due to cube physics; also used by 'real' laser.)


// Globals

::source_offset <- Vector(24, 0, 0); // offset from laser source entity
// set by the map:
// ::starting_source = emitter to use for next trace


// static variables

// arrays of env_lasers, corresponding targets and env_sprites
lasers <- [];
targets <- [];
sprites <- [];

pathtracks <- []; //The path_tracks for the new portaly effect.

// laser_arrays: number of available env_lasers; set by initialise()
// index: current tracing step (must be lower than laser_arrays)
// hit: status for tracing; 0 = nothing, 1 = cube, 2 = portal
// current_pos: current scan position
// current_dir: current scan direction (must be unit length)

field_search_area <- 32;

portalgun_size <- 32;


//  ________________________________________________________________________
//
//                         Angle and vector math
//  ________________________________________________________________________
//


function vector_length(v)
{
    // Return the length of a vector.
    return sqrt(pow(v.x, 2) + pow(v.y, 2) + pow(v.z, 2));
}


function vector_mult(v, f)
{
    // Multiply all components of v by f and return the result.
    return Vector(v.x * f, v.y * f, v.z * f);
}


function vector_resize(v, f)
{
    // Return a vector colinear to v with length f.
    local len = vector_length(v);
    return vector_mult(v, f / len);
}


function make_rot_matrix(angles)
{
    // Return a 3x3 rotation matrix for the given pitch-yaw-roll angles.
    // (letters are swapped to get roll-yaw-pitch)
    //
    // format: / a b c \
    //         | d e f |
    //         \ g h k /

    local sin_x = sin(-angles.z / 180 * PI);
    local cos_x = cos(-angles.z / 180 * PI);
    local sin_y = sin(-angles.x / 180 * PI);
    local cos_y = cos(-angles.x / 180 * PI);
    local sin_z = sin(-angles.y / 180 * PI);
    local cos_z = cos(-angles.y / 180 * PI);

    return { a = cos_y * cos_z,                           b = cos_y * -sin_z,                           c = sin_y,
             d = -sin_x * -sin_y * cos_z + cos_x * sin_z, e = -sin_x * -sin_y * -sin_z + cos_x * cos_z, f = -sin_x * cos_y,
             g = cos_x * -sin_y * cos_z + sin_x * sin_z,  h = cos_x * -sin_y * -sin_z + sin_x * cos_z,  k = cos_x * cos_y,
           };
}


function rotate(point, angles)
{
    // Rotate point about the origin by angles and return the result.

    local mx = make_rot_matrix(angles);
    return Vector(point.x * mx.a + point.y * mx.d + point.z * mx.g,
                  point.x * mx.b + point.y * mx.e + point.z * mx.h,
                  point.x * mx.c + point.y * mx.f + point.z * mx.k);
}


function unrotate(point, angles)
{
    // Rotate point about the origin by angles in the opposite direction
    // and return the result.
    //
    // This is very straightforward, as the inverse of the rotation
    // matrix is the original one with the rows and columns swapped.

    local mx = make_rot_matrix(angles);
    return Vector(point.x * mx.a + point.y * mx.b + point.z * mx.c,
                  point.x * mx.d + point.y * mx.e + point.z * mx.f,
                  point.x * mx.g + point.y * mx.h + point.z * mx.k);
}


//  ________________________________________________________________________
//
//                              Initialisation
//  ________________________________________________________________________
//


function initialize(n)
{
    // Initialise map entities.  Called 1 second after the map loads.
    // n is the number of lasers in the map template.

    laser_arrays <- 0;
    local origin = self.GetOrigin();
    local tvec = EntityGroup[1].GetOrigin() - origin;
    local lvec = EntityGroup[0].GetOrigin() - (origin + tvec);
    while(laser_arrays < n) {
        // Add a laser and its target and sprite to the lists.
        // The sprite will be at the location of the target since
        // nothing is blocking it.
        local tloc = origin + vector_mult(tvec, laser_arrays + 1);
        lasers.append(Entities.FindByClassnameNearest(
                "env_laser", tloc + lvec, 1));
        targets.append(Entities.FindByClassnameNearest(
                "info_target", tloc, 1));
        local sprite = Entities.FindByClassnameNearest(
                "env_sprite", tloc, 1);
        sprites.append(sprite);
        EntFireByHandle(sprite, "AddOutput",
                        "targetname @portal_deflect_x_sprite" + laser_arrays,
                        0, null, null);
        ++laser_arrays;
		printl("Lasers Found: " + laser_arrays + " of " + n);
    };
	
	default_laser_origin <- origin;
	
//	new_initialize();
	
    reset_lasers();
}

function findPlayerPortalgun(){
	local ent = Entities.First();
	while(ent != null){
		if(ent.GetModelName().find("v_portalgun") != null){
			return ent;
		}
		ent = Entities.Next(ent);
	}
	printl("Error: No Portalgun!");
	return null;
}

function new_initialize(){
	// Unlike most of the other stuff, I wrote all this myself!
	local i;
	for(i = 0; true; i++){
		local ent = Entities.FindByTarget(null,"@portal_deflect_path_track_"+i);
		if(ent == null) break;
		pathtracks.append(ent);
	}
	n_of_pathtracks <- i;
	index <- 1;
//	local portalgun = findPlayerPortalgun();
//	pathtracks[0].SetOrigin(Entities.FindByTarget(null,"player").GetOrigin());
//	EntFireByHandle(pathtracks[0],"SetParent","player",0.0,null,null);
//	printl("DEBUG: Portalgun Name is " + portalgun.GetName());
//	printl("DEBUG: Pathtrack Parent Name is " + pathtracks[0].GetMoveParent.GetName());
//	EntFireByHandle(pathtracks[0],"SetParentAttachment","muzzle",0.2,null,null);
}


//  ________________________________________________________________________
//
//                            Entity operations
//  ________________________________________________________________________
//


function reset_lasers(){
    // Turn off all lasers and reset the index number
    EntFire("@portal_deflect_x_beam*", "TurnOff", "", 0, null);
	for(local i = 0; i < laser_arrays; i++){ //Move everything back to the center.
		lasers[0].SetOrigin(default_laser_origin);
		targets[0].SetOrigin(default_laser_origin);
		sprites[0].SetOrigin(default_laser_origin);
	}
//	for(local i = 0; i < n_of_pathtracks; i++){
//		pathtracks[0].SetOrigin(default_laser_origin);
//	}
    index <- 1;
	printl("Lasers Reset");
}


function place_laser(){
    // Place the lasers[index] at current_pos, facing current_dir
    // and turn it on, with the appropriate color.
    lasers[index-1].SetOrigin(current_pos);
    targets[index-1].SetOrigin(current_pos + current_dir * target_distance);
	EntFireByHandle(lasers[index-1], "Color", current_laser_color, 0, null, null);
    EntFireByHandle(sprites[index-1], "Color", current_laser_color, 0, null, null);
    EntFireByHandle(lasers[index-1], "TurnOn", "", 0, null, null);
    EntFireByHandle(lasers[index-1], "TurnOff", "", 0.49, null, null);
	printl("Laser " + (index-1) + " Placed");
}


function place_effect(){
	//This part I also wrote. It deals with the pathtracks, not the clipping detection.
	pathtracks[index].SetOrigin(current_pos); //Nice and simple, since I use path_tracks.
	// The problem is that they can't check the clipping like env_lasers!
	printl("Pathtrack " + index + " Placed");
}


function schedule_call(code, delay){
    // Set an event to start the next operation after delay seconds.
	// (Not always 0.01 seconds as in the Sendificator)
    EntFireByHandle(self, "RunScriptCode", code, delay, null, null);
}


function find_cube(prev, center){ // Rewritten by me to use targetname filtering instead of modelname, and a center.
    // Iterate over all the cubes in the map.

    local cube = Entities.FindInSphere(prev,center,18.0);
	if(cube != null && cube.GetName().find("portal_deflection_cube") == null){ //If it's not marked as a deflection cube...
		return find_cube(cube, center); //...then recurse.
	}
	
	if(cube != null){
		printl("Deflection Cube Found!");
	}else{
		printl("No More Deflection Cubes Found");
	}
    return cube;
}

function find_field(prev, center){ // Rewritten by me to use targetname filtering instead of modelname, and a center.
    // Iterate over all the reflection fields in the map.

    local field = Entities.FindInSphere(prev, center, field_search_area);
	if(field != null && field.GetName().find("portal_reflection_field") == null){ //If it's not marked as a reflection field...
		return find_field(field, center); //...then recurse.
	}
	
	if(field != null){
		printl("Reflection Field Found!");
	}else{
		printl("No More Reflection Fields Found");
	}
	
    return field;
}

//  ________________________________________________________________________
//
//                              Beam tracing
//  ________________________________________________________________________
//


function trace_start(portal){
    // Start a beam trace.
	
	if(portal == 2){
		current_laser_color <- "255 200 0";
	}else{
		current_laser_color <- "63 127 255";
	}
	
	EntFire("@fizzler_blocker","Enable","",0.0,null);
	
    local source_rot = ::starting_source.GetAngles();
    current_pos <- ::starting_source.GetOrigin() +
        rotate(::source_offset, source_rot);
    current_dir <- rotate(Vector(1, 0, 0), source_rot);
	portalgun_dir <- source_rot;
	
    reset_lasers();
    place_laser();
    schedule_call("trace("+portal+");",0.5);
	printl("Started Tracing");
	
//	EntFireByHandle(pathtracks[0],"SetParent",::starting_source.GetName(),0.0,null,null);
//	pathtracks[0].SetOrigin(::starting_source.GetOrigin());
	
	prev_pos <- current_pos;
}


function trace(portal){
    // Find out what the current laser hits and take the appropriate action.
    // Abort if index has reached laser_arrays
	
	prev_pos <- current_pos;
    current_pos = sprites[index-1].GetOrigin();
	
    if(trace_cubes()){
		printl("Hit a Deflectocube!");
    //  if(index >= laser_arrays - 1){
	//		printl("Uh-oh...out of lasers!");
    //      return;
	//	}
		index = 1;
        place_laser();
	//	place_effect();
        schedule_call("trace("+portal+");",0.5);
    } else {
		if(trace_fields()){ //If we hit a reflective field...
			printl("Hit a Portal Reflection Field!");
			if(++index >= laser_arrays - 1){
				printl("Uh-oh...out of lasers!");
				return;
			}
			place_laser();
	//		place_effect();
			schedule_call("trace("+portal+");",beat_time);
		}else{
			// no more redirections; confirm the final position
			printl("Hit something non-cube-like!");
			//Place the final pathtrack
	//		place_effect();
			dest_confirm(portal, current_pos - current_dir * portalgun_size, portalgun_dir);
		}
    }
}


function trace_cubes(){ //Mine! Much more minimal, since I'm not using lasers. I test collisions with a sphere, not a cube, which makes it faster.
	local cubey = find_cube(null,current_pos);
	if(cubey == null) return false;
	
	current_pos = cubey.GetOrigin() + rotate(Vector(cube_distance,0,0),cubey.GetAngles());
	current_dir = rotate(Vector(1,0,0),cubey.GetAngles());
	portalgun_dir = cubey.GetAngles();
	EntFireByHandle(cubey,"Color",current_laser_color,0.0,null,null);
	EntFireByHandle(cubey,"Color","255 255 255",0.5,null,null);
	return true;
}

function trace_fields(){
	local cubey = find_field(null,current_pos);
	if(cubey == null) return false;
	
	current_dir = cubey.GetForwardVector() + (cubey.GetForwardVector() - current_dir); //Reflect!
	current_pos -= (cubey.GetOrigin() - current_pos); //Move it a bit farther away, just in case.
//	portalgun_dir = ???;
	return true;
}


//  ________________________________________________________________________
//
//                         Portal Redirection Effect
//  ________________________________________________________________________
//


function dest_confirm(portal, location, direction){
    // Completely rewritten by me. I don't need to send stuff, I need portals.
	printl("Figured out path!");
	
	EntFire("@fizzler_blocker","Disable","",0.0,null);
	
	EntFire("@portal_deflect_fx_blue_part","Start","",0.0,null);
	EntFire("@portal_deflect_fx_blue","StartForward","",0.0,null);
	
	local cube_portalgun = Entities.FindByName(null,"@portal_deflect_x_portalgun");
	cube_portalgun.SetOrigin(location);
	printl("Direction for Portalgun: "+direction.x+" "+direction.y+" "+direction.z);
	cube_portalgun.SetAngles(direction.x, direction.y, direction.z);
	EntFireByHandle(cube_portalgun,"FirePortal"+portal,"",0.0,null,null);
	
	EntFire("@portal_deflect_x_blocker","Disable","",0.0,null);
	
	schedule_call("Entities.FindByName(null,\"@portal_deflect_x_portalgun\").SetOrigin(default_laser_origin);",0.5);
	schedule_call("reset_lasers();",0.5);
}
