using UnityEngine;
using YH.Entities;
using YH.Players;

public class AreaAttackSkill : Skill
{
    private Player _player;
    private EntityRenderer _renderer;
    private float _detectingDistance = 20;
    private LayerMask _whatIsTarget;
    private float _attckSpeed = 10f;

    public override void Init(SkillSO skillSO, Entity owner)
    {
        base.Init(skillSO, owner);
        _whatIsTarget = LayerMask.GetMask("Enemy");
        _player = PlayerManager.Instance.Player;
        _renderer = _player.GetCompo<EntityRenderer>(true);
    }

    public override void OnUseSkill()
    {
        Collider2D target = Physics2D.OverlapCircle(_owner.transform.position, _detectingDistance, _whatIsTarget);

        AreaAttack areaAttack = PoolManager.Instance.Pop(ObjectPooling.PoolingType.AreaAttack) as AreaAttack;

        AudioManager.Instance.PlaySound(EAudioName.AreaAttack);


        if (target != null)
        {
            areaAttack.Setting(target.transform.position, _attckSpeed);
        }
        else
        {
            Vector2 targetPosition = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            targetPosition.Normalize();

            targetPosition *= Random.Range(5, _detectingDistance - 3);
            areaAttack.Setting(_owner.transform.position + (Vector3)targetPosition, _attckSpeed);
        }

        float rotationDeg = 360 / PlayerManager.Instance.CurrentPlayerPoint * _edge;
        rotationDeg += _renderer.Direction;

        Vector3 dir = new Vector3(
            Mathf.Cos((rotationDeg + 90) * Mathf.Deg2Rad),
            Mathf.Sin((rotationDeg + 90) * Mathf.Deg2Rad),
             0).normalized;
        dir *= _owner.transform.localScale.x;

        Vector3 position = _owner.transform.position + dir;
        areaAttack.transform.SetPositionAndRotation(position, Quaternion.identity);
    }

    public override Skill Clone()
    {
        Skill skill = new AreaAttackSkill();
        return skill;
    }
}
