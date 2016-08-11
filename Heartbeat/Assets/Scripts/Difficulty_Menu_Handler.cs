using UnityEngine;
using System.Collections;

public class Difficulty_Menu_Handler : MonoBehaviour {

    public void SetBeginner() { Game_Data.difficulty = Song_Parser.difficulties.beginner; UnityEngine.SceneManagement.SceneManager.LoadScene(0); }
    public void SetEasy() { Game_Data.difficulty = Song_Parser.difficulties.easy; UnityEngine.SceneManagement.SceneManager.LoadScene(0); }
    public void SetMedium() { Game_Data.difficulty = Song_Parser.difficulties.medium; UnityEngine.SceneManagement.SceneManager.LoadScene(0); }
    public void SetHard() { Game_Data.difficulty = Song_Parser.difficulties.hard; UnityEngine.SceneManagement.SceneManager.LoadScene(0); }
    public void SetChallenge() { Game_Data.difficulty = Song_Parser.difficulties.challenge; UnityEngine.SceneManagement.SceneManager.LoadScene(0); }
}
