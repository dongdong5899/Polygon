using Doryu.StatSystem;
using System.Collections.Generic;
using UnityEngine;
using YH.Animators;

namespace YH.Entities
{
    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        private readonly int _blinkHash = Shader.PropertyToID("_Blink");

        public float FacingDirection { get; private set; } = 1;
        public float Direction { get; private set; }

        private Entity _entity;
        public Animator Animator { get; private set; }
        public List<SpriteRenderer> SpriteRendererList { get; private set; } = new List<SpriteRenderer>();
        private Material _material;

        private StatElement _sizeStat;
        [SerializeField] private StatElementSO _sizeStatSO;
        [SerializeField] private int _defaultPoint = 3;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _rotationSpeed = 5;
       
        public void Initialize(Entity entity)
        {
            _entity = entity;
            Animator = GetComponent<Animator>();
            GetComponentsInChildren(SpriteRendererList);
        }

        private void Start()
        {
            SpriteRendererList.ForEach(renderer
                => renderer.sprite = PolygonGenerator.GeneratePolygonSprite(_defaultPoint, _radius, Color.white));

            _material = SpriteRendererList[0].material;

            _sizeStat = _entity.GetCompo<StatCompo>().GetElement(_sizeStatSO);
        }

        private void Update()
        {
            transform.parent.localScale = Vector3.one * _sizeStat.Value;
        }

        public void Blink(float amount)
        {
            _material.SetFloat(_blinkHash, amount);
        }

        public void SetPolygonShape(float point, Color color)
        {
            Sprite sprire = PolygonGenerator.GeneratePolygonSprite(point, _radius, color);
            SpriteRendererList.ForEach(renderer
                => renderer.sprite = sprire);
        }

        public void SetParam(AnimParamSO param, bool value) => Animator.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => Animator.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => Animator.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param) => Animator.SetTrigger(param.hashValue);

        #region FlipControl

        public void SetRotation(Vector3 dir, bool isImmadient = false)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            if (isImmadient)
                transform.parent.rotation = targetRotation;
            else
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            Direction = angle;
        }
        #endregion


    }
}
