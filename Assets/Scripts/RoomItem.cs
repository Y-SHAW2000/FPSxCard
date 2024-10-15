using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public int owerId; //プレイヤーId
    public bool IsReady = false; //準備完了しているかどうか
    void Start()
    {
        if(owerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(onReadyBtn);
        }
        else
        {
            transform.Find("Button").GetComponent<Image>().color = Color.black;
        }
    }

    public void onReadyBtn()
    {
        IsReady = !IsReady;

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();

        table.Add("IsReady", IsReady);

        PhotonNetwork.LocalPlayer.SetCustomProperties(table); //カスタマイズ`パラメータの設定

        ChangeReady(IsReady);
    }

    public void ChangeReady(bool isReady)
    {
        transform.Find("Button/Text").GetComponent<Text>().text = isReady == true ? "Ready" : "Unready";
    }


}
