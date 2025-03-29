using ObjectPooling;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageText : MonoBehaviour, IPoolable
{
    public GameObject GameObject { get => gameObject; set { } }
    [field: SerializeField] public PoolingType PoolType { get; set; }

    [SerializeField] private TextMeshPro _text;

    public void Init()
    {
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 2;
    }

    private IEnumerator TextDraw(string text, float delay)
    {
        WaitForSeconds ws = new WaitForSeconds(delay);
        _text.maxVisibleCharacters = 0;
        for (int i = 0; i < text.Length; i++)
        {
            yield return ws;
            _text.maxVisibleCharacters++;
        }

        yield return new WaitForSeconds(1.2f);

        _text.firstVisibleCharacter = 0;
        for (int i = 0; i < text.Length; i++)
        {
            yield return ws;
            _text.maxVisibleCharacters++;
        }

        PoolManager.Instance.Push(this);
    }

    public void Setting(int damage, bool isCritiacl, Vector3 pos)
    {
        transform.position = pos + Random.insideUnitSphere * 0.5f;
        _text.color = isCritiacl ? Color.yellow : Color.white;
        _text.fontSize = isCritiacl ? 9 : 7;
        _text.text = damage.ToString();
        StartCoroutine(TextDraw(damage.ToString(), 0.07f));
    }
}
