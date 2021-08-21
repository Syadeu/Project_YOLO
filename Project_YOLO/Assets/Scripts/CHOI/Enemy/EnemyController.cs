using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Space(5)] [Header("랜덤으로 움직이기")]
    [SerializeField] private bool randomMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int randomPosition;
    
    //리지드바디
    private Rigidbody _rigidbody;

    //애니메이터
    private Animator _animator;
    private bool _animMove;
    
    private void Awake()
    {
        //초기화
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        
        //랜덤 포지션 시작
        if (randomMove)
        {
            InvokeRepeating(nameof(SetRandomPosition), 0, 2);
        }
    }

    private void Update()
    {
        if (randomMove)
        {
            _rigidbody.MovePosition(transform.position + (new Vector3(randomPosition, 0, 0) * moveSpeed));

            var front = new Vector3(_rigidbody.position.x + randomPosition, _rigidbody.position.y + 25, 0);
            //Debug.DrawLine(front, Vector3.down, new Color(0, 1, 0));
            Debug.DrawLine(front, front + Vector3.forward, new Color(0, 1, 0));
            var rayHit_Down = Physics2D.Raycast(front, Vector3.down, 1, LayerMask.GetMask("Floor"));
            var rayHit_Front = Physics2D.Raycast(front, Vector3.forward, 1, LayerMask.GetMask("Floor"));
            if (rayHit_Down.collider != null || rayHit_Front != null)
            {
                if (rayHit_Down.distance > 0.5f)
                {
                    randomPosition *= -1;
                    Debug.Log("HIT");
                }
                
                if (rayHit_Front.distance > 0.5f)
                {
                    randomPosition *= -1;
                    Debug.Log("HIT");
                }
            }
        }
    }

    public void SetRandomPosition()
    {
        randomPosition = Random.Range(-1, 2);

        switch (randomPosition)
        {
            case 1:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
                
            case -1:
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                break;
        }
    }
}
