using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _sellButton;

    private Weapon _weapon;

    public event UnityAction<Weapon, WeaponView> SellButtonClick;
    private void OnEnable()
    {
        _sellButton.onClick.AddListener(onButtonClick);
        _sellButton.onClick.AddListener(TryLockItem);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(onButtonClick);
        _sellButton.onClick.RemoveListener(TryLockItem);
    }
    public void Render(Weapon weapon)
    {
        _weapon = weapon;

        _label.text = weapon.Label;
        _price.text = weapon.Price.ToString();
        _icon.sprite = weapon.Icon;
    }

    private void onButtonClick()
    {
        SellButtonClick?.Invoke(_weapon, this);
    }

    private void TryLockItem()
    {
        if (_weapon.IsBuyed)
        {
            _sellButton.interactable = false;
        }
    }
}
