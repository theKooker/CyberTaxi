using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;
    

    public string fileLocation;
    public float noteTime;
    public float noteSpawnYTop, noteTapYTop;
    public float noteSpawnYBot, noteTapYBot;
    public float noteSpawnXRight, noteTapXRight;
    public float noteSpawnXLeft, noteTapXLeft;
    public float noteDespawnYTop
    {
        get
        {
            return noteTapYTop - (noteSpawnYTop - noteTapYTop);
        }
    }
    public float noteDespawnYBot
    {
        get
        {
            return noteTapYBot - (noteSpawnYBot - noteTapYBot);
        }
    }
    public float noteDespawnXRight
    {
        get
        {
            return noteTapXRight - (noteSpawnXRight - noteTapXRight);
        }
    }
    public float noteDespawnXLeft
    {
        get
        {
            return noteTapXLeft - (noteSpawnXLeft - noteTapXLeft);
        }
    }

    public static MidiFile midiFile;

    public static Melanchall.DryWetMidi.Interaction.Note[] array;

    public CameraController camControll;
    [SerializeField] private Animator musicTitle;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
        StartCoroutine(ShowTitle());
    }

    public void StopSong()
    {
        audioSource.Stop();
    }
    
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    private void Update()
    {
        foreach (var lane in lanes)
        {
            if (!lane.IsFinished())
                return;
        }

        StartCoroutine(EndStage());
    }

    private IEnumerator EndStage()
    {
        yield return new WaitForSeconds(7f);
        camControll.EndStage();
    }

    private IEnumerator ShowTitle()
    {
        yield return new WaitForSeconds(2f);
        musicTitle.SetTrigger("ShowTitle");
    }
}
