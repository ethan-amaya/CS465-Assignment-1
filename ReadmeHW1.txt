CS465 Assignment 1
Team Name: The Inputters
Teammates: Henry Gold, Ethan Amaya, Rhett Long, Nick Guadalupe

================================================================
HOW TO RUN 

Requirements:
- Unity 6.3.x (LTS)
- Meta Quest headset
- Meta Quest Link cable and application (for PC streaming)
- Android Build Support module installed in Unity

Steps to Run:
1. Clone or unzip the project folder.
2. Open Unity Hub and click "Add project from disk", selecting the project root folder.
3. Open the project in Unity 6.3.x.
4. Connect your Meta Quest headset to your PC via Link cable and launch the Meta Quest Link application.
5. In Unity, open the desired scene from Assets > Scenes:
   - Section1_XRToolkit  (XR Interaction Toolkit scenes)
   - Section2_MetaXR     (Meta XR SDK scenes)
   - Section1_Gestures   (XR Toolkit gesture recognition)
6. Press Play in the Unity editor to run in Link mode, or build and deploy the APK using Meta Quest Developer Hub.

================================================================
SECTION 1 - XR INTERACTION TOOLKIT

Task 1 - Object Interaction:
- A table platform is present in the scene.
- Red Block: Grabbable using hand or controller.
- Blue Block: Not grabbable. Changes color from blue to white when hand/controller is nearby.
- Yellow Block: Not grabbable. Plays a sound when hand/controller is nearby.
- Purple Block: Not grabbable. Disappears when hand/controller is nearby.

Task 2 - Gesture Recognition:
- Implemented in scene: Section1_Gestures
- GestureDetector.cs is attached to LeftHand and RightHand GameObjects under XR Origin.
- GestureUI.cs manages the on-screen text display.
- Supported gestures:
  - Thumbs Up: Displays "Right hand thumbs up!" or "Left hand thumbs up!"
  - Fist: Displays "Right hand fist!" or "Left hand fist!"
  - Peace Sign: Displays "Right-hand Peace Sign!" or "Left-hand Peace Sign!"
- Messages auto-clear after 2 seconds.

================================================================
SECTION 2 - META XR SDK

Task 1 - Object Interaction:
- A table platform is present in the scene.
- Red Block: Grabbable using hand or controller.
- Blue Block: Not grabbable. Changes color from blue to white when hand/controller is nearby.
- Yellow Block: Not grabbable. Plays a sound when hand/controller is nearby.
- Purple Block: Not grabbable. Disappears when hand/controller is nearby.

Task 2 - Gesture Recognition:
- Supported gestures:
  - Thumbs Up: Displays "Right hand thumbs up!" or "Left hand thumbs up!"
  - Fist: Displays "Right hand fist!" or "Left hand fist!"
  - Peace Sign: Displays "Right-hand Peace Sign!" or "Left-hand Peace Sign!"

================================================================
YOUTUBE VIDEO LINKS

Section 1 (XR Blocks) Demo: [INSERT YOUTUBE LINK]
Section 1 (XR Hands) Demo: [INSERT YOUTUBE LINK]
Section 2 (Meta Hands) Demo: [INSERT YOUTUBE LINK]
Section 2 (Meta XR SDK) Demo: [INSERT YOUTUBE LINK]

================================================================
TASK ALLOCATION

Ethan Amaya                        - Section 1, Task 1 (XR Toolkit object interaction)
Henry, Nick, Rhett                 - Section 1, Task 2 (XR Toolkit object interaction)
Henry, Ethan                       - Section 2, Task 1 Part D (XR Toolkit gesture recognition)
Nick, Rhett                        - Section 2, Task 2 Part D (Meta XR SDK gesture recognition)

