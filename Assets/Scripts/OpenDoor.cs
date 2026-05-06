using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] bool canOpen;
    [SerializeField] GameObject theDoor;

    void Update()
    {
        if (canOpen == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(OpeningDoor());
            }
        }
    }
    void OnMouseOver()
    {
        if (PlayerCasting.distanceFromTarget < 5)
        {
            canOpen = true;
            UIController.actionText = "Open Door";
            UIController.commandText = "Open";
            UIController.uiActive = true;
        }
        else
        {
            canOpen = false;
            UIController.actionText = "";
            UIController.commandText = "";
            UIController.uiActive = false;
        }
    }
    void OnMouseExit()
    {
        canOpen = false;
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;
    }

    IEnumerator OpeningDoor()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2);
    }

}
