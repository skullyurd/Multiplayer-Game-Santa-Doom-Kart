using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{

    [SerializeField] private bool chosenFullmobile;
    [SerializeField] private bool chosenFullMotion;
    [SerializeField] private bool chosenHalfMotion;

    [SerializeField] private bool panelOn;

    [SerializeField] private GameObject optionPanel;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OpenAndCloseOptionPanel()
    {
        switch (panelOn)
        {
            case true:
                optionPanel.SetActive(true);
                break;

            case false:
                optionPanel.SetActive(false);
                break;
        }

    }
}
