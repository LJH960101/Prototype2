using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour {
    Vector3 pos;
    void Start () {
        pos = transform.localPosition;
	}
    private void FixedUpdate()
    {
        transform.localPosition = pos;
    }
}
