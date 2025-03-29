using TMPro;
using UnityEngine;

public class PlayerLevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        PlayerManager.Instance.OnChangedPlayerLevelEvent += HandleChangedPlayerLevelEvent;
        HandleChangedPlayerLevelEvent(PlayerManager.Instance.CurrentPlayerLevel);
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnChangedPlayerLevelEvent -= HandleChangedPlayerLevelEvent;
    }

    private void HandleChangedPlayerLevelEvent(int value)
    {
        _text.text = RomeNumber.GetNumber(value);
    }
}
