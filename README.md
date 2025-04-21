# OLK Viewer
~~No longer being worked on.~~
Doing my best to get this back on track!

## To do:
	- Improve file replacement
	- Read PKGs recursively
	- Get actual texture resolution (might be fixed)
	- Add more texture formats

You'll have to guess, or check the xbox files in another program to get the proper texture resolution. 
Make sure it's the correct resolution before importing, otherwise it will overwrite other data.

Backup your files just to be safe.


# How to use

## File Replacement
Only works with files in the root olk. Extract File5.olk, then open it with OLK viewer if you want to replace a file in there.

## Texture Replacement
If you want to replace Link's green tunic with a black one for example, open root.olk and select:

	File5.olk > File944.pkg > File1.vmg

Select T0-DXT1 on the right, set the resolution to 512x512, uncheck Alpha Map, right click T0-DXT1 again, and choose import.
Do the same for T1-DXT1 but set the resolution to 32x32.
