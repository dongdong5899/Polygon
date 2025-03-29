using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using YH.Players;
using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class SkillSystemUI : MonoBehaviour, IWindowPanel
{
    [Range(3, 25)]
    public int currentPointCnt = 3;
    public int LayerCount { get; private set; } = 1;

    [SerializeField] private SkillSO _initializeSkill;

    [SerializeField] private GameObject _compressionBtn;
    [SerializeField] private Image _rectangleImg;
    [SerializeField] private PlayerSkillSystem _skillSystem;
    [SerializeField] private SkillLayerSelectUI _skillLayerSelector;
    [SerializeField] private SelectedSkillUI _skillEquip;
    [SerializeField] private Inventory _skillInventory;
    [SerializeField] private PlayerInputSO _player;

    [SerializeField] private Button _titleButton;
 
    private bool isOpen = false;

    public RectTransform RectTrm => transform as RectTransform;

    private void Awake()
    {
        (_titleButton.transform as RectTransform).anchoredPosition = new Vector2(-300, -40);
        RectTrm.anchoredPosition = new Vector2(350, 0);
        isOpen = false;

        _player.InventoryEvent += HandleInventoryEvent;
        //SetSlotAndRectangle(3);

        _titleButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale / 200;

            SceneManager.LoadScene("TitleScene");
        });
    }

    private void Start()
    {
        SetSlotAndRectangle(3);
        InventoryManager.Instance.AddItem(EInventory.Equip, _initializeSkill);
    }

    private void OnDestroy()
    {
        _player.InventoryEvent -= HandleInventoryEvent;
    }

    private void HandleInventoryEvent()
    {
        if (GameManager.Instance.IsGameOver) return;

        if (isOpen)
        {
            Close();
            isOpen = false;
        }
        else
        {
            Open();
            isOpen = true;
        }
    }

    public void CompressionRectangle()
    {
        DOTween.To(() => (float)currentPointCnt,
            value => SetSlotAndRectangle(value)
            , 3f, 1f).SetUpdate(true).OnComplete(Open);


        PlayerManager.Instance.ReShape();
        _compressionBtn.SetActive(false);
        AddLayer();
    }

    public void AddLayer()
    {
        LayerCount++;
        _skillLayerSelector.AddLayer();
        _skillEquip.AddLayer();
    }

    public void Open()
    {
        Time.timeScale = 0.02f;
        Time.fixedDeltaTime = Time.timeScale / 200;
        currentPointCnt = PlayerManager.Instance.CurrentPlayerPoint;

        RectTrm.DOAnchorPosX(-350, 0.2f).SetUpdate(true);

        (_titleButton.transform as RectTransform).DOKill();
        (_titleButton.transform as RectTransform).DOAnchorPos(new Vector2(50, -40), 0.2f).SetUpdate(true);

        SetSlotAndRectangle(currentPointCnt);

        _skillLayerSelector.Init();
        _compressionBtn.SetActive(currentPointCnt >= 8);
    }

    public void Close()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale / 200;
        RectTrm.DOAnchorPosX(350, 0.2f).SetUpdate(true);
        _skillEquip.AddSkillToSkillSystem();


        (_titleButton.transform as RectTransform).DOKill();
        (_titleButton.transform as RectTransform).DOAnchorPos(new Vector2(-300, -40), 0.2f).SetUpdate(true);
    }

    private void SetSlotAndRectangle(float value)
    {
        Sprite rectangle = PolygonGenerator.GeneratePolygonSprite(value, 5, Color.white);
        _rectangleImg.sprite = rectangle;

        Vector2[] vertices = rectangle.vertices;
        List<Vector2> positions = new List<Vector2>();

        for (int i = 1; i < vertices.Length; i++)
        {
            Vector2 position = (vertices[i] / 5) * (_rectangleImg.rectTransform.sizeDelta / 2 + (Vector2.one * 60));
            positions.Add(position);
        }

        _skillEquip.SetSlotCnt(positions.Count, positions);
    }
}
