using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IStates
{
    private FSM FSMManager;
    private Parameter parameter;
    private float timer;
    public IdleState(FSM FSMManager)
    {
        this.FSMManager = FSMManager;
        this.parameter = FSMManager.parameter;
    }

    public void onEnter()
    {
        this.parameter.animator.Play("Idle");
    }

    public void onExit()
    {
        timer = 0;
    }

    public void onUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= parameter.idleTime)
        {
            FSMManager.TransitionState(StateType.Patrol);
        }
    }



}
public class PatrolState : IStates
{
    private FSM FSMManager;
    private Parameter parameter;
    private int PatrolPosition;
    public PatrolState(FSM FSMManager)
    {
        this.FSMManager = FSMManager;
        this.parameter = FSMManager.parameter;
    }

    public void onEnter()
    {
        parameter.animator.Play("Walk");
    }

    public void onExit()
    {
        PatrolPosition++;
        if (PatrolPosition >= parameter.patrolPoints.Length)
        {
            PatrolPosition = 0;
        }
    }

    public void onUpdate()
    {
        FSMManager.Flipto(parameter.patrolPoints[PatrolPosition]);
        FSMManager.transform.position = Vector2.MoveTowards(FSMManager.transform.position,
            parameter.patrolPoints[PatrolPosition].position, parameter.moveSpeed * Time.deltaTime);
        if (Vector2.Distance(FSMManager.transform.position, parameter.patrolPoints[PatrolPosition].position) < 0.1f)
        {
            FSMManager.TransitionState(StateType.Idle);
        }
    }



}
public class ChaseState : IStates
{
    private FSM FSMManager;
    private Parameter parameter;
    public ChaseState(FSM FSMManager)
    {
        this.FSMManager = FSMManager;
        this.parameter = FSMManager.parameter;
    }

    public void onEnter()
    {
        parameter.animator.Play("Walk");
    }

    public void onExit()
    {
        //parameter.target = null;
    }

    public void onUpdate()
    {
        
        if (parameter.target != null)
        {
            FSMManager.Flipto(parameter.target);
            FSMManager.transform.position = Vector2.MoveTowards(FSMManager.transform.position,
                parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
        }
        if (parameter.target == null ||
            FSMManager.transform.position.x < parameter.chasePoints[0].position.x ||
            FSMManager.transform.position.x > parameter.chasePoints[1].position.x)
        {
            FSMManager.TransitionState(StateType.Idle);
        }
        if(Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            FSMManager.TransitionState(StateType.Attack);
        }
    }



}
public class ReactState : IStates
{
    private FSM FSMManager;
    private Parameter parameter;
    private AnimatorStateInfo info;
    public ReactState(FSM FSMManager)
    {
        this.FSMManager = FSMManager;
        this.parameter = FSMManager.parameter;
    }

    public void onEnter()
    {
        parameter.animator.Play("React");
    }

    public void onExit()
    {
        
    }

    public void onUpdate()
    {
        //FSMManager.Flipto(parameter.target);
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            FSMManager.TransitionState(StateType.Chase);
        }
    }



}
public class AttackState : IStates
{
    private FSM FSMManager;
    private Parameter parameter;
    private AnimatorStateInfo info;
    public AttackState(FSM FSMManager)
    {
        this.FSMManager = FSMManager;
        this.parameter = FSMManager.parameter;
    }

    public void onEnter()
    {
        parameter.animator.Play("Attack");
    }

    public void onExit()
    {
        
    }

    public void onUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            FSMManager.TransitionState(StateType.Chase);
        }
    }



}