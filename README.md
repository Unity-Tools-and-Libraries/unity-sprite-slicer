# unity-sprite-slicer
A library to programmatically generate slices sprites.

## Adding to your project via Unity Package Manager
Open **Window > Package Manager** in your editor.

Click the **+** in the top left and select **Add package from git URL...**

Paste *https://github.com/Unity-Tools-and-Libraries/unity-sprite-slicer.git?path=Assets* and click **Add**.

## Usage
This app assumes that sprites are layed out with each row as group of related sprites.

Find the image in your project to be sliced and right click. Find **Custom Actions > Process Image**.

This will open both the default Unity sprite editor and the new custom window.

Enter the desired size of the output sprites.

Click "Add New Name" button to add a new name. Sprites will be named from top to bottom based on this list.
