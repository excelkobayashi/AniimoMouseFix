# About This Project
This project fixes an issue where the mouse doesn't work in the game [Aniimo](https://www.aniimo.com) when running on Linux through Wine.
The issue is caused by an invisible web overlay window that stays on top of the game and blocks the mouse clicks.
This overlay also exists on Windows, but it doesn't block the mouse clicks for whatever reason.
Note that it's always possible to play with a controller instead, and the keyboard and mouse movements still work too, just not clicking.

This project simply searches for the window and hides it.
Note that it's possible that this could hide some important browser overlay used by the game, so proceed with caution.
I believe the browser overlay is primarily used for logging in, so you should make sure have the game logging in and ready to play before running this.

Note that this does NOT fix another unrelated issue where the launcher might not open.
If you encounter that problem, then I recommend first downloading the game in a VM (try WinBoat), and then copying the files back over to Linux and then launching the main game EXE directly without using the launcher.

# How To Use
1. Make sure you have the game setup and ready to play, with your account already logged in
2. Download the ZIP from the [Release Page](https://github.com/excelkobayashi/AniimoMouseFix/releases) and extract it somewhere
   - Note: this doesn't have to be within the game's Wine prefix
3. Add an additional environment variable for launching the game:
  - `PROTON_REMOTE_DEBUG_CMD=/path/to/AniimoMouseFix.exe`
  - Obviously the path will need to be updated to wherever you extracted the ZIP
  - Avoid using any paths with spaces, and use the full real Linux path (not the Windows path)
  - The method for setting this environment variable depends on how you're launching the game (Steam, Lutris, Heroic, Faugus, etc)
  - Some launchers support running "additional programs" with the game, so that could be an alternative this
4. Launch the game and wait a few seconds on the login screen
5. Eventually the mouse should start working and the fix will exit
