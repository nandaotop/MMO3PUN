using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField]
    RectTransform icon = null;
    [SerializeField]
    Equip equip = null;
    [SerializeField]
    float lenght = 10;

    public void OnDrag(PointerEventData eventData)
    {
        icon.position = Input.mousePosition;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Vector3 origin = icon.position + (icon.forward * lenght);
        Vector3 direction = icon.position + (-icon.forward * lenght);
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);
        foreach (var h in hits)
        {
        }
        icon.localPosition = Vector3.zero;
    }

    public void Init(Equip equip)
    {
        this.equip = equip;
        icon.GetComponent<UnityEngine.UI.Image>().sprite = equip.sprite;
    }
}
