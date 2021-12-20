# AFEStatViewer
A statistics viewer for Aliens: Fireteam Elite

Expects savegame file to be at the default location: "%LOCALAPPDATA%\Endeavor\Saved\SaveGames\"
In AFE prior to Season 2, this was always: "%LOCALAPPDATA%\Endeavor\Saved\SaveGames\char.sav"
In AFE Season 2 (and later?) this can be: "%LOCALAPPDATA%\Endeavor\Saved\SaveGames\<SteamID>\char.sav"

AFE Statistics Viewer will automatically search for the most recently-accessed savegame, and read that.

This mostly doesn't matter to most players, AFE Statistics Viewer will find your savegame automatically
upon being started. However, in cases where more than one Steam user on a computer plays AFE and maintains savegames 
on the same computer, you may need to have AFE access your savegame in order to ensure it's reading the correct one,
after switching users.
In order to do this if you do have multiple Steam users playing AFE on the same computer:
1. Start AFE and let it load to the main menu.
2. Quit the game. (AFE appears to update the savegame here.)
3. AFE Statistics Viewer will now read the correct savegame. (And will continue to do so until you switch users.)

As AFE Season 2 added Lifetime Stats, the campaign completion tracking portion of AFE Statistics Viewer 
has become (Thankfully! This should heve been in AFE from day one!) obsolete, as this information is now
available in game. However, AFE presently still does not show progress towards several achievements.
AFE Statistics Viewer allows players to check that information still.

Presently displays which campaign missions, on which difficulties you have completed or not.
Also displays progress towards SOME achievements.
(Mostly has been made obsolete with the release of AFE Season 2.)

Run program. Observe window with information that pops up.
Does not refresh in real time, restart program to get an updated readout.

You MAY need to install .NET 5.0 to get this to work. Download link here should work: https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-5.0.0-windows-x64-installer