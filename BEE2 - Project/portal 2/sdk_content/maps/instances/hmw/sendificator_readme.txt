_______________________________________________________________________________

                        Sendificate part 0 source files
_______________________________________________________________________________


This archive contains the source files for the Portal 2 map "sendificate",
version 4.

You may use these files for your own work, in accordance with the Creative
Commons 3.0 Attribution Unported licence.
For details, see: http://creativecommons.org/licenses/by/3.0/


VERSION 4 MAJOR UPDATE: you can now use the sendificator in coop maps!  Many
thanks to Groxkiller585 for helping me out with this.


                                File structure
_______________________________________________________________________________


The folder structure in this archive matches the one found in the "portal2"
folder of the official game.  The subfolder "_replaced files" contains files
that replace the original game versions; make sure to create backups of the
originals before copying this folder, or use a separate "mod" file structure.


                   How to add a sendificator to your own map
_______________________________________________________________________________


This package includes an additional map called "sendificator_test", that I used
for testing the mechanism.  It shows a simple example of how to use the various
components.

To add a sendificator to your own map, do the following:

-  Make sure that you have all of the relevant files in your Portal 2 folder.
   Just copying this archive into the main .../Portal 2/portal2 folder will do
   the trick.  Also remember to include the materials, models and scripts in
   your distribution.  (Gotta bsp-zip 'em all!)

-  Insert one instance of the "instances_hmw/gadgets/sendtor_manager.vmf"
   template into your map.  This contains some global entities needed by the
   mechanism.  You can position it anywhere you want, but do not rotate it!


-  Add one or more func_portal_detectors to your map that completely envelope
   the playable area.

   * For single player, add one detector named "@sendtor_portal_detect".

   * For coop, add two detectors named "@sendtor_portal_detect1" and
     "@sendtor_portal_detect2".  Set the detector portal ID to 1 and 2
     respectively.

   (You don't need to add any outputs, the sendificator script will take care
    of that.  You can add your own outputs for other purposes; these will not
    interfere with the mechanism.)


-  Glass windows MUST be 1 unit thick.  You no longer need to make them
   non-solid and add separate clip blocks, as was necessary with previous
   versions.

   If you have thicker glass in your map and you can't change it for some
   reason, look in the commments inside the sendificator.nut script for a
   possible workaround.


-  You need to trigger both a sendtor_pedestal* and its corresponding
   sendtor_emit* instance at the same time for anything to happen.  This is
   done to allow them to be combined in any way your twisted puzzle logic may
   require ;)

   To trigger the special green / orange antlines, include appropriate antline
   controller instances and trigger them at the same time.  (See example map.)

-  When a sendificator is triggered, all the others are disabled for 2 seconds
   to prevent the script from getting confused.  There is a mutex-like
   mechanism in the templates that enforces this.  Messing with it will void
   your warranty!
