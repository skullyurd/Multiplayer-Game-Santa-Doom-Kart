using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{

    [SerializeField] private float startTime;
    [SerializeField] private float countDown;
    [SerializeField] private Text timeText;

    void Start()
    {
        startTime = 296;
        countDown = Time.deltaTime * 1;
    }

    
    void Update()
    {

        startTime -= countDown;

        float t = startTime - countDown;
        string seconds = (t % 59).ToString("f0");
        string minutes = ((int)t / 59).ToString();

        timeText.text = minutes + ":" + seconds;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }

}
