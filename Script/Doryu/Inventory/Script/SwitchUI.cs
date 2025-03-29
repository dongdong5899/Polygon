using UnityEngine;

public class SwitchUI : MonoBehaviour
{
    [SerializeField] private GameObject _creaft, _inventory;

    private bool _isOn;

    private void Start()
    {
        _creaft.SetActive(false);
        _inventory.SetActive(false);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    _isOn = !_isOn;
        //    if (_isOn)
        //    {
        //        _creaft.SetActive(true);
        //        _inventory.SetActive(false);
        //    }
        //    else
        //    {
        //        _creaft.SetActive(false);
        //        _inventory.SetActive(false);
        //    }
        //}
        //if (_isOn && Input.GetKeyDown(KeyCode.Tab))
        //{
        //    _creaft.SetActive(!_creaft.activeSelf);
        //    _inventory.SetActive(!_inventory.activeSelf);
        //}
    }
}
