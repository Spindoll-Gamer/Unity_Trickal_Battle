using UnityEngine;
using UnityEngine.UI;
public class CharacterCard : MonoBehaviour
{

    private CharacterData targetData;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClickCard);
    }

    public void Setup(CharacterData data)
    {
        targetData = data;
    }

    private void OnClickCard()
    {
        if (targetData == null) return;

        TeamPlacementManager.Instance.TryPlaceCharacter(targetData);
    }
}
