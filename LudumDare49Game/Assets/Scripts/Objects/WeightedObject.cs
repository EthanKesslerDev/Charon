using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{

    public bool inHands = false;
    bool fallen = false;
    public float myWeight = 1f;
    public Rigidbody rb;
    
    [Header("Info for boat")]
    public float distanceFromMid;
    public bool onBoat = false; //True when located on the boat
    public LayerMask boatLayer;
    Boat theBoat;

    void Start()
    {
        theBoat = GameObject.FindObjectOfType<Boat>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    void Update()
    {

        if(Vector3.Distance(theBoat.transform.position, transform.position) <= 32f) //Dont question why 30, its just some maths I did
        {
            if(!fallen)
            {
                fall();
            }
        }

        if(onBoat)
        {
            //THIS IS NOT THE DISTANCE THAT SHOULD BE USED FOR MOMENTS... but i can't be bothered
            distanceFromMid = Vector3.Distance(theBoat.transform.position, transform.position); //gives us the horizontal distance from the boat theoretically :)
            Vector3 myRight = transform.right;
            Vector3 boatDir = theBoat.transform.position - transform.position;
            if(Vector3.Dot(Vector3.right, boatDir) > 0)
            {
                 //IF THE BOAT IS TO YOUR RIGHT, YOU ARE ON THE LEFT: ANTICLOCKWISE MOMENT
                distanceFromMid *= -1;
            }
        }
    }

    public void fall()
    {
        // Debug.Log("Falling");
        rb.isKinematic = false;
        rb.detectCollisions = true;
        fallen = true;
    }

    public GameObject pickup(Vector3 toPosition, Transform parent) //This is going to need a transform parameter for where to stow object. 1 at a time folks.
    {
        inHands = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;


        transform.position = toPosition;
        transform.parent = parent;
        
        return gameObject;
    }

    /// <summary>
    /// Drop the object with either rb physics or to a point on the boat.
    /// </summary>
    /// <param name="toPosition"></param>
    /// <param name="parent"></param>
    /// <param name="placedOnBoat"></param>
    /// <param name="force"></param>
    public void drop(Vector3 toPosition, Transform parent, bool placedOnBoat, float force = 12f) //This is going to need parameters for where to throw it :)
    {
        if(placedOnBoat)
        {
            toPosition.y += 1f;
            transform.parent = theBoat.transform; //For now we're just not going to deal with this bs
            transform.position = toPosition;
            rb.detectCollisions = true;
            rb.isKinematic = false;
        } else {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.AddForce(toPosition * force);
            transform.parent = null;
        }
        inHands = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision);
        if(collision.gameObject.CompareTag("boat"))
        {
            // Debug.Log("Entered boat");
            transform.parent = theBoat.transform;
            onBoat = true;

            //Check if the weight is known to the boat inventory
            if(!theBoat.weights.Contains(gameObject))
            {
                theBoat.weights.Add(gameObject);
            }
            //Sort out the moments on the boat.
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("boat"))
        {
            // Debug.Log("Left boat");
            if(theBoat.weights.Contains(gameObject))
            {
                theBoat.weights.Remove(gameObject);
            }
            onBoat = false;
        }
    }
}
