using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField]
    RectTransform icon = null;
    Vector3 startPos;
    [SerializeField]
    Skill skill = null;
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
            ActionButton b = h.transform.GetComponent<ActionButton>();
            if (b != null)
            {
                b.SetSkill(skill);
                break;
            }
        }
        icon.position = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = icon.transform.position;
        if (skill != null)
            icon.GetComponent<UnityEngine.UI.Image>().sprite = skill.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
