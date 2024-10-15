using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Xml.Serialization;

public class LoginUI : MonoBehaviour, IConnectionCallbacks
{

    void Start()
    {
        transform.Find("startBtn").GetComponent<Button>().onClick.AddListener(onStartBtn);
        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(onQuitBtn);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this); //Pun2 Add
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this); //Pun2 remove
    }

    public void onStartBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("Connecting Server...");

        //pun2 Serverに接続
        PhotonNetwork.ConnectUsingSettings(); //実行した後OnConnectedToMasterを実行
    }

    public void onQuitBtn()
    {
        Application.Quit();
    }

    public void OnConnected()
    {

    }

    //サーバーに接続できたあと行う関数
    public void OnConnectedToMaster()
    {
        //全てのUIを閉じる
        Game.uiManager.CloseAllUI();
        //Debug.Log("Connect Successfully!");
        //LobbyUIを開く
        Game.uiManager.ShowUI<LobbyUI>("LobbyUI");
    }

    //サーバー切断の関数
    public void OnDisconnected(DisconnectCause cause)
    {
        Game.uiManager.CloseUI("MaskUI");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }
}
