# Sega Model3 UI for 20240128+
*Copyright 2004 PonMI The man eat shark*

## Overview

SegaModel3UI is (Runs on Windows operating systems 10 or 11)　a launcher for Supermodel, an emulator for Sega Model 3. Supermodel is a program that emulates the hardware of Sega Model 3, allowing users to run Sega Model 3 games on their computers. SegaModel3UI provides an interface to make Supermodel more user-friendly, enabling users to easily configure settings and launch and manage games.

## How to get it

Windows builds are available on [Release](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/releases) page.

## Build Instructions

### Windows
* 1 Install Visual Studio: First, download and install Visual Studio Community 2022.
* 2 Obtain the Source Code: Get the source code for Supermodel3UI. You can clone it from a repository like GitHub or download it directly.
* 3 Open Solution File: Launch Visual Studio and open the Supermodel3UI solution file (.sln).
* 4 Add SharpDX and SharpDXInput via NuGet: Right-click on your solution in Solution Explorer, select "Manage NuGet Packages for Solution," search for "SharpDX" and "SharpDXInput," and install them for your project.
* 5 Select Build Configuration: Choose the build configuration (Debug or Release) you want to build.
* 6 Build: Select "Build" > "Build Solution" from the menu bar. This will compile the source code and generate executable files.
* 7 Run: Once the build is complete, run the generated executable to launch Supermodel3UI.

### Additional Future
Sega Model3 UI for 20240128+ includes additional features not found in the original version. 
Pressing a small button on the right reveals hidden options where you can enable/disable FakeScanLine settings. 
![319844574-a0fb75c5-8d02-4108-8bea-0d940ead7b5f](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/assets/107375174/9c5e86b2-b16a-438b-baf4-3c5723f0eb6a)


Additionally, there is a simple controller viewer that displays input status in Xinput format.

![image](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/assets/107375174/5f4bd9e6-6819-4a2e-8e53-a4b93fd90469)

Additionally, when used in conjunction with Supermodel3 PonMi Edition, you can change the titles. 
This feature is aimed at enabling correct capture through capture software like OBS when running multiple instances of Supermodel on a single PC (e.g., for link play).

![aaasasdasd](https://github.com/BackPonBeauty/Sega-Model-3-UI-for-20240128-/assets/107375174/b1da9c62-546a-4251-ac6f-2bd510381054)





