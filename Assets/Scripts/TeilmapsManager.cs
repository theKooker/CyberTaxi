using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TeilmapsManager : MonoBehaviour
{

    [SerializeField] private GameObject[] teilmaps;
    [SerializeField] private float speed;
    [SerializeField] private float limitx;
    [SerializeField] private float restartx;
    [SerializeField] private GameObject currentTileMap;

    [SerializeField] private GameObject currentTileMap2;
    // Start is called before the first frame update
    void Awake()
    {
        int pickup = Random.Range(0, teilmaps.Length);
        currentTileMap = Instantiate(teilmaps[pickup], new Vector2(0, 0), Quaternion.identity);
        currentTileMap.transform.parent = GameObject.Find("Grid").transform;
        pickup = Random.Range(0, teilmaps.Length);
        currentTileMap2 = Instantiate(teilmaps[pickup], new Vector2(restartx, 0), Quaternion.identity);
        currentTileMap2.transform.parent = GameObject.Find("Grid").transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentTileMap.gameObject.transform.Translate(Vector3.right * GameData.speedTeilmaps * Time.deltaTime);
        currentTileMap2.gameObject.transform.Translate(Vector3.right * GameData.speedTeilmaps * Time.deltaTime);
        if (currentTileMap.gameObject.transform.position.x > limitx)
        {
            Destroy(currentTileMap);
            int pickup = Random.Range(0, teilmaps.Length);
            currentTileMap = Instantiate(teilmaps[pickup], new Vector2(restartx, 0), Quaternion.identity);
            currentTileMap.transform.parent = GameObject.Find("Grid").transform;
        }
        if (currentTileMap2.gameObject.transform.position.x > limitx)
        {
            Destroy(currentTileMap2);
            int pickup = Random.Range(0, teilmaps.Length);
            currentTileMap2 = Instantiate(teilmaps[pickup], new Vector2(restartx, 0), Quaternion.identity);
            currentTileMap2.transform.parent = GameObject.Find("Grid").transform;
        }

    }
}
