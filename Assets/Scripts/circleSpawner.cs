using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class circleSpawner : MonoBehaviour
{
    [SerializeField]
    float minX,maxX,minY,maxY;
    [SerializeField]
    GameObject Circle;
    // Start is called before the first frame update
    void Awake()
    {
        // Here We Generate 5-10 Circles on the Screen
        for(int i=0; i<Random.Range(5,10); i++)
        {
            Instantiate(Circle, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), Quaternion.identity);
        }
    }

    //Public Function for restartGame
    public void Restart()
    {
        SceneManager.LoadScene("Task2"); // Here We Load Scene Again
    }
}
