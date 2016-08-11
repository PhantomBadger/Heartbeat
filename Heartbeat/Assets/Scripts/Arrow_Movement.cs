using UnityEngine;
using System.Collections;

public class Arrow_Movement : MonoBehaviour {

    public GameObject arrowBack;

    private Step_Generator gen;
    private float arrowSpeed = 0;
    private Score_Handler scoreHandler;
    private bool scoreApplied = false;

    public direction dir;
    public enum direction {  left, down, up, right };

    private const float strumOffset = 0.075f;
    private const float despawnTime = 1.5f;

	// Use this for initialization
	void Start () {
        gen = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Step_Generator>();
        scoreHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Score_Handler>();

        switch(GetComponent<SpriteRenderer>().sprite.name)
        {
            case "arrowsheet_down":
                dir = direction.down;
                break;
            case "arrowsheet_left":
                dir = direction.left;
                break;
            case "arrowsheet_right":
                dir = direction.right;
                break;
            case "arrowsheet_up":
                dir = direction.up;
                break;
        }
	}

    // Update is called once per frame
    void Update() {

        arrowSpeed = gen.arrowSpeed;
        Vector3 tempPos = transform.position;
        tempPos.y -= arrowSpeed;
        transform.position = tempPos;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && dir == direction.left)
        {
            CheckLocation();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && dir == direction.down)
        {
            CheckLocation();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && dir == direction.up)
        {
            CheckLocation();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && dir == direction.right)
        {
            CheckLocation();
        }

        //Missed
        if (transform.position.y < arrowBack.transform.position.y - strumOffset)
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(0.5f, 0.0f, 0.0f));
            StartCoroutine(DespawnArrow());
            if (!scoreApplied)
            {
                scoreApplied = true;
                scoreHandler.SendMessage("LoseScore");
            }
        }
    }

    void CheckLocation()
    {
        if (transform.position.y >= arrowBack.transform.position.y - strumOffset && transform.position.y <= arrowBack.transform.position.y + strumOffset)
        {
            arrowBack.GetComponent<Animator>().SetBool("isLit", true);
            scoreHandler.SendMessage("AddScore");
            Destroy(this.gameObject);
        }
    }

    IEnumerator DespawnArrow()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}
