# About This Project
This project fixes an issue where the mouse doesn't work in the game [Aniimo](https://www.aniimo.com) when running on Linux through Wine.
The issue is caused by an invisible web overlay window that stays on top of the game and blocks the mouse clicks.
This overlay also exists on Windows, but it doesn't block the mouse clicks for whatever reason.
Note that it's always possible to play with a controller instead, and the keyboard and mouse movements still work too, just not clicking.

This project simply searches for the window and hides it.
Note that it's possible that this could hide some important browser overlay used by the game, so proceed with caution.
I believe the browser overlay is primarily used for logging in, so you should make sure have the game logging in and ready to play before running this.

# How To Use
These are instructions for people that already have the game working with a controller and just want to fix the mouse.
See the [Full Setup Guide](#full-setup-guide) below instead if you don't have the game working yet.
1. Make sure you have the game setup and ready to play, with your account already logged in
2. Download the ZIP from the [Release Page](https://github.com/excelkobayashi/AniimoMouseFix/releases) and extract it somewhere
   - Note: this doesn't have to be within the game's Wine prefix
3. Add an additional `Environment Variable` for launching the game:
  - `PROTON_REMOTE_DEBUG_CMD=/path/to/AniimoMouseFix.exe`
  - Obviously the path will need to be updated to wherever you extracted the ZIP
  - Avoid using any paths with spaces, and use the full real Linux path (not the Windows path)
  - The method for setting this environment variable depends on how you're running the game (Steam, Lutris, Heroic, Faugus, etc)
  - Some launchers support running "additional programs" with the game, so that could be an alternative this
4. Launch the game and wait a few seconds on the title screen
5. The fix will run in the background and check the window every 10 seconds
6. Wait on the title screen and the mouse should start working

# Full Setup Guide
These instructions are written for CachyOS, you will have to adjust these instructions for your situation as needed.
Replace `{user}` with your OS username.

Note that we'll have to set up 2 different game entries because the launcher and the game itself need different Wine configurations.

### 1. Preparation
1. Make sure you have the following packages installed: `wine-cachyos-opt`, `proton-cachyos-native`, `lutris`
2. Create a new directory: `/home/{user}/Games/Aniimo`
3. Download the official Aniimo installer to `/home/{user}/Games/Aniimo/aniimo-global.exe`
4. Download `AniimoMouseFix.zip` from the [Release Page](https://github.com/excelkobayashi/AniimoMouseFix/releases) and extract it to `/home/{user}/Games/Aniimo/AniimoMouseFix/`
5. Open Lutris and click the ⚙️ (configure) icon on the `Wine` runner on the left column
   - Set `Runner Options`:
      - Enable DPI Scaling: on
      - DPI: 192
        - Note: this is for 4k monitors, adjust as needed
   - Set `System Options`:
     - Disable Lutris Runtime: on
     - Command prefix: game-performance

### 2. Launcher
1. In Lutris, click the + button in the top-left, and select `Add locally installed game`
   - Set `Game Info`:
     - Name: Aniimo (Launcher)
     - Runner: Wine
   - Set `Game Options`:
     - Executable: /home/{user}/Games/Aniimo/aniimo-global.exe
     - Wine prefix: /home/{user}/Games/Aniimo/Launcher/
   - Set `Runner Options`:
     - Wine version: custom
     - Custom Wine executable: /opt/wine-cachyos/bin/wine
       - Note: we can't use Proton here because it adds extra fonts, and the launcher crashes if any of these fonts exist: simsun, msgothic, malgun, tahomabd 
2. Open a command console and run:
   - `WINEPREFIX="/home/{user}/Games/Aniimo/Launcher/" /opt/wine-cachyos/bin/wine reg add "HKEY_CURRENT_USER\Software\Wine\X11 Driver" /v ClientSideWithRender /t REG_SZ /d N`
   - Note: this fixes an issue where the launcher window doesn't open when DPI scaling is enabled
3. Run the `Aniimo (Launcher)` in Lutris
   - Select `Custom Install`
   - Browse to select the `Z:\home\{user}\Games\Aniimo` folder
   - The full path should say `Z:\home\{user}\Games\Aniimo\Aniimo`
   - After the installation, close the installer without clicking the `Open Now` button
4. Back in Lutris, right-click to configure the `Aniimo (Launcher)`
   - Under `Game Options`, set the `Executable` to `/home/{user}/Games/Aniimo/Aniimo/Launcher.exe`
5. Run `Aniimo (Launcher)` again
   - Install the game
   - After installation, close the launcher without clicking `Start`

### 3. Game
1. In Lutris, click the + button in the top-left, and select `Add locally installed game`
    - Set `Game Info`:
        - Name: Aniimo
        - Runner: Wine
    - Set `Game Options`:
        - Executable: /home/{user}/Games/Aniimo/Aniimo/game/Aniimo.exe
        - Wine prefix: /home/{user}/Games/Aniimo/Game/
    - Set `Runner Options`:
        - Wine version: proton-cachyos-native
2. Run `Aniimo` in Lutris
   - Wait for the downloads to finish
   - Log in to your account
   - Once you are logged in, it is expected that the mouse can no longer click in the game
   - With the game window still focussed, press `ALT+F4` to close the game
3. Back in Lutris, right-click to configure `Aniimo`
   - Under `System Options` `Environment Variables`, add:
     - Key: PROTON_REMOTE_DEBUG_CMD
     - Value: /home/{user}/Games/Aniimo/AniimoMouseFix/AniimoMouseFix.exe
4. Run `Aniimo` again
   - At this point everything should be working normally
   - The mouse fix checks every 10 seconds, so you may have to wait a few seconds for the mouse to work
   - Note that the mouse fix will hide the login overlay, so if you ever have to log in again you'll need to temporarily remove the mouse fix environment variable
