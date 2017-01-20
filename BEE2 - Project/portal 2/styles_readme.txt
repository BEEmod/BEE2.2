Carl Kenner's Puzzle Maker Styles Mod for Portal 2 - v1.08 BETA
Copyright (c) Carl Kenner, 2012. 
Also supports mods by Benjamin Blodgett, August Loolam, and HMW.
Playtested Certified logo drawn by Geneosis.
Some parts are by Valve.

Not finished yet! Work in progress!

This mod allows you to choose different styles for any maps you make in Portal 2's Puzzle Maker:
1950s, 1960s, 1970s, 1980s, Overgrown, Portal 1, Behind The Scenes, Evil Wheatley, and Clean.
And it can instantly convert any existing map you made in Puzzle Maker into a new style.
It adds Reflection Gel and Paint Fizzlers, and can be used with BEEMOD or HMW's mod to add many new items.
It also lets you use screenshots as the thumbnails for your map.
And it adds an icon to show that you playtested your map before publishing.

Installation instructions (for PC):
***********************************
0.  Exit Portal 2 (obviously)
1.  Remove Fudge Packer, or any entity limit breaking mods, if you have installed them.
    Don't worry, this mod can also pack files, understands fudgepacker commands, and removes the entity limit.
	You can remove them (only if they are installed!!) by removing vbsp.exe and renaming _vbsp.exe (or whatever it is called) to vbsp.exe 
2.  If you have modified your editoritems.txt file, back it up and give it a name like "editoritems_mychanges.txt"
3.  Extract this zip file to your "Portal 2" folder, eg. "C:\Program Files (x86)\Steam\steamapps\common\portal 2".
    Replace any existing files if it asks you
4.  Find your portal 2 folder, and run StyleChanger.exe.
    Choose any style, and press OK.
    You can make a shortcut to the StyleChanger.exe file on your desktop by holding down the ALT key while dragging it onto your desktop.
5.  (optional) Install the latest BEE Mod from here: 
    http://portal2backstock.com/bee
6.  (optional) Install HMW's Mod from here: 
    http://forums.thinkingwithportals.com/puzzle-creator/puzzle-creator-tech-info-hacks-and-laughs-t6168-75.html
	You may need to install WinRar to extract it: http://www.win-rar.com/start.html?&L=0
	The CONTENTS (not the folder itself) of the hmw_pti_items0 folder need to go in your "Portal 2" folder.
7.  IF you installed those other mods, run StyleChanger.exe AGAIN,
    and choose the style that you want and press OK.
	
Installation instructions (for Mac):
************************************
0.  Exit Portal 2 (obviously)
1.  If you have modified your editoritems.txt file, back it up and give it a name like "editoritems_mychanges.txt"
2.  Make a copy of the original unmodified "portal 2/portal2_dlc2/scripts/editoritems.txt" file and call it "editoritems_original.txt"
3.  Extract this zip file to your "Portal 2" folder, maybe it's in "Steam/steamapps/common/portal 2", but I don't have a Mac, so I don't know.
    Replace any existing files if it asks you.
4.  (optional) Install the latest BEE Mod "manual installation files" from here:
    http://portal2backstock.com/bee
    To extract RAR files, you may need to install WinRAR (ignore it's name): http://www.win-rar.com/start.html?&L=0
	Extract the RAR file to the folder that your "portal 2" folder is in, NOT your "portal 2" folder,
	eg. "Steam/steamapps/common/", so that the "portal 2" folder in the RAR file will merge with your "portal 2" folder.
	Replace any existing files.
5. (optional) Install HMW's Mod from here: 
    http://forums.thinkingwithportals.com/puzzle-creator/puzzle-creator-tech-info-hacks-and-laughs-t6168-75.html
	WinRAR (above) can extract 7zip files too.
	The CONTENTS of the hmw_pti_items0 folder inside the 7zip file need to go in your "Portal 2" folder.
6.  Go to your "portal 2/portal2_dlc2/scripts" folder, choose one of my editoritems_*.txt files, 
    and COPY it to replace your existing editoritems.txt file.
	Maybe you can do it by opening it in a text editor program and saving it as that name? I don't have a Mac, so I don't know.

Making Better Maps:
****************
Remember! Making good maps isn't just about using a fancy style and new elements.
You can learn how to make good maps by subscribing to The Portal2Cast:
http://www.youtube.com/user/portal2cast/videos
It reviews maps chosen at random, and teaches you how to avoid the common
beginner mistakes.

Automatic Map Elements:
***********************
Maps you make will automatically have entrance and exit elevators and rooms
for that style. There are 7 different entrance rooms (only 1 for coop) and 
4 exit rooms (4 for coop too), which puzzle maker chooses at random. They are
saved and will stay the same whever you preview or publish that same map. I
don't know how to change manually which rooms are used. The elevator is always
the same, only the corridor before or after it changes.
 
But in my mod, some elements in the entrance elevators or rooms will change,
depending on the contents of your map. Different gel pipes will go into the
building depending on which gels are used. Vents will go into the building if
diversity vents are used. In the entrance rooms, the signs on the walls will
change depending on what's in the test. Repulsion gel signs will only be there
if there is repulsion gel in the test, safety signs will only be there if the
test is deadly, chemical hazard signs will only be there if there's asbestos
funnels, etc.

Most of the styles have extra voices, in addition to Cave Prime.
The voices will also change what they say depending on what items are in the 
puzzle. Cave may talk about asbestos if there are asbestos funnels, lasers if
there are lasers, safety if it is deadly, gel if it is there, glass if there's
glass, etc. The same with GLaDOS, Wheatley, or the Announcer.

Some parts of the entrance, exit, or chamber will change depending on the where
the player is up to in the PeTI storyline. If there is a sentient cloud, there
will be no security cameras (not implemented yet). If Cave Johnson is not in charge of aperture, 
there won't be portraits of him, or extra Cave lines.

Usage instructions (for PC):
****************************
New Keys
========
  Ctrl+F9 = preview with elevators, or export to hammer (but hold down Ctrl a few secs after)
  F5 = take a screenshot to use as a thumbnail
  Ctrl or Shift at the start of the lighting step = full lighting in preview

Changing styles
===============
  Whenever you want to change styles, just run the StyleChanger.exe file, 
  choose a new style from the list, and press OK. It won't give you a 
  confirmation message, it will just exit, but that means it worked.
  
  If Portal 2 is still running, it will tell you to exit and restart Portal 2, 
  so do that, otherwise your map will be half one style and half another.
  
  The styles with BEE or HWM items have more items, so it's good to use them,
  if you have them installed, and if you don't mind it being slightly harder 
  to choose some items. There was no room in most of the BEE items styles for
  me to add Reflection Gel, so use the normal or HMW versions instead if you
  want Reflection Gel, or use the "1960s with BEE items" style which doesn't
  have warm lights.
  
  If your map uses items that aren't included in the toolbar for that style,
  those items will still work. So you can make a map in "1950s with HMW items",
  add some HMW items, then change the style to "1950s with BEE items", and add
  some BEE items, then your map will have both kinds of items! You can use that
  trick if you need Reflection Gel, Mashy Spike Plates, and Portal Magnets all
  in the same level, for example. You can even duplicate items from a different
  set by using Ctrl+dragging. But the default BEE, HMW, and Valve styles don't 
  support that, because they weren't made by me.
  
  Go into Puzzle Maker and load an existing map or create a new one.
  It will always look like Clean style in the editor, so don't worry.
  It will look correct when you test or publish it.

  If you used the 1950s style and loaded an existing map, there's a small 
  chance you might get a conflict with a button and something under it. 
  That's because 1950s buttons go down into the floor. Use the compatability
  style instead if you have that problem. Or just move the button.
  
Previewing / Testing
====================
  Make your map, then preview it by pressing F9 or clicking the play button.
  
  My mod will compile the lighting step much faster when previewing, 
  but still slow when publishing, to make testing quicker. The lighting will
  be darker and won't look quite as good in the preview, but will look better
  when you publish it. To test with full lighting, just hold down either CTRL
  or SHIFT just before it starts the lighting step, then release it after it
  starts the lighting step.
  
  In the preview you will see your map in the chosen style. 
  The style includes full elevators and entrance rooms,
  but the preview starts from just in front of the door, 
  and restarts at the exit, unless you press Ctrl+F9 
  (and hold down ctrl a few secs after) to test with elevators.
  The published map will always have full elevators and won't restart.
  
  If the walls, floor, ceiling, and indicator lines are in the wrong style,
  it's because Steam updated Portal 2 and replaced my vbsp.exe and vrad.exe. 
  Exit Portal 2, and run my StyleChanger.exe again to automatically fix it.
  If EVERYTHING has gone back to normal style, it's because Steam updated my
  editoritems.txt file. Copy the new editoritems.txt file to editoritems_original.txt
  and run my StyleChanger.exe again and choose a style to automatically fix it.
  If your Reflection Gel is now making purple splashes instead of grey ones, it's
  because Steam has updated my particles_manifest.txt file. Copy your
  "portal 2/portal2/particles/styles_particles.txt" file to "particles_manifest.txt"
  and replace the existing file. Then restart Portal 2.
  
  In MY styles, the preview will automatically take a screenshot when you
  enter the room. It will be used as the thumbnail when publishing. You can
  also press F5 at any time to take another screenshot to use instead.
  When you complete the map during testing, it will modify the screenshot to 
  include the Playtested Certified logo in the bottom right corner. 
  This logo guarantees a map is possible to win. Don't use cheats to get
  screenshots or the logo will be a lie!
  If you re-enter the chamber after completing it, you may loose the logo
  and get a new screenshot, and have to complete it again to get it back.
  If you use Ctrl+F9, it has no preview code, so you must press F5 manually
  when you want a screenshot.
 
  In the default BEE, HMW, or Valve styles (but not in my versions) you can use F5 
  to manually take a screenshot to use when publishing. You must do that if you
  previewed a different map in one of my styles within the last 2 hours, 
  otherwise it will use the old screenshot from the other map. If there hasn't
  been any screenshot in the last 2 hours, maps will be published with the
  Puzzle Maker map as the thumbnail.

  Automatic preview screenshots are never deleted, so you might want to delete 
  them manually. They are in either "portal 2\portal2\screenshots" or
  "portal 2\portal2_sixense\screenshots".   
  
Publishing
========== 
  Maps are published exactly like normal. But you should test them first, to
  automatically use an in-game screenshot as the thumbnail when publishing.
  You should test the map, and complete it, within 2 hours before you publish.
  
  Please use the #StyleMod tag somewhere in your map description, so people
  can search for styled maps easily. And you are welcome to mention me and my 
  mod and tell people where to get it. 
  
  Any additional files (for the Reflection Gel, High Energy Pellet, or
  Unstationary Scaffold) will be packed automatically into your map, so the
  player won't need to download anything. The reflection gel splash particle
  effects are impossible to pack in Portal 2, so players will see them as
  purple instead of grey if they don't have my mod installed. But the purple
  splashes still look OK, and the gel itself will still look grey when falling.
  
  Published maps will be part of the PeTI storyline and still feature 
  Cave Prime, even if they also have other voices as part of the style.
  
New Items
========= 
  Depending on your style, you may see new items in the toolbar.
  Some existing items are merged into one icon, eg. all cubes, 
  so add one then right click it to change it's type.
  For new items, change their type by adding them, then right clicking, 
  changing Button Type, then looking at it in the editor to see what it is. 
  
  To add Reflection Gel, choose it from the toolbar and place it on the floor.
  Don't change, or even view, it's Gel type or you may crash the editor.
  You can change it's flow type and all other settings though.
  You can duplicate it by Ctrl-dragging it's Paint Splat, NOT THE DROPPER.
  
  My Reflection Gel will look grey and shiny in the game, not glitchy white.
  It's splash particle effects will look purple if players don't have my mod,
  but will look correctly grey if they do, it's only a minor problem.
  On the floor, it will look exactly the same shade as white gel, but players
  probably won't notice unless you use both gels close together. In your face,
  and on unpaintable props like doors or security cameras, and light bridges,
  it will look orange.
  
  It's called Weak Reflection Gel, because it only reflects lasers 
  (and any wall reflects pellets and spheres). 
  The original Reflection Gel was supposed to reflect hard light bridges
  and excursion funnels too, but they never implemented that. 
  You need either angled panels or redirection cubes to use reflection gel.
  It's also slightly sticky, and will stop painted cubes from sliding on 
  orange gel, but please don't use that effect, it's a left-over from purple
  Adhesion (sticky, wallwalking) Gel. I'll make a purple sticky gel for you
  later.
  
  To add Paint Fizzlers, choose Fizzler from the toolbar, and place it.
  Then right-click it and set it to "Start Reversed". 
  A normal fizzler destroys portals and objects but lets through paint.
  The reverse fizzler destroys paint and lets through everything else.
  It will look the same in the editor, but different in the game.
  Paint fizzlers show up as a red incandescent particle field. 
  The particles might not reach the middle if you make it too wide,
  but it still fizzles paint along it's whole length.
  Paint that is set to Allow Streaks can streak underneath it a bit though.
  
  To add Laser Death Fields, choose Fizzler from the toolbar, and place it.
  Then right-click it and set it's Hazard Type to Laserfield.
  Setting it to Start Reversed has no effect yet, so don't do it.
  
  See the BEEMOD, or HMW Mod, documentation or discussions for more information
  about those items. I remade them in different styles, and they may have some
  slight changes though, so don't blame them if you have problems with a styled 
  version.
  
Hammer
======
  You don't need Hammer to use this mod!
  But Hammer is the official map editing program that Valve used to make
  all the official maps in Portal 2, Half Life 2, and their other games.
  It's very powerful, but very complicated and hard to learn. You can
  get it for free if you install Portal 2 Authoring Tools Beta from
  your steam library tools section.
  
  Because this mod replaces vbsp and vrad, it will also affect Hammer.
  The main difference is that it can automatically pack files into your
  .bsp file by using BspZip. Just add Fudge Packer commands to comments
  of entities in your map (but not the worldmap entity). There are also
  new commands that I implemented:
  
  packer_additem:materials/example/foobah.vmt;materials/example/foobah.vtf;sounds/example/um.wav
  
  packer_rename:materials/example/oldname.vmt;materials/example/newname.vmt
  
  packer_addfile:c:\myfiles\packfilelist.txt
  
  Those commands only work in the main .vmf file, not in instances.
  
  Don't name your map preview.vmf though, or vrad might compile it with bad 
  lighting. And don't specify the same entity limit puzzle maker uses as a 
  parameter to vbsp, or it will try to convert your Hammer map to another 
  style.
  
  To export a puzzlemaker map to Hammer, press Ctrl+F9 in Puzzle Maker.
  You don't have to wait for the visibility or lighting steps to finish.
  Then open "portal 2\sdk_content\maps\styled\preview.vmf" in Hammer.
  Then rename it to something else, and edit it however you want.
  Don't try to use the puzzlemaker_export command, it will only half
  change the style, because it won't run my vbsp.exe on it.
  You can also open your published maps by loading the other files in that
  "portal 2\sdk_content\maps\styled\" folder.
  
  Note that I use lots of FireUser1, and FireUser2 in my instances. For
  sounds, FireUser1 selects it and deactivates the alternatives, and 
  FireUser2 is to play them and any sound that comes after it. For items,
  FireUser1 turns them off, and FireUser2 or Trigger turns them on.
  
  You can use my instances in your own hammer maps, although because I
  optimised them, the interfaces can be a bit strange. Use the instances
  in the maps\instances\p2editor_* folders.
  
  You can also create your own instances and your own editoritems.txt files
  to make your own puzzle maker styles or items. The StyleChanger.exe will
  automatically detect any editoritems_*.txt files. You could then release
  your editoritems_*.txt files and your instances to other style changer mod 
  users. The hard part is making new models to display in the editor.
  
Usage instructions (for Mac):
*****************************
Because I, and other programmers, don't have Macs, and don't know anyone who 
does, we can't make programs for them, or test programs for them. Unless you
buy us a Mac.

Most features of this mod will still work on a Mac (in theory), but not the
features that require a modified vbsp or vrad program. It probably wouldn't be
too hard to implement on a Mac, I just can't do it without a Mac.

The main problem Mac users will have is that the wall, floor, and ceiling 
textures can't be changed from the default. The props will still be changed,
and the elevators, entrance, exit, and observation rooms will be totally 
changed. But the chamber itself will always have modern wall textures. I tried
to fix that a bit with a few decals here and there, but it's not as good.

Other problems are: Fizzler fields and indicator lines won't look like the 
50s/60s/70s versions (but the fizzler models and indicator panels will).
You can't add paint fizzlers. Bottomless pits will always have Goo. You can't
remove the entity limit. And you can't pack the reflection gel colour or the
High Energy Pellet files into your map (so players will need to install my mod
to play your maps with High Energy Pellets, and can optionally install it to
make Reflection Gel look better). And the lighting step of compiling won't be
sped up. And the Unstationary Scaffold in the Portal 1 style won't look quite
as good. And paint bombs will be able to paint a surface under glass.

Changing styles (Mac)
===============
  Whenever you want to change styles, just save a copy of one of the 
  "steam/steamapps/common/portal 2/portal2_dlc2/scripts/editoritems_*.txt" 
  files as "editoritems.txt", replacing the existing file. 
  Maybe you can do that in a text editor, or maybe you have to go to that 
  folder and copy and rename files, I don't know. 
  
  If Portal 2 is still running, you need to exit and restart Portal 2, 
  otherwise the style won't change at all.
  
  The styles with BEE or HWM items have more items, so it's good to use them,
  if you have them installed, and if you don't mind it being slightly harder 
  to choose some items. There was no room in most of the BEE items styles for
  me to add Reflection Gel, so use the normal or HMW versions instead if you
  want Reflection Gel, or use the "1960s with BEE items" style which doesn't
  have warm lights.
  
  If your map uses items that aren't included in the toolbar for that style,
  those items will still work. So you can make a map in "1950s with HMW items",
  add some HMW items, then change the style to "1950s with BEE items", and add
  some BEE items, then your map will have both kinds of items! You can use that
  trick if you need Reflection Gel, Mashy Spike Plates, and Portal Magnets all
  in the same level, for example. You can even duplicate items from a different
  set by using Ctrl+dragging. But the default BEE, HMW, and Valve styles don't 
  support that, because they weren't made by me.
  
  Go into Puzzle Maker and load an existing map or create a new one.
  It will always look like Clean style in the editor, so don't worry.
  It will look correct when you test or publish it.

  If you used the 1950s style and loaded an existing map, there's a small 
  chance you might get a conflict with a button and something under it. 
  That's because 1950s buttons go down into the floor. Use the compatability
  style instead if you have that problem. Or just move the button.
  
Previewing / Testing (Mac)
====================
  Make your map, then preview it by pressing F9 or clicking the play button.
  
  In the preview you will see your map in the chosen style. 
  The style includes full elevators and entrance rooms, but the preview 
  starts from just in front of the door, and restarts at the exit.
  The published map will always have full elevators and won't restart.
  In some styles you can turn around and walk backwards to see what your 
  entrance room and elevator will look like.
  
  If everything has gone back to normal style, it's because Steam updated my
  editoritems.txt file. Copy the new editoritems.txt file to editoritems_original.txt
  and save the style you was as editoritems.txt again to fix it.
  If your Reflection Gel is now making purple splashes instead of grey ones, it's
  because Steam has updated my particles_manifest.txt file. Copy your
  "portal 2/portal2/particles/styles_particles.txt" file to "particles_manifest.txt"
  and replace the existing file. Then restart Portal 2.
  
  You can press F5 to take a screenshot, and then manually upload it to your
  map's workshop page, so people can see how your map looks when they view that
  page. But you can't change the main thumbnail, unfortunately.
  
Publishing (Mac)
========== 
  Maps are published exactly like normal. But you should test them first to
  make sure they are completable.
  
  When publishing, your maps will use the puzzle-maker editor screen as the
  thumbnail. Unfortunately that means your map will look like any other PeTI
  map, so mention the style in the title and description.
  
  Please use the #StylesMod tag somewhere in your map description, so people
  can search for styled maps easily. That will help people find your styled
  map even if they can't tell it's style from the thumbnail.
  And you are welcome to mention me and my mod and tell people where to get it. 
  
  Any additional files (for the Reflection Gel, or High Energy Pellet) will 
  not be packed into your map, so if you use High Energy Pellets, you must
  mention in the description that the player must download my mod for your map
  to work.
  If you use Reflection Gel, it will look bright glitchy white while it is
  falling, and make purple splashes, if the player doesn't have my mod
  installed. There have been maps like that that made it to the front page of
  the workshop though (but without any splashes), so I guess it's OK. But you
  should still suggest in the description that they download my mod to see 
  the Reflection Gel in your map properly.
  
  Published maps will be part of the PeTI storyline and still feature 
  Cave Prime, even if they also have other voices as part of the style.
  
New Items (Mac)
========= 
  Depending on your style, you may see new items in the toolbar.
  Some existing items are merged into one icon, eg. all cubes, 
  so add one then right click it to change it's type.
  For new items, change their type by adding them, then right clicking, 
  changing Button Type, then looking at it in the editor to see what it is. 
  
  To add Reflection Gel, choose it from the toolbar and place it on the floor.
  Don't change, or even view, it's Gel type or you may crash the editor.
  You can change it's flow type and all other settings though.
  You can duplicate it by Ctrl-dragging it's Paint Splat, NOT THE DROPPER.
  
  My Reflection Gel will look glitchy white unless players have my mod.
  It's splash particle effects will look purple if players don't have my mod,
  but will look correctly grey if they do.
  On the floor, it will look exactly the same shade as white gel, but players
  probably won't notice unless you use both gels close together. In your face,
  and on unpaintable props like doors or security cameras, and light bridges,
  it will look orange.
  
  It's called Weak Reflection Gel, because it only reflects lasers 
  (and any wall reflects pellets and spheres). 
  The original Reflection Gel was supposed to reflect hard light bridges
  and excursion funnels too, but they never implemented that. 
  You need either angled panels or redirection cubes to use reflection gel.
  It's also slightly sticky, and will stop painted cubes from sliding on 
  orange gel, but please don't use that effect, it's a left-over from purple
  Adhesion (sticky, wallwalking) Gel. I'll make a purple sticky gel for you
  later.
  
  You can't add Paint Fizzlers on a Mac, so don't try. Sorry. 
  Setting a fizzler or laserfield to "Start Reversed" will do nothing. 
    
  To add Laser Death Fields, choose Fizzler from the toolbar, and place it.
  Then right-click it and set it's Hazard Type to Laserfield.
  
  See the BEEMOD, or HMW Mod, documentation or discussions for more information
  about those items. I remade them in different styles, and they may have some
  slight changes though, so don't blame them if you have problems with a styled 
  version.
  
Warning:
********
Use of this program, or anything else that requires electricity, will destroy 
the entire world's climate for the next few thousand years, unless you use 100%
green or nuclear energy. Seriously. And don't shoot the messenger, it's not my
fault.