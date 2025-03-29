using UnityEngine;

namespace Doryu.StatSystem
{
    //Stat�� ������ �ʴ� ���� ����������
    [CreateAssetMenu(fileName = "StatElement", menuName = "SO/Stat/StatElement")]
    public class StatElementSO : ScriptableObject
    {
        public string statName;
        public string displayName;
        public Vector2 minMaxValue;
        public Sprite statIcon;
    }
}