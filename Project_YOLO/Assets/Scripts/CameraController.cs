using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

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

    void Update()
    {
        var position = transform.position;
        position = Vector3.Lerp(position, new Vector3(target.position.x, position.y, position.z), Time.deltaTime * moveSpeed);
        transform.position = position;
    }
}
