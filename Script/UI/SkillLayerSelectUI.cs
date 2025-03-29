using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLayerSelectUI : MonoBehaviour
{
    [SerializeField] private Transform _btnParent;
    [SerializeField] private LayerButtonUI _layerBtn;
    [SerializeField] private SelectedSkillUI _skillUI;

    private List<LayerButtonUI> _layerBtns = new List<LayerButtonUI>();

    private int _layerCnt = 0;

    private int _currentLayer = 1;
    public int CurrentLayer
    {
        get => _currentLayer;
        set
        {
            _currentLayer = value;
            _skillUI.SelectLayer(_currentLayer);

            _layerBtns.ForEach(btn => btn.Close());
            _layerBtns[_currentLayer].Open();
        }
    }

    public void AddLayer()
    {
        _layerCnt++;

        LayerButtonUI layerBtn = Instantiate(_layerBtn, _btnParent);
        layerBtn.SetText(_layerCnt.ToString());
        _layerBtns.Add(layerBtn);
        layerBtn.Button.onClick.AddListener(() => CurrentLayer = layerBtn.GetLayer());
    }

    public void Init()
    {
        if (_layerCnt == 0)
            AddLayer();

        CurrentLayer = 0;
    }
}
