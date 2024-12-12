1. Create a txt file in Assets/Resources/Dialogues/Scripts.
  - The first line of the file should be the NPC's name.
  - The rest of the file is displayed one line at a time as the player clicks through the dialogue.

2. Create an instance of the Dialogue prefab (Assets/Resources/Dialogues/Prefabs)
  - Dialogue.cs is attached to this prefab. Set its "File" property to your txt file.
  - Faelyn the sparrow is displayed by default. If a different NPC is to speak, replace her prefab.
  - Move SecondaryCamera to display whatever you wanna show in the background.

3. Rename the Dialogue prefab so it has the same name as the txt file. Drag and drop it into Assets/Resources/Dialogues to create a new prefab.
  - REMOVE the prefab from your scene.

4. Add an instance of the GameManager prefab (Assets/Prefabs/Game) to your scene if it does not already exist.

5. When you want to start the dialogue, call: GameObject.Find("GameManager").GetComponent<Game>().ActivateDialogue("NAME OF THE TXT FILE");