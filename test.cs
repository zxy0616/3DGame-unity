using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    void Awake()
    {
        Debug.Log("awake");
    }
    void Start () {
        Debug.Log("start");
	}
    void Update () {
        Debug.Log("update");
	}
    void FixedUpdate()
    {
        Debug.Log("fixedupdate");
    }
    void LateUpdate()
    {
        Debug.Log("lateupdate");
    }

    void OnGUI()
    {
        Debug.Log("ongui");
    }
    void OnDisable()
    {
        Debug.Log("ondisable");
    }
    void OnEnable()
    {
        Debug.Log("onenable");
    }
}
