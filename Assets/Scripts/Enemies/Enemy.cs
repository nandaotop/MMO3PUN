using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    EnemyState estate = EnemyState.idle;
    Timer timer = new Timer();
    float delta;
    [SerializeField]
    Vector3 direction = new Vector3(0, 0, 5);
    Vector3 endPos;
    Vector3 startPosition;
    Vector3 destination;
    [SerializeField]
    float patrolDist = 1;
    float wait = 2;
    [SerializeField]
    bool staticEnemy = false;

    public override void Init()
    {
        base.Init();
        timer.StartTimer(wait);
        startPosition = transform.position;
        endPos = transform.position + direction;
        destination = endPos;
    }

    public override void Tick()
    {
        delta = Time.deltaTime;
        if (staticEnemy) return;

        switch (estate)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.patrol:
                Patrol();
                break;
            case EnemyState.Combat:
                Combat();
                break;
        }

    }

    void Combat()
    {

    }

    void Patrol()
    {
        float dist = Vector3.Distance(transform.position, destination);
        if (dist <= patrolDist)
        {
            if (destination == endPos)
            {
                destination = startPosition;
            }
            else
            {
                destination = endPos;
            }
            timer.StartTimer(wait);
            estate = EnemyState.idle;
            sync.SetMove(false);
        }
        else
        {
            MoveAt(destination);
        }
    }

    void MoveAt(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        Vector3 look = destination - transform.position;
        look.y = 0;
        Quaternion rot = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, wait * delta);
    }

    void Idle()
    {
        if (!timer.timerActive(delta))
        {
            estate = EnemyState.patrol;
            sync.SetMove(true);
        }
    }

    enum EnemyState
    {
        idle,
        patrol,
        Combat
    }
}
