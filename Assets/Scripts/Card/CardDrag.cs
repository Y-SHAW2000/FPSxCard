using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private RectTransform target;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 移動する前のポジションを記録する
        originalPosition = rectTransform.anchoredPosition;
        // dragする時透明化
        canvasGroup.alpha = 0.6f;
        // このカートとのインタフェースを禁止する，ほかのUIにdragできるために
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // カードをマウスと一緒に移動させる
        rectTransform.anchoredPosition += eventData.delta;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        // カードを放すとき、透明度とインタフェースを復元する
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 特定な場所に放さないと、元の場所に復元
        if (IsOverHandArea() && !IsOverActiveArea())
        {
            rectTransform.anchoredPosition = originalPosition;
        }
        else if(IsOverActiveArea())
        {
            //targetの子にする
            rectTransform.GetComponent<RectTransform>().SetParent(target, true);
            rectTransform.anchoredPosition = new Vector2(0,0);
            rectTransform.tag = "ActiveArea"; //tag設定
        }
        //Activeから下げる
        else if(IsOverActiveArea() && !IsOverHandArea())
        {
            return;
        }
    }

    // 目的エリアにいるかどうかを判断する
    private bool IsOverActiveArea()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.CompareTag("ActiveArea"))
            {
                target = result.gameObject.GetComponent<RectTransform>();
                Debug.Log(target.name);
                return true;
            }
        }

        return false;
    }
    private bool IsOverHandArea()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.CompareTag("HandArea"))
            {
                target = result.gameObject.GetComponent<RectTransform>();
                return true;
            }
        }

        return false;
    }
}
