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
    // most recent cards drawn
    public GameObject mostRecentCardDrawn;
    public string mostRecentCardDrawnName;
    //public GameObject okButton;
    
      
    
    // --------------- Private Vars -----------------------------------------------------------------------------------
    // Used for gameobject offsets for cards
    private float[] offsets = new float[] {0.07f, 0.14f, 0.21f, 0.28f, 0.35f, 0.42f};
    // Used to hold the shuffled deck along with any pop operations
    private List<GameObject> shuffledCommunityCards;
    private List<GameObject> shuffledChanceCards;
    private Quaternion standardCardRotation; 
    
    
    
    // --------------- Callable Member Functions ------------------------------------------------------------------------
    private void Start()
    {
        standardCardRotation = unorderedCommunityCards[0].GetComponent<Transform>().rotation;
        
        Shuffle("community");
        Shuffle("chance");

        //okButton.SetActive(false);
        
        //DrawAndShowCard("chance");
        //Debug.Log(mostRecentCardDrawnName);
    }

    
    // Usage: deck="chance" or "community" to draw a card from the respective deck
    // Draw a card from specified deck, show to camera, then set aside
    //      (if get out of jail card, use icon for UI)
    // TODO : Need to implement UI "OK" prompt then discard the card!
    public void DrawAndShowCard(string deck)
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
 
        // pop off card of corect deck within unity visuals
        Transform remCardTrans = removedCard.GetComponent<Transform>();
        remCardTrans.position = new Vector3(0f, remCardTrans.position.y+2, 0f);
        
        // Show our card to the camera
        remCardTrans.LookAt(playerCameraTrans, -playerCameraTrans.up);
        
        mostRecentCardDrawn = removedCard;
        mostRecentCardDrawnName = removedCard.name;

        //okButton.SetActive(true);
    }
    
    
    // Should be called after player is done looking at card
    // some sort of UI "OK" button must be clicked for this to be called
    public void StopLookingAtCard()
    {
        // move the card that is not being looked at down
        // it will be moved back when shuffle is called when no more cards remain!
        Transform cardTrans = mostRecentCardDrawn.GetComponent<Transform>();
        cardTrans.position = new Vector3(cardTrans.position.x, cardTrans.position.y-100, cardTrans.position.x);
        cardTrans.rotation = standardCardRotation;
        
        //okButton.SetActive(false);

        // check if deck is empty and shuffle if so!
        if(shuffledCommunityCards.Count == 0)
        {
            Shuffle("community");
        }
        if(shuffledChanceCards.Count == 0)
        {
            Shuffle("chance");
        }

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
                thisCard.position = new Vector3(thisCard.position.x, offsets[i], 3f);
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
                thisCard.position = new Vector3(thisCard.position.x, offsets[i], -3f);
            }
        }
        
    }
    
}
