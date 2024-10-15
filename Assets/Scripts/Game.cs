using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game : MonoBehaviour
{
    public static UIManager uiManager;

    public static bool isLoaded = false;

    private void Awake()
    {
        if(isLoaded == true)
        {
            Destroy(gameObject);
        }
        else
        {
            isLoaded = true;
            DontDestroyOnLoad(gameObject);//Scene•Ï‚í‚Á‚Ä‚à‚±‚ÌGameObject‚Ííœ‚µ‚È‚¢
            uiManager = new UIManager();
            uiManager.Init();

            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
        }
    }
    void Start()
    {
        //Login‰æ–Ê‚ğ•\¦
        uiManager.ShowUI<LoginUI>("LoginUI");
    }


}
