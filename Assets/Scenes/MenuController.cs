using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Games to Load")]
    public string _newGame;
    private string gameToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGame);
    }

    public void LoadGameDialogYes()
    {
        if(PlayerPrefs.HasKey("SavedGame"))
        {
            gameToLoad = PlayerPrefs.GetString("SavedGame");
            SceneManager.LoadScene(gameToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}