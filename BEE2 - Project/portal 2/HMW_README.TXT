_______________________________________________________________________________

                         Portal 2 custom puzzle items

                                    by HMW
_______________________________________________________________________________


This is a collection of custom puzzle items for the Portal 2 puzzle maker.
These items do not use any custom assets (models, textures and the like), so it
should be safe to publish maps containing these items directly to the workshop.


                                  DISCLAIMER
_______________________________________________________________________________

The process of adding custom items to the Portal 2 editor is highly
experimental, and not supported by Valve.  If you mess up your Portal 2
installation, destroy your PC, or look stupid because you uploaded a faulty map
to the workshop, it's your own problem.

Please make backups of any important files (e.g. custom maps) before installing
this pack!


                                 Installation
_______________________________________________________________________________


To install, copy the "portal2_dlc2" and "sdk_content" folders to your Portal 2
folder, merging their contents with the existing folders.

Then go into portal2_dlc2/scripts and copy the text in editoritems_extend.txt
into editoritems.txt.  (This step is necessary to make the new elements visible
in the puzzle maker.)

If your editor configuration is rendered unusable, or if you want to undo any
changes to it, go into portal2_dlc2/scripts and delete editoritems.txt, then
verify your game cache.

The following is a list of the new elements included in this pack.


                                 Portal magnet
_______________________________________________________________________________


Replaces the cube button in the menu.  To place a cube button, place a generic
button and change its type via the right-click menu.

When a portal is placed near this item, it will snap to it.  This is often
useful for places where portals need to be placed fairly accurately from large
distances.  The puzzle editor inserts these automatically near certain test
elements (funnels, light bridges), but now you can place them manually.  They
can also be rotated if desired.

Known limitation: cannot be used on moving surfaces, as far as I'm aware.


                                  Save point
_______________________________________________________________________________


Replaces the ball button in the menu.  To place a ball button, place a generic
button and change its type via the right-click menu.

Saves the game when the player first enters the voxel containing this item.
Ideally, you would use this in hallways connecting different puzzles, or around
another test element that is a significant "sub-goal" in the puzzle.

Please make sure that the area around the save point is completely safe, so
that players cannot die right after saving their game, which would result in an
infinite "death-loop".


                              Yellow light source
_______________________________________________________________________________


Replaces the cleaning gel item on the menu.  To place cleaning gel, be
creative.

This is a square lamp set into the wall, that emits a warm yellow light.  Use
as an alternative for blue light strips, to provide some variation in the
lighting or to highlight points of interest in your map.  Can be combined with
blue light strips on the same surface.


                                  Logic gates
_______________________________________________________________________________


Replaces the laser field in the menu.  Fizzlers can be changed to laser fields
via the right-click menu.

Connect multiple buttons to this item, and then connect the item to what you
want to activate.  Its behaviour depends on its type; see below.

When you place this item, it will default to an AND gate, giving the same
result as connecting all buttons to the item directly.  By right-clicking and
choosing another "button type", you can change the function it performs.
(I can't change the menu labels, unfortunately.)

Type "weighted": AND gate; activates the target when all of its inputs are
		 activated.
		 
		 Example use: reducing antline clutter; fixing flip panel bug.

Type "cube":	 OR gate; activates the target when at lease one of its inputs
                 is activated.
		 
		 Example use: to activate the same thing from multiple
		 locations.

Type "ball":     XOR gate; activates the target when an odd number of inputs is
		 active.  (In other words: the on/off state switches for each
		 activation or de-activation.)
		 
		 Example use: to make people's brains overheat :)

In addition, the output for each type can be inverted (NAND, NOR, NXOR) by
checking the "start enabled" option.

Important: when connecting one logic gate to another, the connection order
matters!  Select the source, press "K", then click on the destination.
