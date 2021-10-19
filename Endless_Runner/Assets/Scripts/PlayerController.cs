using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    //Controls how fast the player accelerates
    [SerializeField] float accelSpeed;

    //Controls the player's max speed
    [SerializeField] float maxSpeed;

    //Controls how much friction there is when the player is slowing down
    [SerializeField] float frictionAmnt;

    //Controls how much force is applied when the player jumps
    [SerializeField] float jumpForce;

    //Player rigidbody
    Rigidbody rb;
    //Direction of movement. Is either -1.0, 0.0, or 1.0
    float moveDir;

    //Keeps track of whether or not the player is trying to jump
    bool jumpInput;

    //Keeps track of whether or not the palyer is allowed to jump
    bool canJump;

    //Keeps track of whether or not the player is dead
    bool dead;

    void Start()
    {
        //Create a reference to the player rigidbody
        rb = GetComponent<Rigidbody>();

        //Makes it so the player cant jump
        canJump = false;

        //Sets the player to be alive
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Gets the movement input
        moveDir = Input.GetAxisRaw("Horizontal");

        //Checks to see if the player is jumping
        if (Input.GetAxisRaw("Vertical") > 0 || Input.GetButton("Jump"))
        {
            jumpInput = true;
        } else
        {
            jumpInput = false;
        }

    }

    void FixedUpdate()
    {
        //If the player is not dead, they are allowed to move
        if (!dead)
        {
            //If the player is trying to move, and is below their max speed:
            if (moveDir < 0 && rb.velocity.x - (accelSpeed * Time.fixedDeltaTime) > -maxSpeed ||
                moveDir > 0 && rb.velocity.x + (accelSpeed * Time.fixedDeltaTime) < maxSpeed)
            {
                //Adds the player movement velocity to their rigidbody
                rb.velocity = new Vector3(rb.velocity.x + (accelSpeed * moveDir * Time.fixedDeltaTime), rb.velocity.y, rb.velocity.z);
            }

            //If the player isn't moving, or is trying to move opposite to their current velocity
            if (moveDir == 0 || Mathf.Sign(moveDir) != Mathf.Sign(rb.velocity.x))
            {

                //Add friction to slow down the player
                rb.velocity = new Vector3(rb.velocity.x * frictionAmnt, rb.velocity.y, rb.velocity.z);
            }

            //If the player is trying to jump AND can jump
            if (jumpInput && canJump)
            {
                //Make it so the player can't jump
                canJump = false;

                //Jump
                Jump();
            }
        }
        
    }

    //Makes the player jump
    void Jump()
    {
        //Adds the jump force to the player
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }

    //Kills the player
    void Die()
    {

        //Sets the player to be dead
        dead = true;

        //Removes rotational and position restraints
        rb.constraints = RigidbodyConstraints.None;

        //Adds a random backwards force to the player
        rb.AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), ForceMode.Impulse);

        //Tells the gameManager that it's game over
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().StartCoroutine("GameOver");
    }

    //Checks to see if the player collided with something
    private void OnCollisionEnter(Collision collision)
    {
        //If so, check to see if it's ground
        if (collision.gameObject.tag == "ground") {
            //If so, let the player jump again
            canJump = true;

        //If it's not ground, check if it's an obstacle
        } else if (collision.gameObject.tag == "obstacle")
        {
            //If it is, kill the player
            Die();
        }
    }
}
