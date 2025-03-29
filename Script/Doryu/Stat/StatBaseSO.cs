using System.Collections.Generic;
using UnityEngine;

namespace Doryu.StatSystem
{
    [CreateAssetMenu(fileName = "BaseStat", menuName = "SO/Stat/BaseStat")]
    public class StatBaseSO : ScriptableObject
    {
        private Dictionary<string, StatElement> _statDictionary;
        [SerializeField] private List<StatElement> _statElements = new List<StatElement>();


        private void OnEnable()
        {
            SetupDictionary();
        }

        private void SetupDictionary()
        {
            _statDictionary = new Dictionary<string, StatElement>();
            foreach (StatElement statElement in _statElements)
            {
                if (statElement.elementSO != null)
                {
                    statElement.SetValue();
                    _statDictionary.Add(statElement.elementSO.statName, statElement);
                }
            }
        }

        public StatElement GetStatElement(string statName)
        {
            if (_statDictionary.ContainsKey(statName))
                return _statDictionary[statName];
            else
                return null;
        }
    }
}
