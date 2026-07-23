using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
