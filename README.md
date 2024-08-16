# MedicineVR

We are using the Microsoft Mixed Reality Toolkit with the following packets: 
- Foundation 
- Tools 
- Extensions 

These have to be installed manually and are published by Microsoft through the Mixed Reality Feature Tool: \
https://learn.microsoft.com/de-de/windows/mixed-reality/mrtk-unity/mrtk2/configuration/usingupm?view=mrtkunity-2022-05

**This only needs to be done if you want to check out the project. You do not have to do this in order to start the application on Quest2**

We used licensed material for this application. For the credits, please visit the Credits.txt file. If we forgot someone or something in the credits, please let us know!

# Build Manual

Little tip beforehand: Shift+RightClick lets you select "Copy Path" on any file.

Firstly, you need to build an aab file by checking Build App Bundle" in the build settings.
Then you have to build the aab file to get an apks bundle. For that, you have to install the latest buildtool version by Google. You also need to have java installed.
Open a terminal in the folder where you installed the bundletool and insert the corresponding paths into the following line:
```
java -jar bundletool-all-<VERSION>.jar build-apks --bundle=path/to/your_app.aab --output=path/to/your/output_dir/output.apks --mode=universal
```
Here, insert the path to the aab file into the "--bundle" parameter and provide an output filepath, ending with .apks, in the "--output" parameter. Also, mind that you have to input the right bundletool version.
Lastly, you need to create a JSON file with a name like quest2_devicespec.json and insert:
```
{
  "supportedAbis": ["arm64-v8a"],
  "screenDensity": 560,
  "sdkVersion": 30,
  "supportedLocales": ["en-US"],
  "glExtensions": ["GL_OES_EGL_image_external"]
}
```
into that file. Now, type the following line into a terminal, which is opened in the same location as the bundletool, 
while inserting the corresponding paths:
```
java -jar bundletool-all-<VERSION>.jar extract-apks --apks=path/to/your_app.apks --output-dir=path/to/output_directory --device-spec=path/to/quest2_devicespec.json
```
Here, "--apks" gets the path to the apks file, "--output-dir" gets the output directory path and "--device-spec" gets the path to your devicespec file.
The result will be a "universal.apk", which you can then install to your quest by sideloading or installing adb with the following command:
```
adb install universal.apk
```
For that, your quest has to be connected to your PC, while developer mode is on and usb debugging is enabled.

# User Manual

If you want to test the application, you can find a current version of our application in the "Releases"-Tab.
After downloading the apk file, you can load it onto your Quest 2 using an adb loader like SideQuest.

On application start, you will be greeted and introduced by Doctor Klamma. If you want to know more about a specific organ, you can click on it.
After clicking on an organ, you will spawn inside a slightly different room with a bigger version and a smaller, more interactible version of the selected organ.
Doctor Klamma will then explain the organ in more detail and guide you to a little quiz, testing your knowledge.
If you want to get back to the starting room, you currently have to use the hand menu, which is only available when using hand-tracking, so make sure, it's enabled!

## Controls
- If you need to "Click" something, use the right/left trigger or pinch your thumb and index finger.
- If you need to navigate, use the left or right thumbstick and push it up to teleport.
- If you need to back off a bit, use the left or right thumbstick and push it back to back off.
- If you need to turn, use the left or right thumbstick and push it in the left or right hand side direction.
- If you need to reset the Scene or get back to the SelectionRoom/want to exit the game, use the hand-tracking menu by looking at your palm.
- Note that our application currently does not support wellness-settings like a "vignette mode".
- Note also that, while our application supports hand-tracking, locomotion is not possible with it currently.
