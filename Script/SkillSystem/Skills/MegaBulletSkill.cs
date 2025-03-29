using Doryu.StatSystem;
using UnityEngine;
using YH.Entities;
using YH.Players;

public class MegaBulletSkill : Skill
{
    private Player _player;
    private StatElement _damageStat;
    private EntityRenderer _renderer;
    private LayerMask _whatIsEnemy;
    private float _bulletSpeed = 20;

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
        Projectile projectile
            = PoolManager.Instance.Pop(ObjectPooling.PoolingType.PlayerProjectile) as Projectile;

        AudioManager.Instance.PlaySound(EAudioName.Megabullet);


        float rotationDeg = 360 / PlayerManager.Instance.CurrentPlayerPoint * _edge;
        rotationDeg += _renderer.Direction;

        Vector3 position = _owner.transform.position; //+ dir * distance;

        projectile.Setting(PlayerManager.Instance.Player, _whatIsEnemy, _bulletSpeed, Mathf.CeilToInt(_damageStat.Value * 2));
        projectile.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotationDeg));
        projectile.transform.localScale = Vector3.one * 2;
    }

    public override Skill Clone()
    {
        Skill skill = new MegaBulletSkill();
        return skill;
    }
}
