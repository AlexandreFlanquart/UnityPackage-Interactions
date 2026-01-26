# MYUNITYPACKAGE-Interactions

## About
MyUnityPackage Interactions is a package who allowed you to make a infinite possibilities of interactions between the player and different objects !
With the modalirity of this package you can change different parameters like the trigger, the conditions or the effects !

## What's New
To see the last update of the package check [here](CHANGELOG.md) !

## How to use

### Creating an Interactable Object

To create an interactable object, follow these steps:

1. **Create a script** that derives from `AInteractable`
2. **Add an Effect** - Create a child with a script that derives from `AEffect`
3. **Add Conditions** - Create child scripts that derive from `ACondition` (optional)
4. **Add a Trigger** - Create a child script that derives from `AInteractionTrigger` (use a premade one or create a custom implementation)

### Setting up in the Inspector

Once your scripts are created:
- Assign the `AInteractionTrigger` instance to the Trigger field
- Assign your `AEffect` instances to the Effects array
- Assign your `ACondition` instances to the Conditions array (if any)

### Execution Flow

- **Trigger** detects player interaction (zone entry, click, key press, etc.)
- **Conditions** are evaluated to determine if interaction is allowed
- **Effects** execute the defined actions (animations, color changes, events, etc.)
- The derived script handles callbacks (`onEnterAction`, `onInteractAction`, `onExitAction`)

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
