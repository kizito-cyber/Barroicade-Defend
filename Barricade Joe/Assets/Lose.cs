using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour
{
    public GameObject loss;
    public PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.isEnded==true)
        {
            loss.gameObject.SetActive(true);
        }
    }
    public void Redo()
    {

        player.Restart();
    }
   
}
