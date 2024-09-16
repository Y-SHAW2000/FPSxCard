using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class CardDisplay : MonoBehaviour
{
    public Card card;                           // 適用するカード
    public TextMeshProUGUI nameText;            // カード名
    public TextMeshProUGUI type;                // カード種類
    public Image artworkImage;                  // カードイメージ
    public TextMeshProUGUI descriptionText;     // カード説明


    void Start()
    {
        foreach (var textMeshProUGUI in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (textMeshProUGUI.name == "Text_CardName")
            {
                nameText = textMeshProUGUI;
            }
            else if (textMeshProUGUI.name == "Text_Type")
            {
                type = textMeshProUGUI;
            }
            else if (textMeshProUGUI.name == "Text_Description")
            {
                descriptionText = textMeshProUGUI;
            }
        }
        artworkImage = GetComponentInChildren<Image>(true);
        nameText.text = card.cardName;
        type.text = card.type;
        artworkImage.sprite = card.artwork;
        descriptionText.text = card.description;
    }
}
