using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] public List<GameObject> weights = new List<GameObject>();
    
    [Header("Moments")]
    public float totalClockwiseMoment = 0f;
    public float totalAntiClockwiseMoment = 0f;
    public float direction = 0f; //+ve values will cause the ship to turn right.

    [Header("Movement")]
    public float standardSpeed = 5f;
    public float accelPerSecond = 0.01f;
    public float steeringSensRed = 75f;
    public float rotationSpeed = 0.1f;

    void FixedUpdate()
    {
        Moments();
        Move();
        UpdateRotation();
    }

    void Move()
    {
        Vector3 moveDir = transform.forward;
        moveDir.x += direction / steeringSensRed;
        Debug.DrawRay(transform.position, moveDir * 100, Color.green, 15f);
        transform.Translate(moveDir.normalized * standardSpeed * Time.deltaTime);
    }

    void UpdateRotation()
    {
        Vector3 currentAngle = transform.eulerAngles;
        //    Vector3 Rotation = new Vector3(currentAngle.x, currentAngle.y, direction);
        float targetZ = -direction * 1.2f;
        currentAngle.z = Mathf.LerpAngle(currentAngle.z, targetZ, Time.deltaTime * rotationSpeed);
        transform.eulerAngles = currentAngle;
    }

    void Moments()
    {
        totalAntiClockwiseMoment = 0;
        totalClockwiseMoment = 0;
        foreach(GameObject weight in weights)
        {
            WeightedObject weightedObject = weight.GetComponent<WeightedObject>();
            // Debug.Log(weightedObject.distanceFromMid);
            if(weightedObject.distanceFromMid < 0) //Acting anticlockwise
            {
                totalAntiClockwiseMoment += ((-weightedObject.distanceFromMid) * weightedObject.myWeight);
                // Debug.Log($"Clockwise distance: {weightedObject.distanceFromMid}, clockwise moment: {((-weightedObject.distanceFromMid) * weightedObject.myWeight)}");
            }
            if(weightedObject.distanceFromMid > 0) //Acting clockwise
            {
                totalClockwiseMoment += ((weightedObject.distanceFromMid) * weightedObject.myWeight);
                // Debug.Log($"AntiClockwise distance: {weightedObject.distanceFromMid}, AntiClockwise moment: {((-weightedObject.distanceFromMid) * weightedObject.myWeight)}");
            }
        }

        float resultant = totalClockwiseMoment - totalAntiClockwiseMoment; // negative values will therefore turn the boat left
        direction = resultant;
    }

    
}
