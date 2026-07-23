using UnityEngine;
using UnityEngine.UI;
using static BattleEnums;
using TMPro;

public class CharacterCardUI : MonoBehaviour
{
    [Header("연결할 UI 컴포넌트들")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI roleAndRowText;
    public Image iconImage;

    public CharacterData cardData { get; private set; }

    /// <summary>
    /// 매니저가 캐릭터 데이터를 주입해주면 UI를 갱신하는 함수
    /// </summary>
    public void Setup(CharacterData data)
    {
        cardData = data;

        // 글자 갱신
        nameText.text = data.characterName;
        roleAndRowText.text = $"{data.unitRole}\n{data.positionRow}";

        // 스프라이트 이미지가 배정되어 있다면 이미지 변경
        if (data.iconSprite != null)
        {
            iconImage.sprite = data.iconSprite;
        }
    }
}