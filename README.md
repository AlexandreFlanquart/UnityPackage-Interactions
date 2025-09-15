# MYUNITYPACKAGE-Interactions

## About
MyUnityPackage Interactions is a package who allowed you to make a infinite possibilities of interactions between the player and different objects !
With the modalirity of this package you can change different parameters like the trigger, the conditions or the effects !

## What's New
To see the last update of the package check [here](CHANGELOG.md) !

## How to use
To create interactable object you need :
- Create a script who derive from the <b>AInteractable</b> class at the base
- Create a child with the effect who derive from <b>AEffect</b> class
- Create a child with the condition who derive from <b>ACondition</b> class
- Create a child with a script who derive from <b>AInteractionTrigger</b> class, for this you can use one of the premade script or a complete custom depends of your needs.

After that add the AInteractionTrigger class you created, the effect and the conditions in the AInteractable class in the inspector.

## 📦 How to install in Unity
This guide explains how to install this Unity package using the **Unity Package Manager**.

### 🔹 1. Open the Package Manager
1. In Unity, go to the **top menu**.
2. Click **Window > Package Manager**.
3. The **Package Manager** window will open, showing the list of installed packages.

### 🔹 2. Add the Git Package
1. In the **Package Manager**, click the **➕** button (top left corner).
2. Select **"Add package from git URL..."**.
3. Enter the following Git repository URL: <br>
   https://github.com/AlexandreFlanquart/UnityPackage-Interactions.git
4. Click **"Add"**, and Unity will download and install the package.

### 🔹 3. Install a Specific Version (Optional)
If you want to install a specific release, **append the tag** at the end of the URL: <br>
https://github.com/AlexandreFlanquart/UnityPackage-Interactions.git#v1.0.0

This ensures you get the exact version you need.

### 🔹 4. That's it! ✅
Your package is now installed and ready to use in your Unity project. 🎮🚀

<br>

## 🛠️ Troubleshouting
If there is an issue, report it to a dev.
