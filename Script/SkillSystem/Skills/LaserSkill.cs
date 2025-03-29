using Doryu.StatSystem;
using ObjectPooling;
using System;
using UnityEngine;
using YH.Entities;
using YH.Players;

public class LaserSkill : Skill
{
    private Player _player;
    private StatElement _damageStat;
    private EntityRenderer _renderer;
    private LayerMask _whatIsEnemy;
    private Laser _laser;
    public override void Init(SkillSO skillSO, Entity owner)
    {
        base.Init(skillSO, owner);
        _whatIsEnemy = LayerMask.GetMask("Enemy");
        _player = PlayerManager.Instance.Player;
        _damageStat = _player.GetCompo<StatCompo>().GetElement("Damage");
        _renderer = _player.GetCompo<EntityRenderer>(true);
    }

    public override void OnUseSkill()
    {
        _laser = PoolManager.Instance.Pop(PoolingType.PlayerLaser) as Laser;
        AudioManager.Instance.PlaySound(EAudioName.Laser);
        MoveLaser();
    }

    private void MoveLaser()
    {
        float rotationDeg = 360 / PlayerManager.Instance.CurrentPlayerPoint * _edge;
        rotationDeg += _renderer.Direction;
        Vector3 dir = new Vector3(
        Mathf.Cos((rotationDeg + 90) * Mathf.Deg2Rad),
        Mathf.Sin((rotationDeg + 90) * Mathf.Deg2Rad), 0).normalized;
        dir *= _owner.transform.localScale.x;

        Vector3 position = _owner.transform.position + dir;

        int damage = Mathf.CeilToInt(_damageStat.Value);
        _laser.Setting(PlayerManager.Instance.Player, _whatIsEnemy, damage);
        _laser.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotationDeg));
    }

    public override void OnUpdate()
    {
        if(_laser!=null&&_laser.gameObject.activeInHierarchy)
        {
            MoveLaser();
        }
        base.OnUpdate();
    }

    public override Skill Clone()
    {
        Skill skill = new LaserSkill();
        return skill;
    }
}
