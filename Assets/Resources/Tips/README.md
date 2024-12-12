1. Create an instance of the Tip prefab (Assets/Prefabs/UI)
  - Tip.cs is attached to this prefab. Set its "Title" and "Text" properties accordingly.
  - The Text property holds about 165 characters.

2. If the tip is to appear when an object is equipped, rename the prefab so it has the same name as the object. Drag and drop it into Assets/Resources/Tips to create a prefab variant.
  - REMOVE the prefab from your scene.
  - You're done, ignore the next steps.

3. If the tip is to appear manually, add an instance of the GameManager prefab (Assets/Prefabs/Game) to your scene if it does not already exist.

5. If When you want to start the dialogue, call: GameObject.Find("GameManager").GetComponent<Game>().ActivateTip("NAME OF THE PREFAB");