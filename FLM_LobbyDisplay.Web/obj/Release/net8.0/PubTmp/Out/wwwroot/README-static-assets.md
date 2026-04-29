# Static Assets — Manual Copy Required

The following directories from the original project must be copied into this wwwroot folder
to preserve all existing CSS and media file URLs:

| Source (original project)              | Destination (wwwroot)                          |
|----------------------------------------|------------------------------------------------|
| acc/LobbyDisplay/css/                  | wwwroot/acc/LobbyDisplay/css/                  |
| acc/LobbyDisplay/mainscr/              | wwwroot/acc/LobbyDisplay/mainscr/              |
| acc/LobbyDisplay/secscrtop/            | wwwroot/acc/LobbyDisplay/secscrtop/            |
| acc/LobbyDisplay/secscrbtm/            | wwwroot/acc/LobbyDisplay/secscrbtm/            |
| acc/LobbyDisplay2/css/                 | wwwroot/acc/LobbyDisplay2/css/                 |
| acc/LobbyDisplay2/mainscr/             | wwwroot/acc/LobbyDisplay2/mainscr/             |
| acc/MstMain/                           | wwwroot/acc/MstMain/ (scrollingtext.txt etc.)  |
| acc/MstMainLobby2/                     | wwwroot/acc/MstMainLobby2/                     |
| acc/MstMainPan/                        | wwwroot/acc/MstMainPan/                        |
| acc/PantryDisplay/css/                 | wwwroot/acc/PantryDisplay/css/                 |
| acc/PantryDisplay/mainscr/             | wwwroot/acc/PantryDisplay/mainscr/             |
| js/ (jQuery etc.)                      | wwwroot/js/                                    |

All relative URL references in the Razor pages use the same paths as the original .aspx pages,
so no URL changes are needed after copying.
