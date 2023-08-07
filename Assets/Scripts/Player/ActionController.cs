using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 2;
    bool autoMove = false;
    [SerializeField]
    float attackRange = 3;
    [SerializeField]
    float automoveSpeed = 2;
    Enemy enemyTarget;
    Timer attackTimer = new Timer();
    [SerializeField]
    float attackDelay = 2;
    public List<ActionClass> actions = new List<ActionClass>()
    {
        new ActionClass (){key = KeyCode.Z},new ActionClass (){key = KeyCode.X},new ActionClass (){key = KeyCode.C},
        new ActionClass (){key = KeyCode.V},new ActionClass (){key = KeyCode.B},new ActionClass (){key = KeyCode.N},
        new ActionClass (){key = KeyCode.Alpha1},new ActionClass (){key = KeyCode.Alpha2},new ActionClass (){key = KeyCode.Alpha3},
        new ActionClass (){key = KeyCode.Alpha4}, new ActionClass (){key = KeyCode.Alpha5},new ActionClass (){key = KeyCode.Alpha6},
    };
    public AnimatorSync sync {get; set;}
    bool inAction = false;
    Player player;
    public int mana = 0;
    public Inventory inventory = new Inventory();
    [SerializeField]
    float interactDistance = 5;
    [SerializeField]
    Texture2D[] cursorList = null;
    enum Cursors
    {
        normal, atk, pick, bag, teleport
    }

    public void Init(Player player)
    {
        this.player = player;
        mana = player.stats.Mana;
        BuildInventory();
        UIManager.instance.SetActions(this);
        UIManager.instance.chat.SetUP(player.data.characterName);
    }

    public void Tick(Transform follow, float x, float y)
    {
        float delta = Time.deltaTime;
        if (autoMove == false)
            transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rotSpeed * delta);
        
        RightClick();
        if (x != 0 || y != 0)
        {
            autoMove = false;
        }
        if (autoMove)
        {
            if (enemyTarget == null)
            {
                autoMove = false;
                return;
            }
            Vector3 enemyPos = enemyTarget.transform.position;
            float dist = Vector3.Distance(transform.position, enemyPos);
            float velocity = 0;
            if (dist > attackRange)
            {
                velocity = 1;
                transform.position = Vector3.MoveTowards(transform.position, enemyPos,
                automoveSpeed * delta);
            }
            else 
            {
                AutoAttack(delta);
            }
            sync.Move(0, velocity);
            Vector3 look = enemyPos - transform.position;
            look.y = 0;
            Quaternion rot = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, automoveSpeed * delta);
        }
        foreach (var item in actions)
        {
            if (Input.GetKeyDown(item.key))
            {
                PressButton(item);
                break;
            }
        }
    }

    void BuildInventory()
    {
        inventory.Init(player);
        foreach (var item in inventory.equippedSKills)
        {
            actions[item.value].skill = item.Key;
        }
    }

    void AutoAttack(float delta)
    {
        if (enemyTarget == null && enemyTarget.isDeath)
        {
            autoMove = false;
            enemyTarget = null;
            return;    
        }

        if (!attackTimer.timerActive(delta))
        {
            attackTimer.StartTimer(attackDelay);
            sync.PlayAnimation("Atk");
            enemyTarget.TakeDamage(GetDamage());
        }
    }

    int GetDamage()
    {
        int val = 1;
        return val;
    }

    void RightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == StaticStrings.enemy)
                {
                    autoMove = true;
                    enemyTarget = hit.transform.GetComponent<Enemy>();
                    return;
                }
                Interactable inter = hit.transform.GetComponent<Interactable>();
                if (inter != null)
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < interactDistance)
                    {
                        inter.Interact(player);
                    }
                }
            }
        }
    }

    public void PressButton(ActionClass action = null)
    {
        Skill skill = action.skill;
        if (skill == null) return;

        if (action.button.Charging()) return;

        if (skill.cost <= mana)
        {
            mana -= skill.cost;
            inAction = true;
            sync.PlayAnimation(skill.animName.ToString());
            action.button.SetCountDown();
        }
    }

    public void MouseLeft()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.transform.tag)
            {
                case StaticStrings.enemy:
                    Cursor.SetCursor(cursorList[(int)Cursors.atk], Vector2.zero, CursorMode.Auto);
                    break;
                case StaticStrings.teleport:
                    Cursor.SetCursor(cursorList[(int)Cursors.teleport], Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(cursorList[(int)Cursors.normal], Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }
}

[System.Serializable]
public class ActionClass
{
    public KeyCode key;
    public Skill skill;
    public ActionButton button;
}