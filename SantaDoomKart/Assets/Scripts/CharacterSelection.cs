using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

enum Car
{
    threewheel,
    Sportscar
}

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private GameObject carsContainer;
    [SerializeField] private GameObject charactersContainer;
     private GameObject[] characterList;
     private GameObject[] carList;
     private int indexCar;
     private int indexCharacter;

     private void Start()
     {
      characterList = new GameObject[charactersContainer.transform.childCount];
      carList = new GameObject[carsContainer.transform.childCount];
      
      //Fill Array with models
      for (int i = 0; i < charactersContainer.transform.childCount; i++)
      {
          characterList[i] = charactersContainer.transform.GetChild(i).gameObject;
      }

      for (int i = 0; i < carsContainer.transform.childCount; i++)
      {
          carList[i] = carsContainer.transform.GetChild(i).gameObject;
      }

      //toggle off renderers in charactlist
      foreach (GameObject character in characterList)
      {
          character.SetActive(false);
      }
      
      //toggle of renders in carslist
      foreach (GameObject car in carList)
      {
          car.SetActive(false);
      }
      
      //Toggle on the first indexes
      if (characterList[0] && carList[0])
      {
          carList[0].SetActive(true);
          characterList[0].SetActive(true);
      }
     }

     public void ToggleNextCharacter(bool isLeft)
     {
            //Toggle off current model
             characterList[indexCharacter].SetActive(false);
             if (isLeft)
             {
                 indexCharacter--;
             }
             else
             {
                 indexCharacter++;
             }

             if (indexCharacter == characterList.Length)
             {
                 indexCharacter = 0;
             }
             if (indexCharacter < 0)
             {
                 indexCharacter = characterList.Length - 1;
             }
         
             //toggle on the new model
             characterList[indexCharacter].SetActive(true);
     }

     public void ToggleNextCar(bool isLeft)
     {
         //Toggle off current model
         carList[indexCar].SetActive(false);
         if (isLeft)
         {
             indexCar--;
         }
         else
         {
             indexCar++;
         }
             
         if (indexCar == carList.Length)
         {
             indexCar = 0;
         }
         if (indexCar < 0)
         {
             indexCar = carList.Length - 1;
         }
                      
         //toggle on the new model
         carList[indexCar].SetActive(true);
     }


     public void Confirm()
     {
         PlayerPrefs.SetInt("CarSelected", indexCar);
         PlayerPrefs.SetInt("PlayerSelected", indexCharacter);
         SceneManager.LoadScene("SantaDoomKart");
     }
 
}
