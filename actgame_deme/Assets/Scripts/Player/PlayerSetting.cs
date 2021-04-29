using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSetting 
{
    [Header("补偿速度")]
    public  float lightSpeed ;
    public  float heavySpeed ;
    [Header("打击感")]
    public  float shakeTime;
    public  int lightPause;
    public  float lightStrength;
    public  int heavyPause;
    public  float heavyStrength;
    [Header("攻击伤害")]
    public float lightDamage;
    public float heavyDamage;

    [Space]
    public float interval = 2f;

    [Header("角色属性")]
    public float health;
    public float beHittedDistance;
    public float moveSpeed;
    public float jumpForce;
   
}
