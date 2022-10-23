using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    // --------------- Public Vars ------------------------------------------------------------------------------------
    // Used as a reference to what cards exist
    public List<GameObject> unorderedCommunityCards;
    public List<GameObject> unorderedChanceCards;
    // Used to show the card to the player
    public Transform playerCameraTrans;
      
    
    // --------------- Private Vars -----------------------------------------------------------------------------------
    // Used for gameobject offsets for cards
    private float[] offsets = new float[] {0.07f, 0.14f, 0.21f, 0.28f, 0.35f, 0.42f};
    // Used to hold the shuffled deck along with any pop operations
    private List<GameObject> shuffledCommunityCards;
    private List<GameObject> shuffledChanceCards;
    
    
    
    // --------------- Callable Member Functions ------------------------------------------------------------------------
    private void Start()
    {
        Shuffle("community");
        Shuffle("chance");
    }

    
    // Usage: deck="chance" or "community" to draw a card from the respective deck
    // Draw a card from specified deck, show to camera, then set aside
    //      (if get out of jail card, use icon for UI)
    // TODO : Need to implement UI "OK" prompt then discard the card!
    private void DrawAndShowCard(string deck)
    {
        // copy reference
        List<GameObject> deckStack;
        if(deck == "community")
        {
            deckStack = shuffledCommunityCards;
        }
        else //deck =="chance"
        {
            deckStack = shuffledChanceCards;
        }
            
        // grab and pop a card off of the respective deck
        GameObject removedCard = deckStack[deckStack.Count - 1];
        deckStack.RemoveAt(deckStack.Count - 1);
        
        // check if deck is empty and shuffle if so!
        if(deckStack.Count == 0)
        {
            Shuffle(deck);
        }
 
        // pop off card of corect deck within unity visuals
        Transform remCardTrans = removedCard.GetComponent<Transform>();
        remCardTrans.position = new Vector3(0f, remCardTrans.position.y+2, 0f);
        
        // Show our card to the camera
        remCardTrans.LookAt(playerCameraTrans, -playerCameraTrans.up);
        
        // now move to "under" the board! (discard?)
        
    }

    
    
    
    
    
    
    
    
    // --------------- Helper Functions (Never Directly call these) --------------------------------------------------------
    private List<GameObject> DeepCopyList(List<GameObject> li)
    {
        List<GameObject> returnable = new List<GameObject>(li.Count);
        
        foreach(GameObject g in li)
        {
            returnable.Add(g);
        }
        
        return returnable;
    }
    
    
    // Shuffle the given deck. This is called only when the given deck is empty.
    private void Shuffle(string deck)
    {
        // copy reference
        List<GameObject> deckStack;
        List<GameObject> unorderedDeckStack;
        if(deck == "community")
        {
            deckStack = shuffledCommunityCards;
            unorderedDeckStack = unorderedCommunityCards;
        }
        else //deck =="chance"
        {
            deckStack = shuffledChanceCards;
            unorderedDeckStack = unorderedChanceCards;
        }
        
        
        // SHUFFLE OUR ARRAYS ----------------------------------
        // Deep copy our array
        List<GameObject> deckClone = DeepCopyList(unorderedDeckStack);
        
        // Shuffle the deep copy
        for (int i = 0; i < deckClone.Count; i++)
        {
             int rnd = Random.Range(0, deckClone.Count);
             GameObject temp = deckClone[rnd];
             deckClone[rnd] = deckClone[i];
             deckClone[i] = temp;
         }
        
        // UPDATE THE VISUAL ORDER IN UNITY ----------------------------------
        if(deck == "community")
        {
            // reassign shuffled deck
            shuffledCommunityCards = deckClone;
            for(int i = 0; i < shuffledCommunityCards.Count; i++)
            {
                // get the transform for this card and use the offsets to put it in the correct deck pos
                Transform thisCard = shuffledCommunityCards[i].GetComponent<Transform>();
                thisCard.position = new Vector3(thisCard.position.x, offsets[i], thisCard.position.z);
            }
        }
        else if(deck == "chance")
        {
            // reassign shuffled deck
            shuffledChanceCards = deckClone;
            for(int i = 0; i < shuffledChanceCards.Count; i++)
            {
                // get the transform for this card and use the offsets to put it in the correct deck pos
                Transform thisCard = shuffledChanceCards[i].GetComponent<Transform>();
                thisCard.position = new Vector3(thisCard.position.x, offsets[i], thisCard.position.z);
            }
        }
        
    }
    
}
