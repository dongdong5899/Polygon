using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCanvas : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.Instance.PlaySound(EAudioName.BGM, null);
    }
    public void PlayBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void TutorialBtn()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
