using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class DeliverDish : MonoBehaviour
{
    public Clock clockObject;
    private int currHour;
    private int currDay;
    public string sceneName;
    Scene currScene;

    //game object with the feedvillagers script attached
    // public GameObject feedVillageObject;

    // Start is called before the first frame update
    // void Start()
    // {
    // }

    // Update is called once per frame
    void Update()
    {
        //FeedVillagers fvScript = feedVillageObject.GetComponent<FeedVillagers>();

        currHour = Clock.hour;
        currDay = Clock.day;
        //if (currHour == 22) { // 10pm -> meal time 
        if (isDayToDeliver()) {
            if (sceneName != "DiningRoom") {
                Dialogue.Activate("DeliverDishIntro" + sceneName);
                //fvScript.enabled = false;
            }
            else {
                // nothing
            }
        }
        else {
            // not a day to deliver dishes
            // fvScript.enabled = false;
        }
        
        
        // once the dialogue telling player to go to dining hall has played, AUTOMATICALLY direct player to exit to dining hall (use navmesh)
        // if (hasPlayedDeliverIntro && !startMovingToExit) {
        //    // Debug.Log("has played deliver intro");
        //     MoveToExit();
        //     startMovingToExit = true;
        // }
    }

    public bool isDayToDeliver() {
        if ((currDay == 7 || currDay == 14) && (currHour == 22)) {// && !hasPlayedDeliverIntro) {   // for debugging purposes, 10am
            return true;
                //hasPlayedDeliverIntro = true;
        }
        return false;
    }
    
    // void PlayDialogue() {
    //     SceneTransition stScript = diningHallExit.GetComponent<SceneTransition>();
    //     stScript.enabled = false;
    //     Dialogue.Activate("DeliverDishIntro" + sceneName);
    //     //hasPlayedDeliverIntro = true;
    // }

    // void MoveToExit() {
    //     SceneTransition stScript = diningHallExit.GetComponent<SceneTransition>();
    //     stScript.enabled = true;
    //     agent.SetDestination(diningHallExit.transform.position);
    //     startMovingToExit = true;
    // }

    // GameObject SpawnVillager() {
    //     GameObject spawnedVillager = Instantiate(villager, spawnVillagerPosition, Quaternion.identity);
    //     Debug.Log("the villager should spawn here");
    //     return spawnedVillager;
    // }

    // void PlaceDish() {
    //     //while there are still dishes in inventory
    //         //remove a dish from inventory ("UseItem")
    //         //spawn a villager
    //         //villager.SetDestination(dish.transform.position)
    //         //when villager collides w dish --> play eating noise and make dish Item disappear
    //         //villager.SetDestination(corner of dining hall)
    //         //once villager reaches corner, destroy villager object
        
    //     //for now, do while there are items in inventory (change later so that just take DISHES -> add attribute "item type" to Item class?)
    //     // for (int i=0; i<inventory.items.Count; i++) {
    //     Item currItemToRemove = inventory.items[0];
    //     inventory.Remove(currItemToRemove);
    //     //invSlot.UseItem(currItemToRemove); // puts item out into game, remove from inventory UI
    //     //dishPosition = player.transform.position; // agent will go towards player, which is near the deposited item
    //     // }
    // }

    // void EndDeliverDishScene() {
    //     enabled = false;
    // }

}
