using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField]
    Image icon = null;
    [SerializeField]
    Image countdownImage = null, onOffimage = null;
    [SerializeField]
    Text buttontext = null;
    ActionClass action;
    ActionController controller;

    public void SetUpButton(ActionController controller, ActionClass action)
    {
        this.action = action;
        this.controller = controller;
        buttontext.text = this.action.key.ToString();
        var skill = action.skill;
        if (skill != null)
        {
            icon.sprite = skill.sprite;
        }
    }

    public void Pressed()
    {
        controller.PressButton(action);
    }

    public void ManaCheck()
    {
        if (action.skill == null) return;
        if (action.skill.cost <= controller.mana)
        {
            onOffimage.enabled = false;
        }
        else 
        {
            onOffimage.enabled = true;
        }

    }
}
