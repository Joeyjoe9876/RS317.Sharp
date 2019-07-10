# RS317.Sharp

A fork of [317Refactor](https://github.com/Jameskmonger/317refactor) that is ported to C#/.NET.

Special thanks to the developer of [RS2Sharp](https://www.rune-server.ee/runescape-development/rs2-client/downloads/466784-rs2sharp-fully-functioning-rs2-317-client-c-2.html) which indirectly contributed to the WinForms/GDI rendering and input code.

## Implementations

Rs317.Sharp is made up of the core Library of the RS engine as well as some code that allows for easily creating implementations of the client. Currently, two implementations exist.

**[Rs317.Client.WF](https://github.com/HelloKitty/RS317.Sharp/tree/master/src/Rs317.Client.WF):** Is the Winforms/GDI+ implementation that renders similarly to the Java applet version by pushing bitmaps to the screen.



**[Rs317.Client.OpenTK](https://github.com/HelloKitty/RS317.Sharp/tree/master/src/Rs317.Client.OpenTK):** Is the OpenGL/OpenTK implementation that also, as of the time of this documentation, pushes bitmaps to the screen via OpenGL. However, because it's GPU rendered it allows for psuedo-antialiasing and Resizable. Eventually it should/could support rendering the gameview as OpenGL meshes.

## FAQ

#### Q. Why isn't my **Cache** loading?

**A.** Unlike most clients, this client expects the cache to be in a subfolder called Cache.

## License

Applicable licensing referenced in [317Refactor](https://github.com/Jameskmonger/317refactor) and additional work/changes done licensed under a modified AGPL 3.0 licensed.

What does **modified** mean? In this case, all it means is that any contributor to the project retains an additional unrestricted, non-exclusive, perpetual, and irrevocable license granted to themselves for all portions of work they contribute.

What this means is if you write something, contribute it as a pull request to the repository, it is licensed to other users under AGPL 3.0 but you also retain your ownership and rights to it too, so you can do whatever you want with the code you've written yourself and aren't restrited by the repositories AGPL license. In the plainest of terms, you can do whatever you want with what you write. Just not with what everyone else has written, which requires you to follow the AGPL.
