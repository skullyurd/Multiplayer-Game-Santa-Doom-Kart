using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{

    [SerializeField] private Button shootButton;
    [SerializeField] private Shootbullet playerShootbullet_Script;
    

    private void OnDisable()
    {
        shootButton.onClick.RemoveListener(onShootClick);
    }

    void onShootClick()
    {
        if(playerShootbullet_Script != null)
        {
            playerShootbullet_Script.shoot();
            return;
        }
        Debug.Log("didn't find the script bud");
    }

    public void SetShootButton(Button button)
    {
        this.shootButton = button;
        shootButton.onClick.AddListener(onShootClick);
    }

}