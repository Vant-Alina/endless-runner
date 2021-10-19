using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Keeps track of where the camera starts, ends, and how long it takes to move between the two positions
    [SerializeField] Vector3 cameraStartPos;
    [SerializeField] Vector3 cameraEndPos;
    [SerializeField] float timeUntilMaxZoom;

    //Keeps track of the obstacle start speed, end speed, and how long it takes to reach the max speed
    [SerializeField] float startGameSpeed;
    [SerializeField] float maxGameSpeed;
    [SerializeField] float timeUntilMaxSpeed;

    //Keeps track of the obstacle spawn frequency, and how long it should take to reach the maximum spawn frequency
    [SerializeField] float startObstacleFrequency;
    [SerializeField] float maxObstacleFrequency;
    [SerializeField] float timeUntilMaxFrequency;

    //Creates a reference to the camera
    [SerializeField] GameObject camera;

    //Keeps track of what obstacles can spawn
    [SerializeField] GameObject[] obstacles;

    //keeps track of the obstacle speed
    public float gameSpeed;

    //Keeps track of the spawn frequency of obstacles
    public float spawnFrequency;

    //Keeps track of the time since the last obstacle spawned
    public float timeSinceLastSpawn;

    //Keeps track of how many seconds have passed since the start of the game
    float secondsPassed;

    //Keeps track of whether or not the game is over
    bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        //Moves the camera to the start pos
        camera.transform.position = cameraStartPos;

        //Sets all needed values to the start of the game
        gameSpeed = startGameSpeed;
        secondsPassed = 0;
        gameOver = false;
        timeSinceLastSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Adds the passed time to seconds passed
        secondsPassed += Time.deltaTime;

        //If the game isn't over:
        if (!gameOver)
        {
            //Move the camera by the needed amount
            if (secondsPassed < timeUntilMaxZoom)
            {
                camera.transform.position = Vector3.Lerp(cameraStartPos, cameraEndPos, secondsPassed / timeUntilMaxZoom);
            }

            //Increase the game speed by the needed amount
            if (secondsPassed < timeUntilMaxSpeed)
            {
                gameSpeed = Mathf.Lerp(startGameSpeed, maxGameSpeed, secondsPassed / timeUntilMaxSpeed);
            }

            //Increase the spawn frequency by the needed amount
            if (secondsPassed < timeUntilMaxFrequency)
            {
                spawnFrequency = Mathf.Lerp(startObstacleFrequency, maxObstacleFrequency, secondsPassed / timeUntilMaxFrequency);
            }

            //Increase the time since the last spawn by the needed amount
            timeSinceLastSpawn += Time.deltaTime;

            //If the time since the last spawn has exceeded the spawnFrequency:
            if (timeSinceLastSpawn >= spawnFrequency)
            {
                //Reset the time since the last spawn
                timeSinceLastSpawn = 0;

                //Spawn an obstacle and move it back to off camera
                GameObject obstacle = Object.Instantiate(obstacles[Random.Range(0, obstacles.Length)]);
                obstacle.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 30);
            }
        }

    }

    //If a game over happens
    public IEnumerator GameOver()
    {

        //Set game over to true
        gameOver = true;

        //Wait 3 seconds
        yield return new WaitForSeconds(3f);

        //Reset the scene
        SceneManager.LoadScene("SampleScene");
    }
}
