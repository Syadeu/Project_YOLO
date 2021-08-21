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

    [Space(5)] [Header("점프")] 
    [SerializeField] private float jumpPower;
    private bool _downJumpAvailable;
    private bool _isJumping;

    //현재 밟고 있는 패스플로어
    [SerializeField] public List<Collider> ONPassFloors = new List<Collider>();
    
    //아이템
    [NonSerialized] public GunController Gun;
    [NonSerialized] public BoosterController Booster;
    [NonSerialized] public int BlueprintCount;

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
        Gun = gameObject.GetComponent<GunController>();
        Booster = gameObject.GetComponent<BoosterController>();
        
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

        if (_isJumping)
        {
            if (Booster != null)
            {
                if (!Booster.HaveBooster) return;
                if (!Booster.boosterAvailable) return;
                
                _rigidbody.velocity = Vector3.zero;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        Booster.Booster(BoosterType.Diagonal, new Vector3(-1, 1.5f, 0));
                    }
                    else
                    {
                        Booster.Booster(BoosterType.Straight, Vector3.left);
                    }
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        Booster.Booster(BoosterType.Diagonal, new Vector3(1, 1.5f, 0));
                    }
                    else
                    {
                        Booster.Booster(BoosterType.Straight, Vector3.right);
                    }
                }
                else
                {
                    Booster.Booster(BoosterType.Upward, new Vector3(0, 1, 0));
                }
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
                if (!_downJumpAvailable) return;
                
                //애니메이션
                _animator.Play("JumpStart");
            }
        }
    }

    public void IsJumping()
    {
        _isJumping = true;

        foreach (var floor in ONPassFloors)
        {
            floor.enabled = false;
        }

        ONPassFloors.Clear();
    }
    
    //점프 착지
    private void OnCollisionEnter(Collision other)
    {
        var tag = other.gameObject.tag;
        if (tag != "Floor" && tag != "PassFloor") return;
        if (other.transform.position.y >= transform.position.y + (transform.localScale.y * 0.5f)) return;;
        
        _downJumpAvailable = other.gameObject.tag switch
        {
            "Floor" => false,
            "PassFloor" => true,
            _ => _downJumpAvailable
        };
        if (!_isJumping) return;

        //미끄러지지 않도록 속도 초기화
        _rigidbody.velocity = Vector3.zero;
        
        //애니메이션
        _animator.Play("JumpLanding");

        _isJumping = false;
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
