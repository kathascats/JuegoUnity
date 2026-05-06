using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MetalDoor : MonoBehaviour
{
    [SerializeField] bool canOpen;
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject theCam;
    [SerializeField] GameObject textOnScreen;



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
        if (theCam != null) theCam.SetActive(true);
        if (thePlayer != null) thePlayer.SetActive(false);
        textOnScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        textOnScreen.SetActive(false);
        if (thePlayer != null) thePlayer.SetActive(true);
        if (theCam != null) theCam.SetActive(false);
    }
}