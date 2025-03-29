using DG.Tweening;
using UnityEngine;

public class BoxCaster2D : Caster2D
{
    public Vector2 size;

    public override bool CheckCollision(out RaycastHit2D[] hits, LayerMask whatIsTarget, Vector2 moveTo = default)
    {
        hits = Physics2D.BoxCastAll(transform.position + transform.rotation * offset,
                            size, transform.eulerAngles.z, moveTo.normalized, moveTo.magnitude, whatIsTarget);
        return hits.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(offset, size);
    }
}
