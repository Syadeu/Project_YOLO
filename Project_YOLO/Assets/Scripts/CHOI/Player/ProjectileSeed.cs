using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSeed : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destoyTime;
    private void Start()
    {
        Invoke(nameof(Destroy), destoyTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * speed));
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
