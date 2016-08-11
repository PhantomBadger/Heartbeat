using UnityEngine;
using System.Collections;

public class Editing_BPM_UI : MonoBehaviour {

    Heartbeat_Controller controller;
    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Heartbeat_Controller>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        spriteRenderer.enabled = controller.editingBPM;
	}
}
