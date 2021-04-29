using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateType
{
    Attack,
    Chase,
    Dead,
    Hit,
    Idle,
    React,
    Walk,


}

[Serializable]
public class Paramator{
    public float health;
    public Animator animator;
    public float idleTime;
    public Vector3[] walkPoint;
    public float walkDistance;
    public float walkSpeed;
    public Transform target;
    public float chaseSpeed;
    public Transform AttackPoint;
    public float AttackArea;
    public LayerMask AttackLayer;
   
    

}
public class FSMManager : MonoBehaviour
{
    public Paramator paramator;
    public bool isDead = false;
    

    private Dictionary<StateType,IState> states = new Dictionary<StateType, IState>();
    private IState currentState;
    // Start is called before the first frame update
    void Start()
    {
        
        paramator.animator = this.GetComponent<Animator>();
        states.Add(StateType.Idle, new IdleStates(this));
        states.Add(StateType.Walk, new WalkStates(this));
        states.Add(StateType.React, new ReactStates(this));
        states.Add(StateType.Chase, new ChaseStates(this));
        states.Add(StateType.Attack, new AttackStates(this));
        states.Add(StateType.Hit, new HitStates(this));
        states.Add(StateType.Dead, new DeadStates(this));

        paramator.walkPoint = new Vector3[2];
        paramator.walkPoint[0] = this.transform.position+new Vector3(paramator.walkDistance,0,0);
        paramator.walkPoint[1] = this.transform.position-new Vector3(paramator.walkDistance, 0, 0);
        

        TransStates(StateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();        
        RefreshEnemy();
    }

    public void TransStates(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    public void FlipTo(Vector3 target)
    {
        if (target != null)
        {
            if (this.transform.position.x < target.x)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            }
            if (this.transform.position.x > target.x)
            {
                this.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
           if (collision.gameObject.CompareTag("Player") && currentState != states[StateType.Attack])
            {
                paramator.target = collision.transform;
                TransStates(StateType.React);
            }
        }
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Player")&& currentState!=states[StateType.Attack])
            {
                UnityEngine.Debug.LogError(currentState);
                paramator.target = null;
                TransStates(StateType.Idle);
            }
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(paramator.AttackPoint.position,paramator.AttackArea);
    }

    private void RefreshEnemy()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            isDead = false;
            paramator.health = 10;
            TransStates(StateType.Idle);
            for (int i = 0; i <= transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    public void KillEnemy()
    {
        for(int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
