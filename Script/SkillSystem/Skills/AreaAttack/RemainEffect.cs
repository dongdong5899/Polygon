using Doryu.StatSystem;
using UnityEngine;
using YH.Entities;

public class RemainEffect : MonoBehaviour
{
    private Caster2D _caster;

    [SerializeField] private float _tickDelay = 0.1f;
    [SerializeField] private LayerMask _whatIsTarget;
    private RaycastHit2D[] _hits;
    private float _prevTick;
    private StatElement _damage;


    private void Awake()
    {
        _prevTick = Time.time;
        _caster = GetComponent<Caster2D>();
        _damage = PlayerManager.Instance.Player.GetCompo<StatCompo>().GetElement("Damage");
    }

    private void FixedUpdate()
    {
        if (_caster.CheckCollision(out _hits, _whatIsTarget))
        {
            if (_prevTick + _tickDelay >= Time.time) return;

            _prevTick = Time.time;

            foreach (var hit in _hits)
            {
                if (hit.transform.TryGetComponent(out Entity entity))
                {
                    int damage = Mathf.CeilToInt(_damage.Value / 5);
                    entity.GetCompo<HealthCompo>().ApplyDamage(PlayerManager.Instance.Player.GetCompo<StatCompo>(), damage);
                }
            }
        }
    }
}
