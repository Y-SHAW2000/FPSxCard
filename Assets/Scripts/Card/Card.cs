using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;  // カード名前
    public Sprite artwork;   // カードイラスト
    public string type;      // カートタイプ
    public string limit;     // 職業/キャラ制限の有無
    [TextArea]
    public string description;  // カード説明
}
