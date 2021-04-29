using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    void OnEnter();

    void OnUpdate();

    void OnExit();
}

public class IdleStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    public IdleStates( FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;
        

    }
    public void OnEnter()
    {
        paramator.animator.Play("Idle");
        timer = paramator.idleTime;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (Physics2D.OverlapCircle(paramator.AttackPoint.position, paramator.AttackArea, paramator.AttackLayer))
        {
            fsm.TransStates(StateType.React);
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            fsm.TransStates(StateType.Walk);
        }
    }
}
public class WalkStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private int PatrolPosition;
    public WalkStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;
        

    }
    public void OnEnter()
    {
        paramator.animator.Play("Walk");
    }

    public void OnExit()
    {
        PatrolPosition++;
        if (PatrolPosition>=paramator.walkPoint.Length)
        {
            PatrolPosition = 0;
        }
    }

    public void OnUpdate()
    {
        if (Physics2D.OverlapCircle(paramator.AttackPoint.position, paramator.AttackArea, paramator.AttackLayer))
        {
            fsm.TransStates(StateType.React);
        }
        fsm.FlipTo(paramator.walkPoint[PatrolPosition]);
        fsm.transform.position = Vector2.MoveTowards(fsm.transform.position,
            paramator.walkPoint[PatrolPosition],
            paramator.walkSpeed*Time.deltaTime);
        if (Vector2.Distance(fsm.transform.position, paramator.walkPoint[PatrolPosition]) <0.1f)
        {
            fsm.TransStates(StateType.Idle);
        }
        
    }
}
public class ChaseStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    
    public ChaseStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("Chase");
    }

    public void OnExit()
    {
       
        
    }

    public void OnUpdate()
    {
        fsm.FlipTo(paramator.target.position);
        fsm.transform.position = Vector2.MoveTowards(fsm.transform.position,
            paramator.target.position,
            paramator.chaseSpeed * Time.deltaTime);
        if (Physics2D.OverlapCircle(paramator.AttackPoint.position,paramator.AttackArea,paramator.AttackLayer))
        {
            fsm.TransStates(StateType.Attack);
        }
    }
}
public class DeadStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;

    public DeadStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;


    }
    public void OnEnter()
    {
        fsm.isDead = true;
        paramator.animator.Play("Dead");
        fsm.transform.GetChild(0).GetComponent<Animator>().Play("Dead");
        AttackSense.Instance.AttackPause(100);
       
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 1f)
        {
            
            fsm.KillEnemy();
        }


    }
}
public class ReactStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;

    public ReactStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("React");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        //fsm.FlipTo(paramator.target.position);
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);
        if (paramator.target != null)
        {
            if (info.normalizedTime >= 0.95f)
            {
                fsm.TransStates(StateType.Chase);
            }
        }
    }
}
public class AttackStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;
    public AttackStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("Attack");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);
       
            if (info.normalizedTime >= 0.95f)
            {
                if (Physics2D.OverlapCircle(paramator.AttackPoint.position, paramator.AttackArea, paramator.AttackLayer))
                {
                    fsm.TransStates(StateType.Attack);
                }
            fsm.TransStates(StateType.Chase);
            }
        
    }
}
public class HitStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;
    public HitStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("Hit");
        fsm.transform.GetChild(0).GetComponent<Animator>().Play("hit");       
        paramator.target = Transform.FindObjectOfType<PlayerController>().transform;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.6f)
        {
            fsm.TransStates(StateType.Chase);
        }

    }
}