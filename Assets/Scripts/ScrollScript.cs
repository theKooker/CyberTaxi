using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    


    public string type;

    [SerializeField] private float limitx = 2.5f;

    [SerializeField] private float restartx = -4.5f;
    // Update is called once per frame
    void Update()
    {
        float speed = (type == "background" ? GameData.speedBackground : GameData.speedMidground);
        if (speed > 0)
        {
            transform.Translate(Vector2.right *  speed * Time.deltaTime);
        }

        if (transform.position.x > limitx)
        {
            transform.position = new Vector3(restartx, transform.position.y, transform.position.z);
        }
        
    }
}
