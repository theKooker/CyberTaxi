using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    protected double timeInstantiated;
    public float assignedTime;
    private bool _durationNote = false;
    private void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Taxi"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("Taxi"))
        {
            Destroy(gameObject);
        }
    }

    public bool IsDurationNote()
    {
        return _durationNote;
    }

    public void SetDurationNote()
    {
        _durationNote = true;
        ChangeParticleEffect();
    }

    private void ChangeParticleEffect()
    {
        GetComponentsInChildren<ParticleSystem>()[0].Stop();
        GetComponentsInChildren<ParticleSystem>()[1].Play();
    }
}
