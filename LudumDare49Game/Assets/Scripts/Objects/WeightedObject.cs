using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{

    public bool inHands;
    public float myWeight = 1f;
    public Rigidbody rigidbody;

    public void pickup() //This is going to need a transform parameter for where to stow object. 1 at a time folks.
    {
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;
    }

    public void drop() //This is going to need parameters for where to throw it :)
    {
        rigidbody.isKinematic = false;
        rigidbody.detectCollisions = true;
    }
}
