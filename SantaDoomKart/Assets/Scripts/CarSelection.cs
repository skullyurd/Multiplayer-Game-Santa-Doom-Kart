using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    [SerializeField] Sprite[] cars = new Sprite[]{};
    [SerializeField] private GameObject carSelectPanel;
    [SerializeField] private PlayerSpawn playerSpawn;
    [SerializeField] private Vector2 imageSize;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var car in cars)
        {
            GameObject NewObj = new GameObject(); //Create the GameObject
            NewObj.name = car.name;
            Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
            Button NewButton = NewObj.AddComponent<Button>();
            NewImage.sprite = car; //Set the Sprite of the Image Component on the new GameObject
            NewObj.GetComponent<RectTransform>().sizeDelta = imageSize;
            NewImage.preserveAspect = true;
            NewObj.GetComponent<RectTransform>().SetParent(carSelectPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
            NewObj.SetActive(true); //Activate the GameObject
        }
    }
}
