using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public TextMesh scoreText;
    public static int score = 0;
    public static int missScore = 0;
    public static int comboScore = 0;
    public static string perf = "SSS";
    public static bool gameOver = false;

    [SerializeField] private int comboScoreGoal = 10;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private Material shaderMat;

    private bool _flowActive = false;
    private static int _maxScore = 0;
    
    [SerializeField] private List<AudioClip> Misses;
    private AudioSource _jukebox;

    private void Awake()
    {
        _jukebox = GetComponent<AudioSource>();
    }

    void Start()
    {
        Instance = this;
        scoreText.text = score.ToString();
        Debug.Log(SceneManager.GetActiveScene().name);
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
            {
                _maxScore = 181;
                break;
            }
            case "Level2":
            {
                _maxScore = 125;
                break;
            }
            case "Level3":
            {
                _maxScore = 77;
                break;
            }
            default:
                _maxScore = 1;
                break;
        }
    }
    
    private AudioClip ChooseRandomMissSound()
    {
        return Misses[Random.Range(0, Misses.Count - 1)];
    }
    
    public void Hit()
    {
        score += 1;
        comboScore += 1;
        hitSFX.Play();
        cameraController.GearUp();
        scoreText.text = score.ToString();
    }
    public void Miss()
    {
        missScore++;
        comboScore = 0;
        _jukebox.PlayOneShot(ChooseRandomMissSound());
        cameraController.GearDown();
    }
    private void Update()
    {
        if (comboScore == 0 && _flowActive)
        {
            StopAllCoroutines();
            StartCoroutine(DeactivateFlow());
            _flowActive = false;
        }
        else if (comboScore >= comboScoreGoal && !_flowActive)
        {
            StopAllCoroutines();
            StartCoroutine(ActivateFlow());
            _flowActive = true;
        }
    }

    private void OnDestroy()
    {
        shaderMat.SetFloat("_FullscreenIntensity", 0f);
    }

    private IEnumerator ActivateFlow()
    {
        float timeScale = 0;
        float speed = 0.75f;
        float start = 0;

        while(timeScale < 1)
        {
            timeScale += Time.deltaTime * speed;
            shaderMat.SetFloat("_FullscreenIntensity", Mathf.Lerp(start, 0.25f, timeScale));
            yield return null;
        }
    }
    
    private IEnumerator DeactivateFlow()
    {
        float timeScale = 0;
        float speed = 0.75f;
        float start = 0.25f;

        while(timeScale < 1)
        {
            timeScale += Time.deltaTime * speed;
            shaderMat.SetFloat("_FullscreenIntensity", Mathf.Lerp(start, 0f, timeScale));
            yield return null;
        }
    }

    public static void CalculatePerformance()
    {
        string grade = "";
        Debug.Log(_maxScore);
        Debug.Log(score);
        Debug.Log(missScore);
        float perc = (100f / _maxScore) * score;
        Debug.Log(perc);
        if (missScore > 0)
        {
            if (missScore < 5)
            {
                perc -= 2.5f;
            }
            else if (missScore < 10)
            {
                perc -= 5f;
            }
            else if (missScore < 15)
            {
                perc -= 7.5f;
            }
            else
            {
                perc -= 10f;
            }
        }

        Debug.Log(perc);
        
        if (perc >= 98f)
        {
            grade = "S+";
        }
        else if (perc >= 95f)
        {
            grade = "S";
        }
        else if (perc >= 80f)
        {
            grade = "A";
        }
        else if (perc >= 70f)
        {
            grade = "B";
        }
        else if (perc >= 60f)
        {
            grade = "C";
        }
        else if (perc >= 50f)
        {
            grade = "D";
        }
        else
        {
            grade = "F";
        }
        perf = grade;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        score = 0;
        missScore = 0;
        comboScore = 0;
        gameOver = false;
    }
}
