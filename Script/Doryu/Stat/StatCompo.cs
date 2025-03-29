using System.Collections.Generic;
using UnityEngine;
using YH.Entities;

namespace Doryu.StatSystem
{
    public class StatCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private List<StatElement> _overrideStatElements = new List<StatElement>();
        [SerializeField] private StatBaseSO _baseStat;

        private Dictionary<string, StatElement> _overrideStatDictionary;
        private Entity _entity;


        public void Initialize(Entity entity)
        {
            _baseStat = Instantiate(_baseStat);
            _entity = entity;
            _overrideStatDictionary = new Dictionary<string, StatElement>();
            foreach (StatElement statElement in _overrideStatElements)
            {
                if (statElement.elementSO != null)
                {
                    statElement.SetValue();
                    _overrideStatDictionary.Add(statElement.elementSO.statName, statElement);
                }
            }
        }

        public StatElement GetElement(StatElementSO statType)
        {
            if (_overrideStatDictionary.TryGetValue(statType.statName, out StatElement statElement))
                return statElement;

            statElement = _baseStat.GetStatElement(statType.statName);
            if (statElement != null)
                return statElement;
            else
                return null;
        }
        public StatElement GetElement(string statName)
        {
            if (_overrideStatDictionary.TryGetValue(statName, out StatElement statElement))
                return statElement;

            statElement = _baseStat.GetStatElement(statName);
            if (statElement != null)
                return statElement;
            else
                return null;
        }
    }
}