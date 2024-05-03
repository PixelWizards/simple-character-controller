# Simple-character-controller
------------------------------------------------------
A simple Unity character controller

# What it is
------------------------------------------------------
The simplest way to describe this is a rework of the character controller that is available in the Unity Starter / Standard packages.

## Trailer

![Preview Trailer](https://github.com/PixelWizards/simple-character-controller/blob/main/.Docs%2FThumbnail.png)

Preview Trailer: https://www.youtube.com/watch?v=OF-5xu5xEpM

## Demo 
There is a demo (windows exe) available, if you'd like to check it out:

https://github.com/PixelWizards/simple-character-controller/releases/download/Release%2F1.0/Simple-Character-Controller-Demo-Release-1.0.zip

# Multiple movement modes

## Free Roam
Camera relative movement, Character rotates to face the direction that they are moving

## Targeted / Over the Shoulder
A standard over the shoulder third person shooter type movement system. Character faces in the direction the camera is aiming, 
character has 8 directional movement animation (strafe / etc) for proper directional movement.

## As well as 
- Jump
- Fall
- Walk / Run
- pre configured Cinemachine third person camera complete with camera offset, collision etc.

# Getting started
------------------------------------------------------

Open Scenes/Prototype.unity

Code is in /Scripts

Project was built with setup with URP, Unity 2022.3.24f1, but has no dependency on URP, will work with any render pipeline.

# Known Issues

- the jump code is basically the same as the original starter assets code, it's pretty jank, will take a look at it
- there are some random bugs that I haven't had time to sort out (going up the stairs speeds you up? wtf?) anyways...
- you fall far too fast, it's jarring, need to revise gravity logic
- Report any bugs you find! (use Github Issues)

# Roadmap
We have a public roadmap over on Codecks:
https://open.codecks.io/simple-character-controller

# Get in touch!

If you have any feature requests / bug reports, or want to chat about the character controller (or any game dev topics)
please feel free to join our discord! 

https://discord.gg/qKVPm4XWZ9


# Asset Licenses

## Character by Quaternius
------------------------------------------------------
Ultimate Modular Males, Ultimate Modular Females and Zombie by @Quaternius
https://www.patreon.com/quaternius

Character License:
CC0 1.0 Universal (CC0 1.0) 
Public Domain Dedication
https://creativecommons.org/publicdomain/zero/1.0/


## Animations are from Unity:
------------------------------------------------------
These animations are from the Unity Standard-Assets-Characters repo:
https://github.com/Unity-Technologies/Standard-Assets-Characters

These animations are from the Unity Starter Assets - Third Person
https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526

Animation License:
Unity Companion License:
https://unity.com/legal/licenses/unity-companion-license
