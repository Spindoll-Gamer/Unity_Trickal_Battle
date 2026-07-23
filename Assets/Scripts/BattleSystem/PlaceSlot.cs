using UnityEngine;
using static BattleEnums;

public class PlaceSlot : MonoBehaviour
{
    public PositionRow slotRow;

    private GameObject currentSpriteObj;
    public CharacterData occupiedCharacter { get; private set; }
    
    public void SetCharacter(CharacterData newCharacter, int sortingLayer)
    {

        if (currentSpriteObj != null)
        {
            Destroy(currentSpriteObj);
            currentSpriteObj = null; // 빈 상자로 초기화
        }

        occupiedCharacter = newCharacter;
        if (occupiedCharacter == null) return;

        GameObject charactersprite = new GameObject("Character Sprite");
        charactersprite.transform.SetParent(this.transform);
        charactersprite.transform.localPosition = new Vector3(0f,3f,0f);
        charactersprite.transform.localRotation = Quaternion.identity;
        charactersprite.transform.localScale = new Vector3(4f, 4f, 1f);

        SpriteRenderer spriteRenderer = charactersprite.AddComponent<SpriteRenderer>();
        if (occupiedCharacter != null)
        {
            spriteRenderer.sprite = occupiedCharacter.portraitSprite;
            spriteRenderer.flipX = true;
            spriteRenderer.sortingOrder = sortingLayer+1;
        }
        currentSpriteObj = charactersprite;
    }
}