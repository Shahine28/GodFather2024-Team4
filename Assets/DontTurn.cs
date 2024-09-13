using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontTurn : MonoBehaviour
{

    public Transform transform;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
