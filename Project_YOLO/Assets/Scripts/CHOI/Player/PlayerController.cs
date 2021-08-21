using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Syadeu;
using Syadeu.Presentation;

public class PlayerController : MonoBehaviour, IActor
{
    [Space(5)] [Header("모든 동작 정지")] 
    public bool inputPause;

    [Space(5)] [Header("이동")] 
    [SerializeField] private float moveSpeed;
    public List<Collider> onPassFloors;

    [Space(5)] [Header("점프")] 
    [SerializeField] private float jumpPower;
    [SerializeField] private bool downJumpAvailable;
    public bool isJumping;

    //아이템
    [NonSerialized] public GunController gun;
    [NonSerialized] public BoosterController booster;
    [NonSerialized] public int blueprintCount;

    //리지드바디
    private Rigidbody _rigidbody;

    //애니메이터
    private Animator _animator;
    private bool _animMove;
    
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

    [Space(5)] [Header("IActor")] [SerializeField]
    private ActorID m_ActorID;

    [SerializeField] private SkillDescription[] m_Skills = Array.Empty<SkillDescription>();

    private ActorProvider<PlayerController> m_ActorProvider;

    public Animator Animator => _animator;
    public ActorID ActorID => m_ActorID;
    public SkillDescription[] Skills => m_Skills;

    public ActorProvider<PlayerController> ActorProvider => m_ActorProvider;

    #endregion

    private void Awake()
    {
        //중력 적용
        Physics.gravity = new Vector3(0, -50, 0);
        
        //초기화
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        gun = gameObject.GetComponent<GunController>();
        booster = gameObject.GetComponent<BoosterController>();
        
        PresentationSystemGroup<YOLO_SystemGroup>.Start();
        CoreSystem.WaitInvoke(() => YOLOPresentationProvider.Instance.ActorSystem != null, RegisterActor);
    }

    private void RegisterActor()
    {
        m_ActorProvider = YOLOPresentationProvider.Instance.ActorSystem.RegisterActor(this);
    }

    private void Update()
    {
        if (inputPause) return;

        Move();
        Jump();
        
        //최대 속도 체크
        VelocityCheck();
    }
    
    private void VelocityCheck()
    {
        if (booster == null) return;
        
        if (_rigidbody.velocity.x > booster.maxVelocity)
        {
            _rigidbody.velocity = new Vector3(booster.maxVelocity, _rigidbody.velocity.y, 0);
        }
        else if (_rigidbody.velocity.x < -booster.maxVelocity)
        {
            _rigidbody.velocity = new Vector3(-booster.maxVelocity, _rigidbody.velocity.y, 0);
        }

        if (_rigidbody.velocity.y > booster.maxVelocity)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, booster.maxVelocity, 0);
        }
        else if (_rigidbody.velocity.y < -booster.maxVelocity)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -booster.maxVelocity, 0);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            //애니메이션
            AnimationSet(ref _animMove, "Run", true);

            var h = Input.GetAxisRaw("Horizontal");

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
            _rigidbody.MovePosition(transform.position + (new Vector3(h, 0, 0) * moveSpeed));
        }
        else
        {
            //애니메이션
            AnimationSet(ref _animMove, "Run", false);
        }
    }

    private void Jump()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (isJumping)
        {
            if (booster != null)
            {
                booster.Booster();
            }
        }
        else
        {
            //밑 점프 체크
            if (!Input.GetKey(KeyCode.DownArrow))
            {
                //애니메이션
                _animator.Play("JumpStart");
                
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            else
            {
                if (!downJumpAvailable) return;
                
                //애니메이션
                _animator.Play("JumpStart");
            }
        }
    }

    public void IsJumping()
    {
        isJumping = true;

        foreach (var floor in onPassFloors)
        {
            floor.enabled = false;
        }

        onPassFloors.Clear();
    }
    
    //점프 착지
    private void OnCollisionEnter(Collision other)
    {
        var tag = other.gameObject.tag;
        if (tag != "Floor" && tag != "PassFloor") return;
        if (other.transform.position.y >= transform.position.y + (transform.localScale.y * 0.5f)) return;;
        
        downJumpAvailable = other.gameObject.tag switch
        {
            "Floor" => false,
            "PassFloor" => true,
            _ => downJumpAvailable
        };
        if (!isJumping) return;

        //애니메이션
        _animator.Play("JumpLanding");

        isJumping = false;
    }
    
    /// <summary>
    /// 애니메이션 설정
    /// </summary>
    private void AnimationSet(ref bool factor, string key, bool value)
    {
        if (factor == value) return;

        factor = value;
        _animator.SetBool(key, value);
    }
}
