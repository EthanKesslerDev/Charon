using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform myCamera; //serves as the origin for raycasts :)
    public Transform holdObjectHere;
    public float maxInteractionDistance = 10f;

    public LayerMask interactLayer;
    public LayerMask boatLayer;

    GameObject objectInHands;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //Left click
        {
            RaycastHit hit;
            if(Physics.Raycast(myCamera.position, myCamera.forward, out hit, maxInteractionDistance, interactLayer))
            {
                
                GameObject hitObj = hit.transform.gameObject;
                if(hitObj.GetComponent<WeightedObject>() != null)
                {
                    //Deal with the weighted object
                    WeightedObject weightedObject = hitObj.GetComponent<WeightedObject>();
                    if(!weightedObject.inHands)
                    {
                        objectInHands = weightedObject.pickup(holdObjectHere.position, holdObjectHere.transform);
                    }
                }
            }
            Debug.DrawRay(myCamera.position, myCamera.forward, Color.black, 10f);
        }

        if(Input.GetMouseButtonDown(1))
        {
            // Debug.Log("Right click");
            if(objectInHands)
            {
                WeightedObject weightedObject = objectInHands.GetComponent<WeightedObject>();
                RaycastHit hit;
                if(Physics.Raycast(myCamera.position, myCamera.forward, out hit, 25f, boatLayer))
                {
                    weightedObject.drop(hit.point, hit.transform, true);
                    
                } else {
                    weightedObject.drop(myCamera.forward * 10f, null, false, 200f);
                }
            }
        }
    }
}
