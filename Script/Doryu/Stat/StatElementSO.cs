using UnityEngine;

namespace Doryu.StatSystem
{
    //Stat의 변하지 않는 값을 가지고있음
    [CreateAssetMenu(fileName = "StatElement", menuName = "SO/Stat/StatElement")]
    public class StatElementSO : ScriptableObject
    {
        public string statName;
        public string displayName;
        public Vector2 minMaxValue;
        public Sprite statIcon;
    }
}