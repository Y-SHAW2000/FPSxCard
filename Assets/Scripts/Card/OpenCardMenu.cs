using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCardMenu : MonoBehaviour
{
    public GameObject Camera;
    public KeyCode openMenuKey = KeyCode.F1;
    public GameObject CardMenu;
    private bool isOpeningCardMenu = false;

    void Update()
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        if(!isOpeningCardMenu)
        {
            if (Input.GetKeyDown(openMenuKey))
            {
                CardMenu.SetActive(true);
                Camera.GetComponent<MouseLook>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isOpeningCardMenu = true;
            }
        }
        else if(isOpeningCardMenu)
        {
            if (Input.GetKeyDown(openMenuKey))
            {
                CardMenu.SetActive(false);
                Camera.GetComponent<MouseLook>().enabled = true;
                isOpeningCardMenu = false;
            }
        }
        
        
    }
}
