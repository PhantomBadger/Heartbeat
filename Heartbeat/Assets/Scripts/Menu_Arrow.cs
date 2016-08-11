using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Menu_Arrow : MonoBehaviour {

    public EventSystem eSys;
    public GameObject defaultSelection;

    const float offset = 0.02f;

    const float distance = 0.005f;
    const float speed = 7.0f;

	// Use this for initialization
	void Start () {
        eSys.SetSelectedGameObject(defaultSelection);
	}
	
	// Update is called once per frame
	void Update () {
        if (eSys.currentSelectedGameObject == null)
        {
            eSys.SetSelectedGameObject(defaultSelection);
        }


        Vector3 tempPos = transform.position;
        tempPos.y = eSys.currentSelectedGameObject.transform.position.y + offset;
        tempPos.x += (Mathf.Sin(Time.time * speed) * distance);
        transform.position = tempPos;
    }
}
