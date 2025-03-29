using Doryu.StatSystem;
using UnityEngine;

public static class DamageCalculator
{
    public static int DamageCalculat(float damageCoefficient, StatCompo dealerStat)
    {
        float damage = dealerStat.GetElement("Damaeg").Value;

        float random = Random.Range(0, 100);
        if (random < dealerStat.GetElement("Critical").Value)
            damage *= dealerStat.GetElement("CriticalDamage").Value / 100;

        return Mathf.RoundToInt(damage);
    }
}
