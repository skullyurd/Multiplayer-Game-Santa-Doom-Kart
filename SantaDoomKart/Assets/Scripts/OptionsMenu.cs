using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private Image mobileImage;
    [SerializeField] private Image fullMotionImage;
    [SerializeField] private Image halfMotionImage;

    [SerializeField] private InputManager inputManagerScript;

    [SerializeField] private GameObject optionPanel;

    private void Start()
    {
        mobileImage.color = Color.red;
        inputManagerScript.togglefullMobile();

        if(inputManagerScript.pc == true)
        {
            optionPanel.SetActive(false);
        }
    }

    public void toggleFullmotion()
    {
        fullMotionImage.color = Color.red;
        mobileImage.color = Color.white;
        halfMotionImage.color = Color.white;
    }

    public void toggleHalfMotion()
    {
        fullMotionImage.color = Color.white;
        mobileImage.color = Color.white;
        halfMotionImage.color = Color.red;
    }

    public void togglefullMobile()
    {
        fullMotionImage.color = Color.white;
        mobileImage.color = Color.red;
        halfMotionImage.color = Color.white;
    }
}
