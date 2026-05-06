using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleTrigger : MonoBehaviour
{
    [SerializeField] GameObject WebEvent; // aparecerá en el Inspector

    void OnMouseOver()
    {
        Debug.Log("CandleTrigger: OnMouseOver distance=" + PlayerCasting.distanceFromTarget);
        if (PlayerCasting.distanceFromTarget < 5)
        {
            if (WebEvent != null) WebEvent.SetActive(true);
            UIController.actionText = "Vela";
            UIController.commandText = "Trigger";
            UIController.uiActive = true;
        }
    }

    void OnMouseExit()
    {
        Debug.Log("CandleTrigger: OnMouseExit");
        if (WebEvent != null) WebEvent.SetActive(false);
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;
    }
}