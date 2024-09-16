using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class CardDisplay : MonoBehaviour
{
    public Card card;                           // �K�p����J�[�h
    public TextMeshProUGUI nameText;            // �J�[�h��
    public TextMeshProUGUI type;                // �J�[�h���
    public Image artworkImage;                  // �J�[�h�C���[�W
    public TextMeshProUGUI descriptionText;     // �J�[�h����


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
