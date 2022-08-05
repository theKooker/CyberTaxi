using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [FormerlySerializedAs("backgroundSpeed")]
    [Header("Background Reference")]
    [SerializeField] private float backgroundSpeed = 0.5f;
    [SerializeField] private GameObject background;
    private float _scaleBackground;
    [SerializeField] private float fargroundSpeed = 1f;
    [SerializeField] private GameObject farground;
    private float _scaleFarground;
    [SerializeField] private float midgroundSpeed = 1.5f;
    [SerializeField] private GameObject midground;
    private float _scaleMidground;
    [SerializeField] private float neargroundSpeed = 2f;
    [SerializeField] private GameObject nearground;
    private float _scaleNearground;
    [SerializeField] private float frontgroundSpeed = 2.5f;
    [SerializeField] private GameObject frontground;
    private float _scaleFrontground;

    [Header("Positions")] 
    [SerializeField] private Transform camPivot;

    [Header("UI Elements")] 
    [SerializeField] private GameObject backToMenuBtn;
    [SerializeField] private GameObject retryStageBtn;

    private List<GameObject> _backgroundList = new List<GameObject>();
    private List<GameObject> _fargroundList = new List<GameObject>();
    private List<GameObject> _midgroundList = new List<GameObject>();
    private List<GameObject> _neargroundList = new List<GameObject>();
    private List<GameObject> _frontgroundList = new List<GameObject>();

    [Header("Driving Speed")]
    [SerializeField] private float _driveSpeedMul = 1f;
    
    private bool _gameOver = false;
    private bool _lost = false;

    [Header("Animators")]
    [SerializeField] private Animator missAnimController;
    [SerializeField] private Animator scoreAnimController;
    [SerializeField] private Animator perfAnimController;
    
    [Header("Song Management")]
    [SerializeField] private SongManager songManager;
    [SerializeField] private AudioSource lostMusic;
    [SerializeField] private AudioSource wonMusic;
    
    private bool _showStats = false;

    private void Start()
    {
        backToMenuBtn.SetActive(false);
        var cam = GetComponent<Camera>();
        cam.backgroundColor = Color.black;

        foreach (Transform child in background.transform)
        {
            if (child.CompareTag("Background"))
            {
                _backgroundList.Add(child.gameObject);
                _scaleBackground = child.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }
        foreach (Transform child in farground.transform)
        {
            if (child.CompareTag("Background"))
            {
                _fargroundList.Add(child.gameObject);
                _scaleFarground = child.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }
        foreach (Transform child in midground.transform)
        {
            if (child.CompareTag("Background"))
            {
                _midgroundList.Add(child.gameObject);
                _scaleMidground = child.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }
        foreach (Transform child in nearground.transform)
        {
            if (child.CompareTag("Background"))
            {
                _neargroundList.Add(child.gameObject);
                _scaleNearground = child.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }
        foreach (Transform child in frontground.transform)
        {
            if (child.CompareTag("Background"))
            {
                _frontgroundList.Add(child.gameObject);
                _scaleFrontground = child.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
   
        if (_gameOver)
        {
            var highScore = PlayerPrefs.GetInt("highscore", 0);
            if (ScoreManager.score > highScore)
            {
                PlayerPrefs.SetInt("highscore", ScoreManager.score);
            }
            transform.position = Vector3.Lerp(transform.position, camPivot.position, 8f * Time.deltaTime);
            if (!_showStats)
                StartCoroutine(GameOver());
            backToMenuBtn.SetActive(true);
            retryStageBtn.SetActive(true);
            return;
        }

        background.transform.position += new Vector3(backgroundSpeed * _driveSpeedMul, 0, 0) * Time.deltaTime;
        farground.transform.position += new Vector3(fargroundSpeed * _driveSpeedMul, 0, 0) * Time.deltaTime;
        midground.transform.position += new Vector3(midgroundSpeed * _driveSpeedMul, 0, 0) * Time.deltaTime;
        nearground.transform.position += new Vector3(neargroundSpeed * _driveSpeedMul, 0, 0) * Time.deltaTime;
        frontground.transform.position += new Vector3(frontgroundSpeed * _driveSpeedMul, 0, 0) * Time.deltaTime;
        GeneratePic(_backgroundList, _scaleBackground); //16.28f
        GeneratePic(_fargroundList, _scaleFarground); //2.008f
        GeneratePic(_midgroundList, Random.Range(_scaleMidground, _scaleMidground*3));
        GeneratePic(_neargroundList, Random.Range(_scaleNearground, _scaleNearground*3));
        GeneratePic(_frontgroundList, _scaleFrontground); //6.04f
    }

    private void GeneratePic(IList<GameObject> picList, float scaleX)
    {
        if(_gameOver)
            return;

        if (picList.Count == 0)
            return;
        
        if (!(picList[0].transform.position.x >= this.transform.position.x + 30f)) 
            return;
        
        var instance = picList[0];
        picList.RemoveAt(0);
            
        var last = picList[picList.Count() - 1];
        var pos = last.transform.position;
        pos -= new Vector3(scaleX, 0, 0);
        instance.transform.position = pos;
            
        picList.Add(instance);
    }

    public void GearUp()
    {
        if(_gameOver)
            return;
        
        if(_driveSpeedMul >= 4f)
            return;
        
        _driveSpeedMul += 0.1f;
    }
    
    public void GearDown()
    {
        if(_gameOver)
            return;

        _driveSpeedMul -= 0.2f;

        if (_driveSpeedMul <= 0f)
        {
            Debug.Log("LOST GAME");
            _gameOver = true;
            _lost = true;
            ScoreManager.gameOver = true;
        }
    }

    public void EndStage()
    {
        _gameOver = true;
    }

    private IEnumerator GameOver()
    {
        songManager.StopSong();
        if (_lost)
        {
            lostMusic.Play();
        }
        else
        {
            wonMusic.Play();
        }
        _showStats = true;
        yield return new WaitForSeconds(2f);
        missAnimController.SetTrigger("ShowMisses");
        yield return new WaitForSeconds(3f);
        scoreAnimController.SetTrigger("ShowScore");
        yield return new WaitForSeconds(3f);
        perfAnimController.SetTrigger("ShowPerf");
    }
}
