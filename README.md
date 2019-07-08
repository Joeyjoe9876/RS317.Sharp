# RS317.Sharp

A fork of [317Refactor](https://github.com/Jameskmonger/317refactor) that is ported to C#/.NET.

Special thanks to the developer of [RS2Sharp](https://www.rune-server.ee/runescape-development/rs2-client/downloads/466784-rs2sharp-fully-functioning-rs2-317-client-c-2.html) which indirectly contributed to the WinForms/GDI rendering and input code.

## Implementations

Rs317.Sharp is made up of the core Library of the RS engine as well as some code that allows for easily creating implementations of the client. Currently, two implementations exist.

[Rs317.Client.WF](https://github.com/HelloKitty/RS317.Sharp/tree/master/src/Rs317.Client.WF): Is the Winforms/GDI+ implementation that renders similarly to the Java applet version by pushing bitmaps to the screen.

[Rs317.Client.OpenTK](https://github.com/HelloKitty/RS317.Sharp/tree/master/src/Rs317.Client.OpenTK): Is the OpenGL/OpenTK implementation that also, as of the time of this documentation, pushes bitmaps to the screen via OpenGL. However, because it's GPU rendered it allows for psuedo-antialiasing and Resizable. Eventually it should/could support rendering the gameview as OpenGL meshes.

## License

Applicable licensing referenced in [317Refactor](https://github.com/Jameskmonger/317refactor) and additional work/changes done licensed under a modified AGPL license.
