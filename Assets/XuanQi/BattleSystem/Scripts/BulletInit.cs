using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletInit : MonoBehaviour
{
    public GameObject[] BulletPrefebs = new GameObject[4];
    public Vector3 SectorBegin;
    public int SectorDensity, SectorAngle;
    public float InitRadius, Distance;
    private int TwirlTimes = 5;
    private void Start()
    {
        
    }
    public IEnumerator Colorful(Func<int, Vector3, IEnumerator> CreateFun, int initColor, Vector3 Pos, int times)
    {
        int ColorRandom = initColor;
        for (int i = 0; i < times; i++)
        {
            yield return CreateFun(ColorRandom, Pos);
            ColorRandom = UnityEngine.Random.Range(0, 4);
        }
        yield return null;
    }
    public IEnumerator CircleBullet(int colorIndex, Vector3 InitPosition)
    {
        Debug.Log("成功");
        Quaternion rotateQuaternion = Quaternion.AngleAxis(20, Vector3.forward);
        Quaternion fireDirection = Quaternion.Euler(Vector3.zero);
        for (int k = 0; k < 18; k++)
        {
            Instantiate(BulletPrefebs[colorIndex], InitPosition, fireDirection);
            fireDirection = rotateQuaternion * fireDirection;
        }
        yield return null;
    }
    public IEnumerator SectorBullet(int colorIndex, Vector3 InitPosition)
    {
        Quaternion roateQuaternion = Quaternion.AngleAxis(SectorAngle, Vector3.forward);
        Quaternion fireDirection = Quaternion.Euler(SectorBegin);
        for (int k = 0; k < SectorDensity; k++)
        {
            Instantiate(BulletPrefebs[colorIndex], transform.position, fireDirection);
            fireDirection = roateQuaternion * fireDirection;
        }
        yield return null;
    }
    public IEnumerator InvokeCircle(int colorIndex, Vector3 InitPosition)
    {
        Quaternion rotateQuaternion = Quaternion.AngleAxis(45, Vector3.forward);
        List<GameObject> Bullets = new List<GameObject>();
        Quaternion fireDirection = Quaternion.Euler(Vector3.zero);
        for (int i = 0; i < 9; i++)
        {
            GameObject temp = Instantiate(BulletPrefebs[colorIndex], InitPosition, fireDirection);
            fireDirection = rotateQuaternion * fireDirection;
            Bullets.Add(temp);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < Bullets.Count; i++)
        {
            if (Bullets[i] == null)
                continue;
            StartCoroutine(CircleBullet(colorIndex, Bullets[i].transform.position));
            Destroy(Bullets[i]);
        }
        yield return null;
    }
    public IEnumerator TwirlBullet(int colorIndex, Vector3 InitPosition)
    {
        Vector3 fireDirection = transform.up, firePos;
        Quaternion TwirlRotate = Quaternion.AngleAxis(20, Vector3.forward);
        float radius = InitRadius;
        for (int i = 0; i < TwirlTimes; i++)
        {
            firePos = InitPosition + fireDirection * radius;
            StartCoroutine(CircleBullet(colorIndex, firePos));
            yield return new WaitForSeconds(0.1f);
            fireDirection = TwirlRotate * fireDirection;
            radius += Distance;
        }
        yield return null;
    }
}