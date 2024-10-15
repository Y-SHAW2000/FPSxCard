using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossUI : MonoBehaviour
{
    public System.Action onClickCallBack;

    void Start()
    {
        transform.Find("resetBtn").GetComponent<Button>().onClick.AddListener(onClickBtn);
    }

    public void onClickBtn()
    {
        if(onClickCallBack != null)
        {
            onClickCallBack();
        }
        Game.uiManager.CloseUI(gameObject.name);
    }

}
