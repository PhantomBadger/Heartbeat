using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Handler : MonoBehaviour {

    [DllImport("user32.dll")]
    private static extern void FolderBrowserDialog();

    public Text warningText;

	// Use this for initialization
	void Start () {
        Debug.Log(Game_Data.difficulty.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGameClick()
    {
        if (!Game_Data.validSongDir)
        {
            warningText.enabled = true;
            StartCoroutine(DespawnWarning());
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator DespawnWarning()
    {
        yield return new WaitForSeconds(2.5f);
        warningText.enabled = false;
    }

    public void DifficultyClick()
    {
        SceneManager.LoadScene(3);
    }

    public void FindSongsClick()
    {
        System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

        System.Windows.Forms.DialogResult result = fbd.ShowDialog();
        if (result == System.Windows.Forms.DialogResult.OK)
        {
            Game_Data.songDirectory = fbd.SelectedPath;
            if (Song_Parser.IsNullOrWhiteSpace(Game_Data.songDirectory))
            {
                Game_Data.validSongDir = false;
            }
            else
            {
                Game_Data.validSongDir = true;
            }
        }
    }
}
