using Dialog;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScene : MonoBehaviour
{
    [SerializeField] private DialogPlayer _dialogPalyer;

    private void Start()
    {
        _dialogPalyer.StartDialog();
        _dialogPalyer.OnCompleteDialog += HandleCompleteTutorial;
    }

    private void HandleCompleteTutorial()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
