using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerSetting playerSetting = new PlayerSetting();

    private float timer;
    public bool isAttack;
    public bool isHitted;
    private string attackType;
    private int comboStep;
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private float input;
    private bool isGround;
    private Vector3 attackMousePostion;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 check;
    private Transform hitTarget;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        isGround = Physics2D.OverlapCircle(transform.position + new Vector3(check.x, check.y, 0), check.z, layer);

        animator.SetFloat("Horizontal", rigidbody.velocity.x);
        animator.SetFloat("Vertical", rigidbody.velocity.y);
        animator.SetBool("isGround", isGround);


        Attack();
        Move();
    }

    void Move()
    {
        if (!isAttack && !isHitted)
            rigidbody.velocity = new Vector2(input * playerSetting.moveSpeed, rigidbody.velocity.y);
        else if(isAttack)
        {
            if (attackType == "Light")
                rigidbody.velocity = new Vector2(playerSetting.lightSpeed * (attackMousePostion.x >= 0 ? 1 : -1), rigidbody.velocity.y);
            else if (attackType == "Heavy")
                rigidbody.velocity = new Vector2(playerSetting.heavySpeed * (attackMousePostion.x >= 0 ? 1 : -1), rigidbody.velocity.y);
        }
        else if (isHitted)
        {
            if(hitTarget.position.x > transform.position.x)
            {
                rigidbody.velocity = new Vector2(-playerSetting.beHittedDistance, rigidbody.velocity.y);
            }
            else
            {
                rigidbody.velocity = new Vector2(playerSetting.beHittedDistance, rigidbody.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rigidbody.velocity = new Vector2(0, playerSetting.jumpForce);
            animator.SetTrigger("Jump");
        }

        if (rigidbody.velocity.x < 0 && !isHitted)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (rigidbody.velocity.x > 0 && !isHitted)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack && !isHitted)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = (Camera.main.ScreenToWorldPoint(mousePosition) - this.transform.position).normalized;
            UnityEngine.Debug.LogError(mousePosition);
            attackMousePostion = mousePosition;

            isAttack = true;
            attackType = "Light";
            comboStep++;
            if (comboStep > 3)
            {
                comboStep = 1;
            }
            animator.SetTrigger("LightAttack");
            animator.SetInteger("ComboStep", comboStep);
        }
        if (Input.GetMouseButtonDown(1) && !isAttack && !isHitted)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = (Camera.main.ScreenToWorldPoint(mousePosition) - this.transform.position).normalized;
            UnityEngine.Debug.LogError(mousePosition);
            attackMousePostion = mousePosition;

            isAttack = true;
            attackType = "Heavy";
            comboStep++;
            if (comboStep > 3)
            {
                comboStep = 1;
            }
            animator.SetTrigger("HeavyAttack");
            animator.SetInteger("ComboStep", comboStep);
        }
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                comboStep = 0;
            }
        }

    }

    public void AttackOver()
    {
        isAttack = false;
    }

    public void IsHittedOver(){
        isHitted = false;
     }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            FSMManager fSMManager = other.GetComponent<FSMManager>();
            if (fSMManager.isDead)
            {
                return;
            }
            switch (attackType)
            {
                case "Heavy":
                    fSMManager.paramator.health -= playerSetting.heavyDamage;
                    AttackSense.Instance.AttackPause(playerSetting.heavyPause);
                    AttackSense.Instance.AttackShake(1f, .5f);
                    break;
                case "Light":
                    fSMManager.paramator.health -= playerSetting.lightDamage;
                    AttackSense.Instance.AttackPause(playerSetting.lightPause);
                    AttackSense.Instance.AttackShake(1f, .5f);
                    break;

            }
            if (fSMManager.paramator.health <= 0 && !fSMManager.isDead)
            {
                fSMManager.TransStates(StateType.Dead);
            }
            else
            {
                fSMManager.TransStates(StateType.Hit);
            }

        }
        if (other.CompareTag("EnemyAttack"))
        {
            if (!isHitted)
            {
                isHitted = true;
                isAttack = false;
                animator.SetTrigger("isHitted");
                hitTarget = other.transform;
                AttackSense.Instance.AttackShake(1f,.5f);
                playerSetting.health--;
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(check.x, check.y, 0), check.z);
    }
}