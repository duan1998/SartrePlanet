using Battle;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class BulletInit : MonoBehaviour
{
    public GameObject[] BulletPrefebs=new GameObject[4];
    public float delayTime;
    public Vector3 SectorBegin;
    public int SectorDensity,SectorAngle,order=0;
    public float InitRadius, Distance;
    private List<int[]> ExcuteOrder=new List<int[]>();
    public int[] a = new int[4]; 
    public int[] b = new int[4]; 
    public int[] c = new int[4]; 
    public int[] d = new int[4];
    private float WaitTime;
    void Start()
    {
        ExcuteOrder.Add(a);
        ExcuteOrder.Add(b);
        ExcuteOrder.Add(c);
        ExcuteOrder.Add(d);
        WaitTime = 2f;
    }
    private void Update()
    {
        if(WaitTime>0)
        {
            WaitTime -= Time.deltaTime;
        }
        else
        {
            if (order > 3)
            {
                PlayerMgr.playerMgr.EnterQTE();
                return;
            }
            int[] temp = ExcuteOrder[order]; order++;
            switch (temp[0])
            {
                case 1:
                    StartCoroutine(CircleBullet(temp[1], temp[2], transform.position));
                    Debug.Log("Circle");
                    break;
                case 2:
                    StartCoroutine(SectorBullet(temp[1], temp[2]));
                    Debug.Log("Sector");
                    break;
                case 3: StartCoroutine(InvokeCircle(temp[1], transform.position));
                    Debug.Log("InvokeCircle");
                    break;
                case 4: StartCoroutine(TwirlBullet(temp[1], temp[2], transform.position));
                    Debug.Log("TwirlCircle");
                    break;
            }
            WaitTime = temp[3];
        }
    }
    public IEnumerator CircleBullet(int colorIndex,int times,Vector3 InitPosition)
    {
        Quaternion rotateQuaternion = Quaternion.AngleAxis(20, Vector3.forward);
        for(int i=0;i< times;i++)
        {
            Quaternion fireDirection = Quaternion.Euler(Vector3.zero);
            for (int k=0;k<18;k++)
            {
                Instantiate(BulletPrefebs[colorIndex], InitPosition, fireDirection);
                fireDirection = rotateQuaternion * fireDirection;
            }
            yield return new WaitForSeconds(delayTime);
        }
        yield return null;
    }
    public IEnumerator SectorBullet(int colorIndex,int times)
    {
        Quaternion roateQuaternion = Quaternion.AngleAxis(SectorAngle, Vector3.forward);
        for (int i = 0; i < times; i++)
        {
            Quaternion fireDirection = Quaternion.Euler(SectorBegin);
            for (int k = 0; k < SectorDensity; k++)
            {
                Instantiate(BulletPrefebs[colorIndex], transform.position, fireDirection);
                fireDirection = roateQuaternion * fireDirection;
            }
            yield return new WaitForSeconds(delayTime);
        }
        yield return null;
    }
    public IEnumerator InvokeCircle(int colorIndex, Vector3 InitPosition)
    {
        Quaternion rotateQuaternion = Quaternion.AngleAxis(45, Vector3.forward);
        List<GameObject> Bullets=new List<GameObject>();
        Quaternion fireDirection = Quaternion.Euler(Vector3.zero);
        for (int i=0;i<9;i++)
        {
            GameObject temp = Instantiate(BulletPrefebs[colorIndex], InitPosition, fireDirection);
            fireDirection = rotateQuaternion * fireDirection;
            Bullets.Add(temp);
        }
        yield return new WaitForSeconds(2f);
        for(int i=0;i<Bullets.Count;i++)
        {
            if(Bullets[i]==null)
                  break;
            StartCoroutine(CircleBullet(colorIndex,1,Bullets[i].transform.position));
            Destroy(Bullets[i]);
        }
    }
    public IEnumerator TwirlBullet(int colorIndex,int times,Vector3 InitPosition)
    {
        Vector3 fireDirection = transform.up, firePos;
        Quaternion TwirlRotate = Quaternion.AngleAxis(20, Vector3.forward);
        float radius = InitRadius;
        for(int i=0;i<times;i++)
        {
            firePos = InitPosition + fireDirection*radius;
            StartCoroutine(CircleBullet(colorIndex, 1, firePos));
            yield return new WaitForSeconds(0.1f);
            fireDirection = TwirlRotate*fireDirection;
            radius += Distance;
        }
        yield return null;
    }
}