using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteUp : Note
{
    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnYTop, Vector3.up * SongManager.Instance.noteDespawnYTop, t); 
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
