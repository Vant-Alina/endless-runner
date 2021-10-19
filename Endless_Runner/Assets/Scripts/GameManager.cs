using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3 cameraStartPos;
    [SerializeField] Vector3 cameraEndPos;
    [SerializeField] float timeUntilMaxZoom;

    [SerializeField] float startGameSpeed;
    [SerializeField] float maxGameSpeed;
    [SerializeField] float timeUntilMaxSpeed;

    [SerializeField] GameObject camera;
    float gameSpeed;
    float secondsPassed;

    // Start is called before the first frame update
    void Start()
    {
        camera.transform.position = cameraStartPos;
        gameSpeed = startGameSpeed;
        secondsPassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        secondsPassed += Time.deltaTime;

        if (secondsPassed < timeUntilMaxZoom)
        {
            camera.transform.position = Vector3.Lerp(cameraStartPos, cameraEndPos, secondsPassed / timeUntilMaxZoom);
        }

        if (secondsPassed < timeUntilMaxSpeed)
        {
            gameSpeed = Mathf.Lerp(startGameSpeed, maxGameSpeed, secondsPassed / timeUntilMaxSpeed);
        }

    }
}
