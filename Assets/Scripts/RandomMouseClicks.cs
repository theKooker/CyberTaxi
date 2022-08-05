using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMouseClicks : MonoBehaviour
{
    [SerializeField] private List<AudioClip> Clicks;

    private AudioSource _jukebox;

    private void Awake()
    {
        _jukebox = GetComponent<AudioSource>();
    }

    private AudioClip ChooseRandomClickSound()
    {
        return Clicks[Random.Range(0, Clicks.Count - 1)];
    }

    public void PlayClick()
    {
        _jukebox.PlayOneShot(ChooseRandomClickSound());
    }
}
