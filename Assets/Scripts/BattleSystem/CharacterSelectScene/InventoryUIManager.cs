using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [Header("캐릭터 데이터 리스트 (여기에 10마리 등록)")]
    public List<CharacterData> characterDatas = new List<CharacterData>();

    [Header("UI 연결")]
    public GameObject cardPrefab;      // 방금 만든 [Step 1]의 카드 프리팹
    public Transform scrollContent;    // Scroll View -> Viewport -> Content 오브젝트 위치

    private void Start()
    {
        RefreshInventory();
    }

    /// <summary>
    /// 데이터를 기반으로 화면에 인벤토리 리스트를 그리는 함수
    /// </summary>
    public void RefreshInventory()
    {
        // 혹시 기존에 생성되어 있던 UI가 있다면 싹 지워줍니다.
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }
        // 10마리의 데이터를 하나씩 돌면서 카드를 생성합니다.
        foreach (CharacterData data in characterDatas)
        {

            if (data == null) continue;

            // 1. Content 부모 아래에 카드 프리팹을 하나 복사해 만듭니다.
            GameObject newCard = Instantiate(cardPrefab, scrollContent);



            // 2. 카드에 붙어있는 스크립트를 가져옵니다.
            CharacterCardUI cardUI = newCard.GetComponent<CharacterCardUI>();
            CharacterCard card = newCard.GetComponent<CharacterCard>();

            // 3. 데이터를 주입해서 화면을 그리게 합니다.
            if (cardUI != null)
            {
                cardUI.Setup(data);
                card.Setup(data);
            }
        }
    }
}