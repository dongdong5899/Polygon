using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleObject : MonoBehaviour
{
    [SerializeField] private int _defaultPoint = 3;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private float _rotationSpeed = 5;
    private float _currentPoint;
    private Sequence _pointSeq;
    public List<SpriteRenderer> SpriteRendererList { get; private set; } = new List<SpriteRenderer>();


    private void Awake()
    {
        GetComponentsInChildren(SpriteRendererList);
    }

    private void Start()
    {
        SpriteRendererList.ForEach(renderer
               => renderer.sprite = PolygonGenerator.GeneratePolygonSprite(_defaultPoint, _radius, Color.white));
        _currentPoint = _defaultPoint;
        HandlePointChangedEvent(4);
    }

    private void Update()
    {
        transform.parent.rotation = Quaternion.Euler(0,0,7 * Time.deltaTime);
    }

    private void HandlePointChangedEvent(int pointCount)
    {
        if (_pointSeq != null && _pointSeq.IsActive()) _pointSeq.Kill();
        _pointSeq = DOTween.Sequence();
        float startValue = _currentPoint;
        _pointSeq.Append(DOTween.To(
            () => startValue,
            value =>
            {
                _currentPoint = value;
                SetPolygonShape(value, Color.white);
            },
            pointCount, 1.5f));
        _pointSeq.AppendCallback(() =>
        {
            HandlePointChangedEvent(pointCount + 1);
        });
    }



    private void SetPolygonShape(float point, Color color)
    {
        Sprite sprire = PolygonGenerator.GeneratePolygonSprite(point, _radius, color);
        SpriteRendererList.ForEach(renderer
            => renderer.sprite = sprire);
    }


}
