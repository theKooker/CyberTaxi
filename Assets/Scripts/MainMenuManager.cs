using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public GameObject mainMenuPart;
    public GameObject levelSelectionPart;
    public GameObject cybertaxi;
    void Start()
    { 
        levelSelectionPart.SetActive(false);
        mainMenuPart.SetActive(true);
        highScoreText.text = PlayerPrefs.GetInt("highscore", 0).ToString();
    }
    
    public void LoadLevel(string levelName)
    {
        LoadSceneWithLoadBar(levelName);
        //SceneManager.LoadScene(levelName);
    }
    public void StartGame()
    {
        levelSelectionPart.SetActive(true);
        mainMenuPart.SetActive(false);
    }
    public void Back()
    {
        levelSelectionPart.SetActive(false);
        mainMenuPart.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private async void LoadSceneWithLoadBar(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);

        do
        {
            await Task.Delay(10);
            cybertaxi.transform.position = new Vector3(-10*scene.progress, cybertaxi.transform.position.y, cybertaxi.transform.position.z);
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
    }
}
