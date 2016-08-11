using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Step_Generator : MonoBehaviour {

    public GameObject leftArrow;
    public GameObject downArrow;
    public GameObject upArrow;
    public GameObject rightArrow;

    public GameObject leftArrowBack;
    public GameObject downArrowBack;
    public GameObject upArrowBack;
    public GameObject rightArrowBack;

    public float arrowSpeed = 0;

    private bool isInit = false;
    private Song_Parser.Metadata songData;
    private float songTimer = 0.0f;
    private float barTime = 0.0f;
    private float barExecutedTime = 0.0f;
    private GameObject heart;
    private AudioSource heartAudio;
    private Song_Parser.difficulties difficulty;
    private Song_Parser.NoteData noteData;
    private float distance;
    private float originalDistance = 1.0f;
    private float originalArrowSpeed = 0;
    private int barCount = 0;

    private Animator leftAnim;
    private Animator downAnim;
    private Animator upAnim;
    private Animator rightAnim;

	// Use this for initialization
	void Start () {
        heart = GameObject.FindGameObjectWithTag("Player");
        heartAudio = heart.GetComponent<AudioSource>();

        leftAnim = leftArrowBack.GetComponent<Animator>();
        downAnim = downArrowBack.GetComponent<Animator>();
        upAnim = upArrowBack.GetComponent<Animator>();
        rightAnim = rightArrowBack.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (leftAnim.GetBool("isLit"))
        {
            leftAnim.SetBool("isLit", false);
        }
        if (downAnim.GetBool("isLit"))
        {
            downAnim.SetBool("isLit", false);
        }
        if (rightAnim.GetBool("isLit"))
        {
            rightAnim.SetBool("isLit", false);
        }
        if (upAnim.GetBool("isLit"))
        {
            upAnim.SetBool("isLit", false);
        }

        if (isInit && barCount < noteData.bars.Count)
        {
            float pitch = heartAudio.pitch;
            arrowSpeed = originalArrowSpeed * pitch;

            distance = originalDistance;
            float timeOffset = distance * arrowSpeed;
            songTimer = heartAudio.time;

            if (songTimer - timeOffset >= (barExecutedTime - barTime))
            {
                StartCoroutine(PlaceBar(noteData.bars[barCount++]));

                barExecutedTime += barTime;
            }
        }
	}

    IEnumerator PlaceBar(List<Song_Parser.Notes> bar)
    {
        for (int i = 0; i < bar.Count; i++)
        {
            if (bar[i].left)
            {
                GameObject obj = (GameObject)Instantiate(leftArrow, new Vector3(leftArrowBack.transform.position.x, leftArrowBack.transform.position.y + distance, leftArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = leftArrowBack;
            }
            if (bar[i].down)
            {
                GameObject obj = (GameObject)Instantiate(downArrow, new Vector3(downArrowBack.transform.position.x, downArrowBack.transform.position.y + distance, downArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = downArrowBack;
            }
            if (bar[i].up)
            {
                GameObject obj = (GameObject)Instantiate(upArrow, new Vector3(upArrowBack.transform.position.x, upArrowBack.transform.position.y + distance, upArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = upArrowBack;
            }
            if (bar[i].right)
            {
                GameObject obj = (GameObject)Instantiate(rightArrow, new Vector3(rightArrowBack.transform.position.x, rightArrowBack.transform.position.y + distance, rightArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = rightArrowBack;
            }
            yield return new WaitForSeconds((barTime / bar.Count) - Time.deltaTime);
        }
    }

    public void InitSteps(Song_Parser.Metadata newSongData, Song_Parser.difficulties newDifficulty)
    {
        songData = newSongData;
        isInit = true;
        barTime = (60.0f / songData.bpm) * 4.0f;
        difficulty = newDifficulty;
        distance = originalDistance;

        switch (difficulty)
        {
            case Song_Parser.difficulties.beginner:
                arrowSpeed = 0.007f;
                noteData = songData.beginner;
                break;
            case Song_Parser.difficulties.easy:
                arrowSpeed = 0.009f;
                noteData = songData.easy;
                break;
            case Song_Parser.difficulties.medium:
                arrowSpeed = 0.011f;
                noteData = songData.medium;
                break;
            case Song_Parser.difficulties.hard:
                arrowSpeed = 0.013f;
                noteData = songData.hard;
                break;
            case Song_Parser.difficulties.challenge:
                originalArrowSpeed = 0.009f;
                arrowSpeed = 0.016f;
                noteData = songData.challenge;
                break;
            default:
                goto case Song_Parser.difficulties.easy;
        }

        originalArrowSpeed = arrowSpeed;
    }
}
