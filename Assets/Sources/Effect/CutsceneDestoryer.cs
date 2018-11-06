using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDestoryer : MonoBehaviour {
	void Start () {
        Destroy(gameObject, 3f);
	}
}
