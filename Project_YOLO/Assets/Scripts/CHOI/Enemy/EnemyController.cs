using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    [Space(5)] [Header("랜덤 이동(정찰)")]
    [SerializeField] private bool randomMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int repeatRate;
    private int randomPosition;

    [Space(5)] [Header("돌진 설정")]
    [SerializeField] private bool rushAvailable;
    [SerializeField] private float rushCastingRate;
    [SerializeField] private float rushPower;
    private bool _passPlayer;
    private bool _isRushing;
    
    [Space(5)] [Header("점프 공격 설정")]
    [SerializeField] private bool jumpAttackAvailable;
    [SerializeField] private float jumpAttackCastingRate;
    private bool _isJumpAtacking;
    
    [Space(5)] [Header("레이캐스트 설정")]
    [SerializeField] private float _downRayMaxDistance = 1.5f;
    [SerializeField] private float _rayMaxDistance = 1.5f;
    [SerializeField] private float _detectionMaxDistance = 10;
    private RaycastHit _hit;
    
    //리지드바디
    private Rigidbody _rigidbody;

    //애니메이터
    private Animator _animator;
    private bool _animMove;
    private bool _animReady;
    private bool _animRush;
    private bool _animJumpAttack;
    
    private void Awake()
    {
        //초기화
        currentHealth = maxHealth;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();

        //랜덤 포지션 시작
        if (randomMove)
        {
            InvokeRepeating(nameof(SetRandomPosition), 0, repeatRate);
        }
    }

    private void FixedUpdate()
    {
        if (randomMove)
        {
            _rigidbody.MovePosition(transform.position + (new Vector3(randomPosition, 0, 0) * moveSpeed));

            RayCast();
        }
        else
        {
            if (rushAvailable)
            {
                RushAttack();
            }
            //점프 공격
            else if (jumpAttackAvailable)
            {
                JumpAttack();
            }
        }
    }

    public void RayCast()
    {
        //앞에 벽이 있는지 체크
        var frontPosition = transform.position + new Vector3(0, 0.5f, 0);
        Debug.DrawRay(frontPosition, transform.forward * _rayMaxDistance, Color.green);
        if (Physics.Raycast(frontPosition, transform.forward, out _hit, _rayMaxDistance, LayerMask.GetMask("Floor")))
        {
            randomPosition *= -1;
            RotationSet();
                
            Debug.Log("벽!");
        }
            
        //앞이 낭떨어지인지 체크
        var downPosition = transform.position + (transform.forward * 1.5f) + (transform.up * 0.5f);
        Debug.DrawRay(downPosition, -transform.up * (_downRayMaxDistance * 0.3f), Color.green);
        if (!Physics.Raycast(downPosition, -transform.up, out _hit, _downRayMaxDistance * 0.5f, LayerMask.GetMask("Floor")))
        {
            randomPosition *= -1;
            RotationSet();
                
            Debug.Log("낭떨어지!");
        }
            
        //플레이어 포착
        var frontDetection = transform.position + new Vector3(0, 2.5f, 0);
        Debug.DrawRay(frontDetection, transform.forward * _detectionMaxDistance, Color.red);
        if (Physics.Raycast(frontDetection, transform.forward, out _hit, _detectionMaxDistance, LayerMask.GetMask("Player")))
        {
            CancelInvoke();
            randomMove = false;
            Debug.Log("플레이어!");
        }
            
        //플레이어 포착
        var backDetection = transform.position + new Vector3(0, 2.5f, 0);
        Debug.DrawRay(backDetection, -transform.forward * _detectionMaxDistance, Color.red);
        if (Physics.Raycast(backDetection, -transform.forward, out _hit, _detectionMaxDistance, LayerMask.GetMask("Player")))
        {
            CancelInvoke();
            randomMove = false;
            Debug.Log("플레이어!");
        }
    }

    public void LookPlayer()
    {
        //플레이어 바라보기
        if (PlayerController.instance.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }
    }

    public void SetRandomPosition()
    {
        randomPosition = Random.Range(-1, 2);

        RotationSet();
    }

    public void RotationSet()
    {
        switch (randomPosition)
        {
            case 1:
                AnimationSet(ref _animMove, "Run", true);
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
            
            case 0:
                AnimationSet(ref _animMove, "Run", false);
                break;
                
            case -1:
                AnimationSet(ref _animMove, "Run", true);
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                break;
        }
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

    public void TakeDamage()
    {
        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RushAttack()
    {
        if (!_isRushing)
        {
            LookPlayer();

            AnimationSet(ref _animMove, "Run", false);
            
            StartCoroutine(_RushAttack());
        }
        else
        {
            if (_animRush)
            {
                if (!_passPlayer)
                {
                    if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) <= 0.5f)
                    {
                        _passPlayer = true;
                    }
                }
                else
                {
                    if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) > 10)
                    {
                        _rigidbody.Sleep();
                            
                        AnimationSet(ref _animRush, "Rush", false);
                            
                        _isRushing = false;
                        _passPlayer = false;
                    }
                }
            }
            else
            {
                LookPlayer();
            }
        }
    }

    public void JumpAttack()
    {
        if (!_isJumpAtacking)
        {
            LookPlayer();

            AnimationSet(ref _animMove, "Run", false);
            
            StartCoroutine(_JumpAttack());
        }
        else
        {
            if (!_animJumpAttack)
            {
                LookPlayer();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var tag = other.gameObject.tag;
        if (tag != "Floor") return;
        if (!_isJumpAtacking) return;
        
        //미끄러지지 않도록 속도 초기화
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.Sleep();
        
        //애니메이션
        AnimationSet(ref _animJumpAttack, "JumpAttack", false);
        _animator.Play("JumpLanding");

        _isJumpAtacking = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("InvisibleWall")) return;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.Sleep();

        AnimationSet(ref _animRush, "Rush", false);
        
        _isRushing = false;
        _passPlayer = false;
    }

    IEnumerator _RushAttack()
    {
        _isRushing = true;
        
        AnimationSet(ref _animReady, "Ready", true);

        yield return new WaitForSeconds(rushCastingRate);

        if (_rigidbody.IsSleeping())
        {
            _rigidbody.WakeUp();
        }
        
        _rigidbody.velocity = Vector3.zero;
        var direction = transform.rotation.y;
        if (direction > 0)
        {
            _rigidbody.AddForce(Vector3.right * rushPower, ForceMode.VelocityChange);
        }
        else
        {
            _rigidbody.AddForce(Vector3.left * rushPower, ForceMode.VelocityChange);
        }
        
        AnimationSet(ref _animReady, "Ready", false);
        AnimationSet(ref _animRush, "Rush", true);
    }
    
    IEnumerator _JumpAttack()
    {
        _isJumpAtacking = true;
        
        AnimationSet(ref _animReady, "Ready", true);

        yield return new WaitForSeconds(jumpAttackCastingRate);

        if (_rigidbody.IsSleeping())
        {
            _rigidbody.WakeUp();
        }

        _rigidbody.velocity = Vector3.zero;
        var direction = transform.rotation.y;
        if (direction > 0)
        {
            _rigidbody.AddForce(new Vector3(Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) * 2.1f, 50, 0), ForceMode.Impulse);
        }
        else
        {
            _rigidbody.AddForce(new Vector3(-Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) * 2.1f, 50, 0), ForceMode.Impulse);
        }

        AnimationSet(ref _animReady, "Ready", false);
        AnimationSet(ref _animJumpAttack, "JumpAttack", true);
    }
}
