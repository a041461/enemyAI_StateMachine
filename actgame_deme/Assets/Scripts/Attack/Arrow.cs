using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rigidbody2;
    private bool isGround;
    private void Awake()
    {
        rigidbody2 = this.transform.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGround)
        rigidbody2.velocity = new Vector2(2-Time.deltaTime*50, rigidbody2.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = true;
    }
}
