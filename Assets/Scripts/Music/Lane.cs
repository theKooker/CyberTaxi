using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public Vector2 input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<Tuple<double, bool>> timeStamps = new List<Tuple<double, bool>>();
    public TextMeshProUGUI winLoseText;
    public GameObject musicPlane;
    public ParticleSystem hitpart;

    int spawnIndex = 0;
    int inputIndex = 0;

    [SerializeField] private ScoreManager scoreManager;

    private bool _durationNote = false;
    private bool _isFinished = false;
    
    [SerializeField] private GameObject DurationNoteEffect;
    [SerializeField] private Transform DurationNoteEffectPlacer;
    private GameObject _currentDurationNoteEffect = null;

    private RhythmController _controller;
    private double timeStamp, marginOfError, audioTime;

    private void Awake()
    {
        _controller = new RhythmController();
    }

    private void Start()
    {
        _controller.MAIN.CTRL.performed += Pop;
        _controller.MAIN.CTRL.canceled += Release;
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add(new Tuple<double, bool>((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f, note.Length >= 48));
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.gameOver)
            return;
        
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex].Item1 - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                //Debug.Log("Length: " + SongManager.array[spawnIndex].Length);
                if (timeStamps[spawnIndex].Item2)
                {
                    Debug.Log("Detected");
                    note.GetComponent<Note>().SetDurationNote();
                }
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex].Item1;
                spawnIndex++;
            }
        }
        else
        {
            _isFinished = true;
        }

        if (inputIndex < timeStamps.Count)
        {
            timeStamp = timeStamps[inputIndex].Item1;
            marginOfError = SongManager.Instance.marginOfError;
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                inputIndex++;
            }
        }
    }
    private void Hit()
    {
        if (hitpart != null) hitpart.Play();
        scoreManager.Hit();
    }
    private void Miss()
    {
        scoreManager.Miss();
    }

    public bool IsFinished()
    {
        return _isFinished;
    }
    
    private void Pop(InputAction.CallbackContext context)
    {
        if (ScoreManager.gameOver)
            return;
        
        if (_durationNote)
        {
            return;
        }

        if (input == Vector2.up)
        {
            if (!(_controller.MAIN.CTRL.ReadValue<Vector2>().y > 0f))
            {
                return;
            }
        }
        else if (input == Vector2.down)
        {
            if (!(_controller.MAIN.CTRL.ReadValue<Vector2>().y < 0f))
            {
                return;
            }
        }
        else if (input == Vector2.left)
        {
            if (!(_controller.MAIN.CTRL.ReadValue<Vector2>().x < 0f))
            {
                return;
            }
        }
        else if (input == Vector2.right)
        {
            if (!(_controller.MAIN.CTRL.ReadValue<Vector2>().x > 0f))
            {
                return;
            }
        }
        
        if (Math.Abs(audioTime - timeStamp) < marginOfError)
        {
            if (notes[inputIndex] != null && notes[inputIndex].GetComponent<Note>().IsDurationNote())
            {
                _durationNote = true;
                _currentDurationNoteEffect = Instantiate(DurationNoteEffect, DurationNoteEffectPlacer);
            }
            Hit();
            Destroy(notes[inputIndex].gameObject);
            inputIndex++;
        }
    }
    
    private void Release(InputAction.CallbackContext context)
    {
        if (ScoreManager.gameOver)
            return;
        
        if (!_durationNote)
        {
            Debug.Log("i go into thiss");
            return;
        }
        
        //we can only come here if the start of duration note is Hit
        Debug.Log("Im in Duration Mode");
        Destroy(_currentDurationNoteEffect);
        _durationNote = false;
        if (Math.Abs(audioTime - timeStamp) < marginOfError)
        {
            Hit();
            Destroy(notes[inputIndex].gameObject);
            inputIndex++;
        }
        else
        {
            Miss();
            inputIndex++;
        }
    }

    private void OnEnable()
    {
        _controller.Enable();
    }

    private void OnDisable()
    {
        _controller.MAIN.CTRL.canceled -= Release;
        _controller.MAIN.CTRL.started -= Pop;
        _controller.Disable();
    }
}
