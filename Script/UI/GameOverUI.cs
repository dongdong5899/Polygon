using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button _titleBtn, _retryBtn;
    [SerializeField] private TextMeshProUGUI _point, _comp, _level, _wave;
    [SerializeField] private GameObject _gameOverPannel;

    private void Awake()
    {
        _titleBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale / 200;
            SceneManager.LoadScene("TitleScene");
        });
        _retryBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale / 200;
            SceneManager.LoadScene("GameScene");
        });
    }

    public void Open()
    {
        _point.text = "도형 변의 수 : " + PlayerManager.Instance.CurrentPlayerPoint;
        _comp.text = "도형 압축 횟수 : " + PlayerManager.Instance.CurrentPlayerReShapeCount;
        _level.text = "도달한 레벨 : " + PlayerManager.Instance.CurrentPlayerLevel;
        _wave.text = WaveManager.Instance.waveCount - 1 + " WAVE";

        _gameOverPannel.SetActive(true);
    }
}
