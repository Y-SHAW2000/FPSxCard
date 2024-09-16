using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<GameObject> cardDeck;  // プレイヤーカードデッキ（組んだ）
    public List<Transform> handArea;   // 手札エリア

    void Start()
    {
        DrawCard(3);  // ゲーム始める前に3枚のカードをドロー

    }

    public void DrawCard(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            if (cardDeck.Count > 0)
            {
                // ランダムカードを1枚ドロー
                int randomIndex = Random.Range(0, cardDeck.Count);
                GameObject drawnCard = cardDeck[randomIndex];

                //生成したカードをデッキから削除
                cardDeck.RemoveAt(randomIndex);

                // カードの生成およびInfoを設置
                GameObject newCard = Instantiate(drawnCard, handArea[i]);
                CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            }
        }
    }

}
