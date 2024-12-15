1. Create a txt file in Assets/Resources/Dialogues/Scripts.
  - The first line of the file should be the NPC's name.
  - The rest of the file is displayed one line at a time as the player clicks through the dialogue.

2. Create an instance of the Dialogue prefab (Assets/Prefabs/Dialogue)
  - Dialogue.cs is attached to this prefab. Set its "File" property to your txt file.
  - Faelyn the sparrow is displayed by default. If a different NPC is to speak, replace her prefab.
  - Move Faelyn and SecondaryCamera if you want. Change the script properties accordingly in their script components.

3. Rename the Dialogue prefab so it has the same name as the txt file. Drag and drop it into Assets/Resources/Dialogues to create a prefab VARIANT.
  - REMOVE the prefab from your scene.

4. When you want to start the dialogue, call: Dialogue.Activate("NAME OF THE TXT FILE");