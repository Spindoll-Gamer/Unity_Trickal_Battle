using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    private DamageTextPool pool;

    public void Setup(float damage, DamageTextPool sourcePool)
    {
        pool = sourcePool;
        // 숫자를 <sprite name="1"> 형태로 변환
        string damageStr = ((int)damage).ToString();
        string spriteText = "";
        foreach (char c in damageStr)
        {
            spriteText += $"<sprite name=\"{c}\">";
        }

        tmpText.text = spriteText;

        // 연출 시작
        StopAllCoroutines();
        StartCoroutine(ActionRoutine());
    }

    IEnumerator ActionRoutine()
    {
        // 메이플 느낌: 살짝 위로 튀어올랐다 사라짐
        float elapsed = 0;
        float duration = 0.8f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float yOffset = (elapsed / duration) * 0.3f; // 포물선
            transform.position = startPos + new Vector3(0, yOffset, 0);

            // 투명도 조절 (TMP는 Vertex Color로 조절 가능)
            tmpText.alpha = 1 - (elapsed / duration);
            yield return null;
        }

        pool.ReturnToPool(this);
    }
}