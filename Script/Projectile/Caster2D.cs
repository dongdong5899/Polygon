using UnityEngine;
using YH.Entities;

public abstract class Caster2D : MonoBehaviour
{
    public Vector2 offset;
    public abstract bool CheckCollision(out RaycastHit2D[] hits, LayerMask whatIsTarget, Vector2 moveTo = default);
}
