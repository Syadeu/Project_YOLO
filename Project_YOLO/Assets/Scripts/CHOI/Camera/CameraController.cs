using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 targetPosition;
    [SerializeField] Transform Target;
    [SerializeField] private float moveSpeed;
    
    public Camera camera;
    
    static CameraController _instance;

    public static CameraController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraController>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        targetPosition = transform.position;
    }

    public void SetPosition(float position)
    {
        targetPosition = new Vector3(position, targetPosition.y, targetPosition.z);
    }

    void Update()
    {
        var position = transform.position;
        position = Vector3.Lerp(position, new Vector3(targetPosition.x, Target.position.y + targetPosition.y, targetPosition.z), Time.deltaTime * moveSpeed);
        transform.position = position;
    }
}
