using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Doryu.StatSystem
{
    //StatElementSO를 가져와서 해당 스탯의 [변화]를 표현해줌
    [Serializable]
    public class StatElement
    {
        public StatElementSO elementSO;
        [SerializeField] private float baseValue;
        private SerializedDictionary<string, float> _addModifies;
        private SerializedDictionary<string, float> _percentModifies;
        private Dictionary<string, int> _overrideAddModifyCount;
        private Dictionary<string, int> _overridePercentModifyCount;

        public event Action<float, float> OnValueChangedEvent;

        public float Value { get; private set; }

        public StatElement(float baseValue)
        {
            this.baseValue = baseValue;
            SetValue();
        }

        public void SetValue()
        {
            CheckSetting();

            //덧셈 변경사항 적용
            float numValue = baseValue;
            foreach (float addModify in _addModifies.Values)
            {
                numValue += addModify;
            }

            //퍼센트 변경사항 적용
            float totalPercentModify = 0;
            foreach (float percentModify in _percentModifies.Values)
            {
                totalPercentModify += percentModify;
            }

            float value = numValue * (1 + totalPercentModify / 100);
            if (elementSO != null)
                value = Mathf.Clamp(value, elementSO.minMaxValue.x, elementSO.minMaxValue.y);

            if (Value != value)
            {
                float prevValue = Value;
                Value = value;
                OnValueChangedEvent?.Invoke(prevValue, value);
            }
        }

        public void AddModify(string key, float modify)
        {
            CheckSetting();
            if (!_addModifies.TryAdd(key, modify))
                _addModifies[key] = modify;
            if (!_overrideAddModifyCount.TryAdd(key, 1))
                _overrideAddModifyCount[key]++;
            SetValue();
        }
        public void AddModifyPercent(string key, float modify)
        {
            CheckSetting();
            if (!_percentModifies.TryAdd(key, modify))
                _percentModifies[key] = modify;
            if (!_overridePercentModifyCount.TryAdd(key, 1))
                _overridePercentModifyCount[key]++;
            SetValue();
        }
        public void RemoveModify(string key)
        {
            CheckSetting();
            if (_overrideAddModifyCount.ContainsKey(key))
                _overrideAddModifyCount[key]--;
            else
                _overrideAddModifyCount[key] = 0;
            if (_addModifies.ContainsKey(key) && _overrideAddModifyCount[key] == 0)
                _addModifies.Remove(key);
            SetValue();
        }
        public void RemoveModifyPercent(string key)
        {
            CheckSetting();
            if (_overridePercentModifyCount.ContainsKey(key))
                _overridePercentModifyCount[key]--;
            else
                _overridePercentModifyCount[key] = 0;
            if (_percentModifies.ContainsKey(key) && _overridePercentModifyCount[key] == 0)
                _percentModifies.Remove(key);
            SetValue();
        }

        public void CheckSetting()
        {
            if (_addModifies == null)
                _addModifies = new SerializedDictionary<string, float>();
            if (_percentModifies == null)
                _percentModifies = new SerializedDictionary<string, float>();
            if (_overrideAddModifyCount == null)
                _overrideAddModifyCount = new Dictionary<string, int>();
            if (_overridePercentModifyCount == null)
                _overridePercentModifyCount = new Dictionary<string, int>();
        }
    }
}