using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private void Awake()
    {
        //マウスを消す
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //すべてのUIを消す
        Game.uiManager.CloseAllUI();
        //FightUIを表示する
        Game.uiManager.ShowUI<FightUI>("FightUI");

        Transform pointTf = GameObject.Find("Point").transform;

        Vector3 pos = pointTf.GetChild(Random.Range(0, pointTf.childCount)).position;

        //プレイヤー生成
        PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
    }
}
