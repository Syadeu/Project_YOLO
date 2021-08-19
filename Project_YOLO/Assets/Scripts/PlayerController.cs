using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Syadeu;
using Syadeu.Presentation;

public class PlayerController : MonoBehaviour, IActor
{
    [Header("기본 정보")]
    [SerializeField] private ProjectileSeed seedProjectile;
    [SerializeField] private new Rigidbody rigidbody;

    [Space(5)] [Header("이동")]
    [SerializeField] private float moveSpeed;
    
    [Space(5)] [Header("점프")]
    [SerializeField] private float jumpPower;
    [SerializeField] private bool isJumping;
    
    [Space(5)] [Header("대쉬")]
    [SerializeField] public bool haveBooster;
    [SerializeField] private float dashPower;
    [SerializeField] private bool isDashing;

    [Space(5)] [Header("공격")]
    [Obsolete][SerializeField] public bool haveSeed;
    
    [Space(5)] [Header("애니메이션")]
    [SerializeField] private Animator animator;
    [SerializeField] bool animMove;
    [SerializeField] bool animAtack;
    [SerializeField] bool animJump;

    static PlayerController _instance;

    public static PlayerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerController>();
            }

            return _instance;
        }
    }

    #region IActor

    [Space(5)] [Header("IActor")]
    [SerializeField] private ActorID m_ActorID;
    [SerializeField] private SkillDescription[] m_Skills = Array.Empty<SkillDescription>();

    private ActorProvider<PlayerController> m_ActorProvider;

    public Animator Animator => animator;
    public ActorID ActorID => m_ActorID;
    public SkillDescription[] Skills => m_Skills;

    #endregion

    private void Awake()
    {
        //중력 적용
        Physics.gravity = new Vector3(0, -50, 0);
        CoreSystem.WaitInvoke(PresentationSystem<YOLO_ActorSystem>.IsValid, RegisterActor);
    }
    private void RegisterActor()
    {
        m_ActorProvider = PresentationSystem<YOLO_ActorSystem>.System.RegisterActor(this);
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            //대쉬 체크
            Dash();
            
            //애니메이션
            AnimationSet(ref animMove, "Run", true);

            //키 입력에 따라 캐릭터 방향
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            
            //이동
            transform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));
        }
        else
        {
            //애니메이션
            AnimationSet(ref animMove, "Run", false);
        }
    }

    private void Jump()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (isJumping) return;

        animator.Play("Jump");
        
        isJumping = true;
        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    
    private void Dash()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (!haveBooster) return;
        if (isDashing) return;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isDashing = true;
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rigidbody.AddForce(new Vector3(-1, 1, 0) * dashPower, ForceMode.Impulse);
            }
            else
            {
                rigidbody.AddForce(Vector3.left * dashPower, ForceMode.Impulse);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            isDashing = true;
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rigidbody.AddForce(new Vector3(1, 1, 0) * dashPower, ForceMode.Impulse);
            }
            else
            {
                rigidbody.AddForce(Vector3.right * dashPower, ForceMode.Impulse);
            }
        }

        if (isDashing) { Invoke(nameof(DashAvailable), 0.8f); }
    }

    private void DashAvailable()
    {
        isDashing = false;
    }
    
    private void Attack()
    {
        if (!Input.GetKeyDown(KeyCode.V)) return;
        if (!haveSeed) return;

        haveSeed = false;
        Instantiate(seedProjectile, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.rotation);
    }

    [Obsolete]
    public void SeedAcquisition()
    {
        haveSeed = true;
        
    }
    public void InsertItem(IItem item)
    {
        // 이제 이 함수로 들어와서 Actor에 들어갑니다.
        m_ActorProvider.InventoryProvider.InsertItem(item);
    }

    public void BoosterAcquisition()
    {
        haveBooster = true;
    }

    public void AnimationSet(ref bool factor, string key, bool value)
    {
        if (factor != value)
        {
            factor = value;
            animator.SetBool(key, value);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }
}
