using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update

    //The amount of time before an obstacle despawns
    [SerializeField] float lifeSpan;

    //Creates a reference to the gameManager
    GameManager gameManager;

    //How much the x can vary when the obstacle spawns
    [SerializeField] float spawnVariation;
    
    //The time the obstacle has existed in the scene
    float timeAlive;
    void Start()
    {
        //Sets the time alive to 0
        timeAlive = 0;

        //Creates a reference to the game manager
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        //Randomly offsets the x to a maximum value of +/-spawnVariation
        transform.position = new Vector3(transform.position.x + Random.Range(-spawnVariation, spawnVariation), transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Adds the passed time to the obstacle's time alive
        timeAlive += Time.deltaTime;

        //Updates the obstacle's position
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (gameManager.gameSpeed * Time.deltaTime));
        
        //If the obstacle has been alive longer than it's lifespan, destroy it.
        if (timeAlive > lifeSpan) {
            Object.Destroy(this);
        }
    }
}
