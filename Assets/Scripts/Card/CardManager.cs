using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<GameObject> cardDeck;  // �v���C���[�J�[�h�f�b�L�i�g�񂾁j
    public List<Transform> handArea;   // ��D�G���A

    void Start()
    {
        DrawCard(3);  // �Q�[���n�߂�O��3���̃J�[�h���h���[

    }

    public void DrawCard(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            if (cardDeck.Count > 0)
            {
                // �����_���J�[�h��1���h���[
                int randomIndex = Random.Range(0, cardDeck.Count);
                GameObject drawnCard = cardDeck[randomIndex];

                //���������J�[�h���f�b�L����폜
                cardDeck.RemoveAt(randomIndex);

                // �J�[�h�̐��������Info��ݒu
                GameObject newCard = Instantiate(drawnCard, handArea[i]);
                CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            }
        }
    }

}
