using UnityEngine;
using YH.Entities;

public class CircleCaster2D : Caster2D
{
    public float radius;

    public override bool CheckCollision(out RaycastHit2D[] hits, LayerMask whatIsTarget, Vector2 moveTo = default)
    {
        hits = Physics2D.CircleCastAll(transform.position + transform.rotation * offset,
                            radius, moveTo.normalized, moveTo.magnitude, whatIsTarget);
        return hits.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(offset, radius);
    }
}
