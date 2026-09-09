# AFEStatViewer

A statistics viewer for Aliens: Fireteam Elite and Aliens: Fireteam Elite 2



Expects savegame file to be at the default location:
In AFE1 prior to Season 2, this was always: "%LOCALAPPDATA%\\Endeavor\\Saved\\SaveGames\\char.sav"
In AFE1 Season 2 (and later) as well as AFE2 this can be: "%LOCALAPPDATA%\\Endeavor\\Saved\\SaveGames\\<SteamID>\\char.sav"

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



Additionally, AFE Statistics Viewer will update automatically every time AFE saves the game, which can lead to

easier greenstamping, with AFE Statistics Viewer running while the game is being played, for quick and easy reference

as to the next map to complete.

It also makes it easy to share your progress with others, as it's all in one place.



Special thanks to:
HourOfOblivion



Todo:

AFE1 Hardcore Mode Tracking

More Achievement support.

Add scrollbars to the completion data areas when the window is shrunk fairly small and they no longer fit.

Expand the area where Achievement tooltips are active to the entire Achievement tile rather than just the text.

