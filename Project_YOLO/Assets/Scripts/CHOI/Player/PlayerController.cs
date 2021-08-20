using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Syadeu;
using Syadeu.Presentation;

public class PlayerController : MonoBehaviour, IActor
{
    [Header("설계도")]
    public int blueprintCount;
    
    [Space(5)] [Header("기본 정보")]
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Collider collider;
    [SerializeField] private bool initiaize;
    private float _maxVelocity = 10;
    
    [Space(5)] [Header("이동")]
    [SerializeField] private float moveSpeed;

    [Space(5)] [Header("점프")]
    [SerializeField] private float jumpPower;
    [SerializeField] private bool isJumping;

    [Space(5)] [Header("총")] 
    public GunController gun;
    
    [Space(5)] [Header("부스터")] 
    public bool haveBooster;
    [SerializeField] private float boosterPower;
    [SerializeField] private float boosterCooltime;
    [SerializeField] private bool boosterAvailable;
    [SerializeField] private GameObject boosterEffect;

    [Space(5)] [Header("애니메이션")]
    [SerializeField] private Animator animator;
    [SerializeField] bool animMove;
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

    public ActorProvider<PlayerController> ActorProvider => m_ActorProvider;

    #endregion

    private void Awake()
    {
        //중력 적용
        Physics.gravity = new Vector3(0, -35, 0);
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
        Dash();

        //최대 속도 체크
        VelocityCheck();
        
        //현재 포시션 체크
        PositionCheck();
    }

    private void VelocityCheck()
    {
        if (rigidbody.velocity.x > _maxVelocity)
        {
            rigidbody.velocity = new Vector3(_maxVelocity, rigidbody.velocity.y, 0);
        }
        else if (rigidbody.velocity.x < -_maxVelocity)
        {
            rigidbody.velocity = new Vector3(-_maxVelocity, rigidbody.velocity.y, 0);
        }
        
        if (rigidbody.velocity.y > _maxVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, _maxVelocity, 0);
        }
        else if (rigidbody.velocity.y < -_maxVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, -_maxVelocity, 0);
        }
    }
    
    private void PositionCheck()
    {
        Vector3 pos = CameraController.instance.camera.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = CameraController.instance.camera.ViewportToWorldPoint(pos);
    }
    
    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            //애니메이션
            AnimationSet(ref animMove, "Run", true);

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

        //밑 점프 체크
        if (!Input.GetKey(KeyCode.DownArrow))
        {
            isJumping = true;
        
            _maxVelocity = 15;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        else
        {
            _maxVelocity = 15;
            CollisionEnable(false);
        }
    }

    public void CollisionEnable(bool enabled)
    {
        if (collider.enabled == enabled) return;
        
        collider.enabled = enabled;

        if (!enabled)
        {
            //애니메이션
            animator.Play("JumpStart");
        }
    }

    private void Dash()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (!haveBooster) return;
        if (!boosterAvailable) return;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            boosterAvailable = false;
            
            rigidbody.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _maxVelocity = 10;
                rigidbody.AddForce(new Vector3(-1, 1, 0) * (boosterPower * 0.7f), ForceMode.Impulse);
            }
            else
            {
                _maxVelocity = 15;
                rigidbody.AddForce(Vector3.left * boosterPower, ForceMode.Impulse);
            }

            //부스터 이펙트
            BoosterEffect();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            boosterAvailable = false;
            
            rigidbody.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _maxVelocity = 10;
                rigidbody.AddForce(new Vector3(1, 1, 0) * (boosterPower * 0.7f), ForceMode.Impulse);
            }
            else
            {
                _maxVelocity = 15;
                rigidbody.AddForce(Vector3.right * boosterPower, ForceMode.Impulse);
            }
            
            //부스터 이펙트
            BoosterEffect();
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                boosterAvailable = false;
                
                _maxVelocity = 15;
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(new Vector3(0, 1, 0) * (boosterPower * 1.3f), ForceMode.Impulse);
                
                //부스터 이펙트
                BoosterEffect();
            }
        }

        if (!boosterAvailable)
        {
            //쿨타임 시작
            StartCoroutine(BoosterCooltime());
        }
    }

    private void BoosterEffect()
    {
        //이펙트
        boosterEffect.gameObject.SetActive(false);
        boosterEffect.gameObject.SetActive(true);
    }

    /// <summary>
    /// 부스터 획득 시 호출
    /// </summary>
    public void BoosterAcquisition()
    {
        haveBooster = true;
        BoosterAvailable();
    }

    /// <summary>
    /// 부스터를 사용 가능하도록 상태 변경합니다
    /// </summary>
    private void BoosterAvailable()
    {
        //부스터 게이지 UI 적용
        UIManager.Instance.SetBoostGage(1);
        
        boosterAvailable = true;
    }
    
    /// <summary>
    /// 애니메이션 설정
    /// </summary>
    private void AnimationSet(ref bool factor, string key, bool value)
    {
        if (factor == value) return;
        
        factor = value;
        animator.SetBool(key, value);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Floor")) return;
        
        //애니메이션
        animator.Play("JumpStart");
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Floor")) return;
        if (!initiaize) 
        {
            initiaize = true; 
            return;
            
        }

        //애니메이션
        animator.Play("JumpLanding");

        if (isJumping)
        {
            isJumping = false;
        }
    }

    //부스터 쿨타임
    IEnumerator BoosterCooltime()
    {
        var wait = new WaitForSeconds(0.01f);
        
        float time = 0;
        while (time < boosterCooltime)
        {
            //부스터 게이지 UI 적용
            UIManager.Instance.SetBoostGage(time / boosterCooltime);

            yield return wait;

            time += 0.01f;
        }
        
        BoosterAvailable();
    }
}