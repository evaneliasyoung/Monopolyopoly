using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyButtons : MonoBehaviour
{
    [SerializeField] private GameControllerSys gameController;
    [SerializeField] private bool increasing;
    public byte property;
    public void OnClick()
    {
        if (increasing)
            gameController.IncreaseProperty(property);
        else
            gameController.DecreaseProperty(property);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
