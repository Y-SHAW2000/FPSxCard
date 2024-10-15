using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    InputField roomNameInput; //ルーム名前

    void Start()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(onCreateBtn);
        roomNameInput = transform.Find("bg/InputField").GetComponent<InputField>();

        //ランダムルームの名前を生成
        roomNameInput.text = "room_" + Random.Range(1, 9999);
    }

    //ルームを作る
    public void onCreateBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("Creating...");
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 12; //ルームの最大プレイヤー数
        PhotonNetwork.CreateRoom(roomNameInput.text, room);//1.ルームの名前　2.ルームのオブジェクト
    }

    public void onCloseBtn()
    {
        Game.uiManager.CloseUI(gameObject.name);
    }

    //ルーム作った後のCallback
    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room Successfully!");
        Game.uiManager.CloseAllUI();
        //RoomUIを表示する
        Game.uiManager.ShowUI<RoomUI>("RoomUI");
    }

    //ルーム作る失敗した後のCallback
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Game.uiManager.CloseUI("MaskUI");
    }

}
