using UnityEngine;
using System.Collections;

public class BG_Colour_Changer : MonoBehaviour {

    private Camera camera;
    //private Color nextCol;
    private float speed = 0.5f;

    private const float lerpLimit = 0.025f;

	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
        //nextCol = camera.backgroundColor;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Mathf.Abs(camera.backgroundColor.r - nextCol.r) >= lerpLimit &&
            Mathf.Abs(camera.backgroundColor.g - nextCol.g) >= lerpLimit &&
            Mathf.Abs(camera.backgroundColor.b - nextCol.b) >= lerpLimit)
        {
            camera.backgroundColor = new Color(Mathf.Lerp(camera.backgroundColor.r, nextCol.r, Time.deltaTime * speed),
                                               Mathf.Lerp(camera.backgroundColor.g, nextCol.g, Time.deltaTime * speed),
                                               Mathf.Lerp(camera.backgroundColor.b, nextCol.b, Time.deltaTime * speed));
        }
        else
        {
            Color randCol = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            nextCol = randCol;
        }*/

        HSBColor randCol = new HSBColor(Mathf.PingPong(Time.time * speed, 1), 1, 1);
        randCol.s = randCol.s / 2;
        camera.backgroundColor = randCol.ToColor();
    }
}
