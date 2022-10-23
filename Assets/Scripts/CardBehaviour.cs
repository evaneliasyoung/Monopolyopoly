using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    // --------------- Public Vars ------------------------------------------------------------------------------------
    // Used as a reference to what cards exist
    public GameObject[] unorderedCommunityCards;
    public GameObject[] unorderedChanceCards;
    // Used to show the card to the player
    public GameObject playerCamera;
    
  
    
    // --------------- Private Vars -----------------------------------------------------------------------------------
    // Used for gameobject offsets for cards
    private float[] offsets = new float[] {0.07f, 0.14f, 0.21f, 0.28f, 0.35f, 0.42f};
    // Used to hold the shuffled deck along with any pop operations
    private GameObject[] shuffledCommunityCards;
    private GameObject[] shuffledChanceCards;
    
    
    
    // --------------- Member Functions -------------------------------------------------------------------------------
    private void Start()
    {
        Shuffle("community");
        Shuffle("chance");
    }


    // Shuffle the given deck. This is called only when the given deck is empty.
    private void Shuffle(string deck)
    {
        // SHUFFLE OUR ARRAYS ----------------------------------
        if(deck == "community")
        {
            // Deep copy our array
            GameObject[] communityClone = DeepCopyArray(unorderedCommunityCards);
            
            // Shuffle the deep copy
            for (int i = 0; i < communityClone.Length; i++)
            {
                 int rnd = Random.Range(0, communityClone.Length);
                 GameObject temp = communityClone[rnd];
                 communityClone[rnd] = communityClone[i];
                 communityClone[i] = temp;
             }
            
            // reassign shuffled deck
            shuffledCommunityCards = communityClone;
        }
        else if(deck == "chance")
        {
            // Deep copy our array
            GameObject[] chanceClone = DeepCopyArray(unorderedChanceCards);
            
            // Shuffle the deep copy
            for (int i = 0; i < chanceClone.Length; i++)
            {
                 int rnd = Random.Range(0, chanceClone.Length);
                 GameObject temp = chanceClone[rnd];
                 chanceClone[rnd] = chanceClone[i];
                 chanceClone[i] = temp;
             }
            
            // reassign shuffled deck
            shuffledChanceCards = chanceClone;
        }
        
        
        // UPDATE THE VISUAL ORDER IN UNITY ----------------------------------
        if(deck == "community")
        {
            for(int i = 0; i < shuffledChanceCards.Length; i++)
            {
                // get the transform for this card and use the offsets to put it in the correct deck pos
                Transform thisCard = shuffledCommunityCards[i].GetComponent<Transform>();
                thisCard.position = new Vector3(thisCard.position.x, offsets[i], thisCard.position.z);
            }
        }
        else if(deck == "chance")
        {
            for(int i = 0; i < shuffledChanceCards.Length; i++)
            {
                // get the transform for this card and use the offsets to put it in the correct deck pos
                Transform thisCard = shuffledChanceCards[i].GetComponent<Transform>();
                thisCard.position = new Vector3(thisCard.position.x, offsets[i], thisCard.position.z);
            }
        }
    }

    
    // Draw a card from specified deck, show to camera, then set aside
    //      (if get out of jail card, use icon for UI)
    private void DrawAndShowCard(string deck)
    {
        // check if either deck is empty!
        
 
        // pop off card of corect deck (shuffled)
        
        
        // Show our card to the camera
        ShowCardToCamera();
        
        // now move to "under" the board! (discard?)
        
    }
    
    
    private void ShowCardToCamera()
    {
        // Use LookAt() function to look at the camera!
    }
    
    
    
    
    
    
    // --------------- Helper Functions -------------------------------------------------------------------------------
    private GameObject[] DeepCopyArray(GameObject[] arr)
    {
        GameObject[] returnable = new GameObject[arr.Length];
        
        for(int i = 0; i < arr.Length; i++)
        {
            returnable[i] = arr[i];
        }
        
        return returnable;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
}
