using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Heartbeat_Controller : MonoBehaviour {

    public float currentBPM = 0.0f;
    public bool editingBPM = false;

    public Text debugText;
    public Text debugText2;

    private float decayTimer = 0.0f;
    private float prevBeatTimer = 0.0f;
    private float beatTimer = 0;
    private float prevBPM = 0;
    private float animCounter = 0.0f;
    private Animator anim;
    private AudioSource audioSource;
    private bool songLoaded = false;

    private float originalBPM = 0.0f;
    private const float bpmChangeBuffer = 10.0f;
    private const float lerpLimit = 0.1f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource>();

        //Song_Parser parser = new Song_Parser();
        // Song_Parser.Metadata meta = parser.Parse(@"C:\Users\josh\CloudStation\Game Jam March 2016\Heartbeat\Assets\Music Files\Kung-Fu\KUNG%20FU%20FIGHTING.sm");
        Song_Parser.Metadata meta = Game_Data.chosenSongData;

        //TODO: Load Audio
        StartCoroutine(LoadTrack(meta.musicPath, meta));

        debugText.text = "Title: " + meta.title +
                         "\nSubtitle: " + meta.subtitle +
                         "\nArtist: " + meta.artist +
                         "\nBanner Path: " + meta.bannerPath +
                         "\nBackground Path: " + meta.backgroundPath +
                         "\nMusic Path: " + meta.musicPath +
                         "\nOffset: " + meta.offset +
                         "\nSample Start: " + meta.sampleStart +
                         "\nSample Length: " + meta.sampleLength +
                         "\nBPM: " + meta.bpm +
                         "\n\nValid: " + meta.valid;

        originalBPM = meta.bpm;

        currentBPM = originalBPM;
        prevBPM = currentBPM;
    }
	
	// Update is called once per frame
	void Update () {
		ControllerHandler ();
        //BPMDecay ();
        SongBPMChange();
        HeartBeat();

        //Debug text
        debugText2.text = "Song BPM: " + originalBPM + "\nCurrent BPM: " + currentBPM + "\nCurrent Pitch: " + audioSource.pitch;

        if (!audioSource.isPlaying && songLoaded)
        {
            //Song is over
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    IEnumerator LoadTrack(string path, Song_Parser.Metadata meta)
    {
        Debug.Log(path);
        string url = string.Format("file://{0}", path);
        WWW www = new WWW(url);

        while (!www.isDone)
        {
            yield return null;
        }

        AudioClip clip = www.GetAudioClip(false, false);
        audioSource.clip = clip;

        Debug.Log("Loaded");

        songLoaded = true;

        audioSource.Play();

        GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
        manager.GetComponent<Step_Generator>().InitSteps(meta, Game_Data.difficulty);
    }

    void HeartBeat()
    {
        if (songLoaded && !editingBPM)
        {
            //Calc how long a beat is in seconds
            float secondsPerBeat = 60.0f / currentBPM;

            //If the time has passed for one beat, beat the heart and reset
            animCounter += Time.deltaTime;
            if (animCounter >= secondsPerBeat)
            {
                animCounter = 0;
                anim.SetBool("isBeat", true);
            }
        }
    }

	void ControllerHandler()
	{
        beatTimer += Time.deltaTime;

        if (anim.GetBool("isBeat"))
		{
			anim.SetBool ("isBeat", false);
		}

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
            editingBPM = true;
			anim.SetBool ("isBeat", true);
            
            //Forcefully calc the BPM
            prevBPM = currentBPM;
            currentBPM = (1.0f / beatTimer) * 60.0f;

            prevBeatTimer = beatTimer;
            decayTimer = prevBeatTimer * 1.5f;
            beatTimer = 0.0f;
        }

        if ((decayTimer -= Time.deltaTime) <= 0)
        {
            editingBPM = false;
        }
	}
    /*
	void BPMDecay()
	{
        if ((decayTimer -= Time.deltaTime) <= 0)
        {
            float changeInY = Mathf.Abs(currentBPM - prevBPM);
            prevBPM = currentBPM;
            currentBPM = Mathf.Clamp((currentBPM - changeInY), 0, Mathf.Infinity);
            decayTimer = prevBeatTimer;
        }
    }*/

    void SongBPMChange()
    {
        float newPitch = currentBPM / originalBPM;
        if (Mathf.Abs(audioSource.pitch - newPitch) > lerpLimit)
        {
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, currentBPM / originalBPM, Time.deltaTime);
        }
        else
        {
            audioSource.pitch = currentBPM / originalBPM;
        }
    }
}