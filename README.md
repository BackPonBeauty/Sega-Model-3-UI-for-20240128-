# Sega Model3 UI for 20240128+
*Copyright 2024 PonMI The man eat shark*

## Overview

Segamodel3UIfor20240128+ is (Runs on Windows operating systems 10 or 11)　a launcher for [Supermodel](https://github.com/trzy/Supermodel), an emulator for Sega Model 3. Supermodel is a program that emulates the hardware of Sega Model 3, allowing users to run Sega Model 3 games on their computers. Segamodel3UIfor20240128+ provides an interface to make Supermodel more user-friendly, enabling users to easily configure settings and launch and manage games.

## How to get it

Windows Latest builds are available on [Release](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/releases) page.

## Build Instructions

### Windows
* 1 Install Visual Studio: First, download and install Visual Studio Community 2022.
* 2 Obtain the Source Code: Get the source code for [Segamodel3UIfor20240128+](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-). You can clone it from a repository like GitHub or download it directly.
* 3 Open Solution File: Launch Visual Studio and open the Supermodel3UI solution file (.sln).
* 4 Add SharpDX and SharpDXInput via NuGet: Right-click on your solution in Solution Explorer, select "Manage NuGet Packages for Solution," search for "SharpDX" and "SharpDXInput," and install them for your project.
* 5 Select Build Configuration: Choose the build configuration (Debug or Release) you want to build.
* 6 Build: Select "Build" > "Build Solution" from the menu bar. This will compile the source code and generate executable files.
* 7 Run: Once the build is complete, run the generated executable to launch Segamodel3UIfor20240128+.

### Additional Future
Segamodel3UIfor20240128+ includes additional features not found in the original version. 
Pressing a small button on the right reveals hidden options where you can enable/disable FakeScanLine settings. 
This feature does not work in fullscreen mode.

* CTRL+[S] ...... Toggle FakeScanLine
* CTRL+[P]/[O] ...... Opacity +/-
* CTRL+[I] ...... Form bring to front
![319844574-a0fb75c5-8d02-4108-8bea-0d940ead7b5f](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/assets/107375174/9c5e86b2-b16a-438b-baf4-3c5723f0eb6a)


Additionally, there is a simple controller viewer that displays input status in Xinput format. and RawInput(MouseIndex)
![image](https://github.com/user-attachments/assets/52192ca0-ad4b-4b1e-962b-f1b151a59beb)

Additionally, when used in conjunction with [Supermodel3 PonMi Edition](https://github.com/BackPonBeauty/Supermodel3-PonMi), you can change the titles. 
This feature is aimed at enabling correct capture through capture software like OBS when running multiple instances of Supermodel on a single PC (e.g., for link play).

![aaasasdasd](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/assets/107375174/b1da9c62-546a-4251-ac6f-2bd510381054)







