using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private CharacterController m_cc;
    
    private Transform m_bottomPointTrans;




    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private float m_gravity;
    [SerializeField]
    private float m_upSpeed;

    private float m_currentUpSpeed;
    private float m_currentDropSpeed;

    [SerializeField]
    private LayerMask m_groundLayer;
    [SerializeField]
    private float m_groundDetectionRadius;




    private static Collider[] s_colliders = new Collider[10]; 

    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        m_bottomPointTrans = transform.Find("Body/BottomPoint");


    }

    private void Start()
    {
        m_isRun = true;
    }
    private void Update()
    {
        Drop();
        if (CheckJump())
            Jump();
        TurnTo();

    }
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (CheckMove(h, v))
            Move(h, v);
        
    }

    private bool CheckJump()
    {
        return Input.GetKeyDown(KeyCode.Space) && IsGround();
    }
    private bool CheckMove(float h,float v)
    {
        return Mathf.Abs(h) > 0.0005f || Mathf.Abs(v) > 0.0005f ;
    }

    private void Jump()
    {
        m_currentUpSpeed = m_upSpeed;
        StartCoroutine(Jumping());
    }
    private void Move(float h,float v)
    {
        Vector3 dir = new Vector3(h, 0,v).normalized;
        m_cc.Move(dir * m_moveSpeed * Time.deltaTime);
    }

    private void Drop()
    {
        if(m_currentUpSpeed<=0&&!IsGround())
        {
            m_currentDropSpeed += Mathf.Abs(m_gravity * Time.deltaTime);
            m_cc.Move(Vector3.down * m_currentDropSpeed * Time.deltaTime);
        }
        else
        {
            m_currentDropSpeed = 0f;
        }
    }
    private void TurnTo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,1000,m_groundLayer))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;
            transform.rotation= Quaternion.LookRotation(playerToMouse);

        }
    }

    private bool IsGround()
    {
        int num = Physics.OverlapSphereNonAlloc(m_bottomPointTrans.position, m_groundDetectionRadius, s_colliders, m_groundLayer);
        return num >0;
    }

    IEnumerator Jumping()
    {
        while (m_currentUpSpeed > 0)
        {
            m_cc.Move(m_currentUpSpeed * Vector3.up * Time.deltaTime);
            m_currentUpSpeed += m_gravity * Time.deltaTime;
            yield return null;
        }
        m_currentDropSpeed = 0f;
    }

    bool m_isRun;
    private void OnDrawGizmos()
    {
        if(m_isRun)
        {
            Gizmos.DrawWireSphere(m_bottomPointTrans.position, m_groundDetectionRadius);
            Gizmos.DrawCube(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
        }
        
    }

}
