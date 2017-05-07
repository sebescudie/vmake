vmake
=====

*Command line tool to generate VVVV project folder structure.*

Howto
----------

- Clone the repo and build or download [latest version](http://sebescudie.fr/files/vmake.7z)
- Put `vmake.exe` somewhere on your hard drive (e.g `C:\vmake`)
- Add this folder to your system's %PATH% (struggled to make it automagically with a .bat without success :/)
- Create a new folder for your project and go inside it
- Open a `cmd` by typing `cmd` in the address bar
- Type `vmake init`
- To directly open your newly created v4p file, add `/o` command argument

```
vmake init /o
```

Folder structure
----------
This is the project structure I'm used to for my projects. Might not fit everyone's needs or habits but that's the way I roll :)

```
├── assets
|   ├─ mesh
|   ├─ sounds
|   ├─ tex
├── modules
├── subs
├── root.v4p
```
