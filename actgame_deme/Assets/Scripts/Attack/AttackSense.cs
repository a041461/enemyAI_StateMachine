using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSense : MonoBehaviour
{
    private static AttackSense instance;
    private bool isShake = false;
    public static AttackSense Instance
    {
        get{
            if(instance == null)
            {
                instance = Transform.FindObjectOfType<AttackSense>();
            }
            return instance;
        }
    }

    public void AttackPause(int duartion)
    {
        StartCoroutine(IAttackPause(duartion));
    }
    IEnumerator IAttackPause(int duartion)
    {
        float pauseTime = duartion / 60f;
        Time.timeScale = .1f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
    public void AttackShake(float duartion, float strength)
    {
        StartCoroutine(IAttackShake(duartion,strength));
    }

    IEnumerator IAttackShake(float duartion,float strength)
    {
        isShake = true;
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.position;

        while (duartion>0)
        {
            camera.position = Random.insideUnitSphere * strength + startPosition;
            duartion -= Time.deltaTime;
        }
        yield return null;
        camera.position = startPosition;
        isShake = false;

    }
}
