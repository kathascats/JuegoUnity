using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static string actionText;
    public static string commandText;
    public static bool uiActive;
    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject commandBox;
    [SerializeField] GameObject interactCross;

    void Update()
    {
        if (uiActive == true)
        {
            if (actionBox != null) actionBox.SetActive(true);
            if (commandBox != null) commandBox.SetActive(true);
            if (interactCross != null) interactCross.SetActive(true);

            if (actionBox != null)
            {
                var t = actionBox.GetComponent<TMPro.TMP_Text>();
                if (t != null) t.text = actionText;
            }

            if (commandBox != null)
            {
                var t2 = commandBox.GetComponent<TMPro.TMP_Text>();
                if (t2 != null) t2.text = "[E] " + commandText;
            }
        }
        else
        {
            if (actionBox != null) actionBox.SetActive(false);
            if (commandBox != null) commandBox.SetActive(false);
            if (interactCross != null) interactCross.SetActive(false);
        }
    }
}