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
    // public int Screenshot_Res_Multiplier = 1;

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
                    if(!objectInHands)
                    {
                        //Deal with the weighted object
                        WeightedObject weightedObject = hitObj.GetComponent<WeightedObject>();
                        if(!weightedObject.inHands)
                        {
                            objectInHands = weightedObject.pickup(holdObjectHere.position, holdObjectHere.transform);
                        }
                    }
                }
            }
            Debug.DrawRay(myCamera.position, myCamera.forward, Color.black, 10f);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<PlayerMovement>().Die();
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
                objectInHands = null;
            }
        }

        // if(Input.GetKeyDown(KeyCode.F1))
        // {
        //     TakeScreenshot();
        // }
    }


    // public void TakeScreenshot()
    //  {
    //     string folderPath = System.IO.Directory.GetCurrentDirectory() + "/Screenshots/";
    //     print(folderPath);

    //     if (!System.IO.Directory.Exists(folderPath))
    //         System.IO.Directory.CreateDirectory(folderPath);

    //     var screenshotName = 
    //                             "Screenshot_" + 
    //                             System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + 
    //                             ".png";
    //     ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), Screenshot_Res_Multiplier);
    //     Debug.Log(folderPath + screenshotName);
    //  }
}
