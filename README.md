# VR-Recorder
A playback and recording system for VR development in Unity.

VR-Recorder addresses the need during vr development to test user interactions in a repeatable way. It achieves this by allowing you to record a session, including the HMD camera position and pose, and any controller position and poses. During development, a developer can load a previously recorded session and play back the hmd and controller positions and rotations. Playback can even be performed when a real HMD is not connected at all.

A sample scene is provided in the Examples Scenes folder, and a sample tracker session is saved in TrackerFiles.

### Note
VR-Recorder requires a slightly modified version of the SteamVR Library. This is included with this library. Modifications are necessary to allow hmd and controller poses to be injected through the standard SteamVR pipeline.

## How To Use ##
Copy the Assets and sample TrackerFiles into the root of your Unity project, overwriting any existing files in your SteamVR directory. You can either modify your existing Camera Rig (Hard), or use the provided prefab (Easy).

### Using a prefab: ###
The 'RecorderRig' prefab is located under Assets/Prefabs/RecorderRig. Drag it into your scene.

### Modifying an existing rig: ###
* Add a VR Player script to your Root CameraRig game object.
* Drag 'Controller (Left)' game object onto the Left Controller property on the VR Player script  
* Drag 'Controller (Right)' game object onto the Right Controller property on the VR Player script
* Drag 'Camera (Eyes)' game object onto the Hmd Device property on the VR Player script
* On the Camera (Eyes) game object (or whichever gameobject has the SteamVR_Camera script), perform the following:
..* Add a VR Recorder script
..* Add a Tracked object Recorder Proxy script
* On each controller game object, perform the following:
..* Add a VR Recorder script
..* Add a Tracked Object Recorder Proxy script


### Quick Start: ###
#### Record a new session: ####
* Start your scene
* Make sure your HMD and controllers are connected and tracking by SteamVR
* From the VR Tools menu, select Set Session Name
* Select the folder to record the session to
* From the VR Tools menu, select Start Recording
* Perform your pose, move controllers etc
* Stop the recording to save the session to disk, from the VR Tools menu, select Stop Recording

#### Playback a session: ####
* Start your scene
* From the VR Tools menu, select Set Session Name (If not already set)
* From the VR Tools menu, select Start Playback

To stop playback, just stop your scene.
