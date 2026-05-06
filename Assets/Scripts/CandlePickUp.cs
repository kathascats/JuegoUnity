using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlePickUp : MonoBehaviour
{
    [SerializeField] bool canPick;
    [SerializeField] GameObject textOnScreen;
    [SerializeField] GameObject tableCandle;
    [SerializeField] GameObject handCandle;



    void Update()
    {
        if (canPick == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.GetComponent<BoxCollider>().enabled = false;
                tableCandle.SetActive(false);
                handCandle.SetActive(true);
            }
        }
    }

    void OnMouseOver()
    {
        if (PlayerCasting.distanceFromTarget < 5)
        {
            canPick = true;
            UIController.actionText = "Vela";
            UIController.commandText = "Pick up";
            UIController.uiActive = true;
        }
        else
        {
            canPick = false;
            UIController.actionText = "";
            UIController.commandText = "";
            UIController.uiActive = false;
        }
    }

    void OnMouseExit()
    {
        canPick = false;
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;
    }

  
}