using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorVisibiltyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
            {
                Cursor.visible = false;
                break;
            }
            case "Level2":
            {
                Cursor.visible = false;
                break;
            }
            case "Level3":
            {
                Cursor.visible = false;
                break;
            }
            default:
                Cursor.visible = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
