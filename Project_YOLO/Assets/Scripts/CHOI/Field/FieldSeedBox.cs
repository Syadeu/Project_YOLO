using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSeedBox : MonoBehaviour
{
    public Transform muzzle;
    public FieldSeed seed;

    public void GetSeed()
    {
        //씨앗 만들기
        var _seed = Instantiate(seed, muzzle.position, muzzle.rotation);
        _seed.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-70f, -35f), Random.Range(45f, 60f), 0), ForceMode.Impulse);
    }
}
