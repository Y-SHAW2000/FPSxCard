using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.VisualScripting;

//LobbyUI
public class LobbyUI : MonoBehaviourPunCallbacks
{
    TypedLobby lobby; //Lobbyオブジェクト

    private Transform contentTf;
    private GameObject roomPrefab;

    void Start()
    {
        transform.Find("content/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("content/createBtn").GetComponent<Button>().onClick.AddListener(onCreateRoomBtn);
        transform.Find("content/updateBtn").GetComponent<Button>().onClick.AddListener(onUpdateRoomBtn);
        contentTf = transform.Find("content/Scroll View/Viewport/Content");
        roomPrefab = transform.Find("content/Scroll View/Viewport/item").gameObject;

        lobby = new TypedLobby("fpsLobby", LobbyType.SqlLobby); //1.Lobby名前　2.Lobbyタイプ
        //lobbyに入る
        PhotonNetwork.JoinLobby(lobby);
    }

    //Lobbyに入ったらCallback関数
    public override void OnJoinedLobby()
    {
        Debug.Log("Joining Lobby...");
    }

    //ルームを作る
    public void onCreateRoomBtn()
    {
        Game.uiManager.ShowUI<CreateRoomUI>("CreateRoomUI");
    }

    //LobbyUIを閉じる
    public void onCloseBtn()
    {
        //接続を切断する
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        //LoginUIを表示
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
    }

    //ルームリストを更新
    public void onUpdateRoomBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("Updating...");

        PhotonNetwork.GetCustomRoomList(lobby, "1"); //この関数終わったらOnRoomListUpdateをCallback
    }

    //contentTf中のすべての内容を削除
    private void ClearRoomList()
    {
        while(contentTf.childCount != 0)
        {
            DestroyImmediate(contentTf.GetChild(0).gameObject);
        }
    }
    //ルーム更新後のCallback
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Game.uiManager.CloseUI("MaskUI");
        
        Debug.Log("Room Updated");

        ClearRoomList();

        for(int i = 0; i < roomList.Count; i++)
        {
            GameObject obj = Instantiate(roomPrefab, contentTf);
            obj.SetActive(true);
            string roomName = roomList[i].Name; //ルーム名前
            obj.transform.Find("roomName").GetComponent<Text>().text = roomName;
            obj.transform.Find("joinBtn").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Debug.Log(roomName);
                Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("Joining...");

                PhotonNetwork.JoinRoom(roomName); //ルームに参加
            });
        }
    }

    public override void OnJoinedRoom()
    {
        //JoinedRoom Callback
        Game.uiManager.CloseAllUI();
        Game.uiManager.ShowUI<RoomUI>("RoomUI");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //ルーム参加失敗の場合
        Game.uiManager.CloseUI("MaksUI");
    }
}
