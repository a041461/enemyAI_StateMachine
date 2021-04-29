using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{

    Idle,Patrol,Chase,React,Attack
    }

[Serializable]
public class Parameter
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Animator animator;
    public Transform target;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
}

public class FSM : MonoBehaviour
{
    public Parameter parameter;
    private IStates currentState;
    private Dictionary<StateType,IStates> states= new Dictionary<StateType,IStates>();
    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Attack,new AttackState(this));
        states.Add(StateType.Chase,new ChaseState(this));
        states.Add(StateType.Patrol,new PatrolState(this));
        states.Add(StateType.React,new ReactState(this));


        TransitionState(StateType.Idle);
        parameter.animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.onUpdate();
    }

    public void TransitionState(StateType type)
    {
        if(currentState != null)
        {
            currentState.onExit();
        }
        currentState = states[type];
        currentState.onEnter();
    }

    public void Flipto(Transform target)
    {
        if(target != null)
        {
            if(this.transform.position.x > target.transform.position.x)
            {
                this.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(this.transform.position.x < target.transform.position.x)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            parameter.target = collision.transform;
            TransitionState(StateType.React);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            parameter.target = null;
            TransitionState(StateType.Idle);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position,parameter.attackArea);
    }
}
