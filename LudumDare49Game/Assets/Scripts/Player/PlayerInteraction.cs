using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform camera; //serves as the origin for raycasts :)
    public Transform holdObjectHere;
    public float maxInteractionDistance = 5f;
    public LayerMask interact;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Debug.Log("Interacted, or at least tried to :)");
            RaycastHit hit;
            if(Physics.Raycast(camera.position, camera.forward, out hit, maxInteractionDistance, interact))
            {
                GameObject hitObj = hit.transform.gameObject;
                
            }
        }
    }
}
