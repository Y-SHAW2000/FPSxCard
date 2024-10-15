using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomUI : MonoBehaviour,IInRoomCallbacks
{
    Transform startTf;
    Transform contentTf;
    GameObject roomPrefab;
    public List<RoomItem> roomList;
    private void Awake()
    {
        roomList = new List<RoomItem>();
        contentTf = transform.Find("bg/Content");
        roomPrefab = transform.Find("bg/roomItem").gameObject;
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        startTf = transform.Find("bg/startBtn");
        startTf.GetComponent<Button>().onClick.AddListener(onStartBtn);

        PhotonNetwork.AutomaticallySyncScene = true; //PhotonNetwork.LoadLevelを実行する際に、ほかのプレイヤーも実行する
    }
    void Start()
    {
        //ルームのプレイヤーコラムを生成
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player p = PhotonNetwork.PlayerList[i];
            CreateRoomItem(p);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    //プレイヤー生成
    public void CreateRoomItem(Player p)
    {
        GameObject obj = Instantiate(roomPrefab, contentTf);
        obj.SetActive(true);
        RoomItem item = obj.AddComponent<RoomItem>();
        item.owerId = p.ActorNumber; //プレイヤーnumber
        roomList.Add(item);

        object val;
        if(p.CustomProperties.TryGetValue("IsReady", out val))
        {
            item.IsReady = (bool)val;
        }
    }

    //ルームから退室したプレイヤーを削除
    public void DeleteRoomItem(Player p)
    {
        RoomItem item = roomList.Find((RoomItem _item) => { return p.ActorNumber == _item.owerId; });
        if(item != null)
        {
            Destroy(item.gameObject);
            roomList.Remove(item);
        }
    }

    //Close
    void onCloseBtn()
    {
        //接続切断
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
    }

    void onStartBtn()
    {
        //load scene &　ルームにいるほかのプレイヤー達もload scene
        PhotonNetwork.LoadLevel("game");
    }

    //新しいプレイヤーがルームに入ったら
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreateRoomItem(newPlayer);
    }

    //あるプレイヤーが退室したら
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        DeleteRoomItem(otherPlayer);
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

    }

    //プレイヤーカスタマイズパラメータ更新Callback
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        RoomItem item = roomList.Find((_item) => { return _item.owerId == targetPlayer.ActorNumber; });
        if(item != null)
        {
            item.IsReady = (bool)changedProps["IsReady"];
            item.ChangeReady(item.IsReady);
        }

        //Host Playerだったら　ほかのプレイヤーたちの準備状態を確認する
        if(PhotonNetwork.IsMasterClient)
        {
            bool isAllReady = true;
            for(int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].IsReady == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            startTf.gameObject.SetActive(isAllReady); //ゲーム開始ボタンを表示する
        }
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {

    }
}
