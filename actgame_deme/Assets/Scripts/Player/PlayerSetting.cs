using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSetting 
{
    [Header("�����ٶ�")]
    public  float lightSpeed ;
    public  float heavySpeed ;
    [Header("�����")]
    public  float shakeTime;
    public  int lightPause;
    public  float lightStrength;
    public  int heavyPause;
    public  float heavyStrength;
    [Header("�����˺�")]
    public float lightDamage;
    public float heavyDamage;

    [Space]
    public float interval = 2f;

    [Header("��ɫ����")]
    public float health;
    public float beHittedDistance;
    public float moveSpeed;
    public float jumpForce;
   
}
