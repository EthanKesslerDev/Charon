using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] public List<GameObject> weights = new List<GameObject>();
    public ParticleSystem smoke;
    
    [Header("Moments")]
    public float totalClockwiseMoment = 0f;
    public float totalAntiClockwiseMoment = 0f;
    public float direction = 0f; //+ve values will cause the ship to turn right.

    [Header("Movement")]
    public float standardSpeed = 5f;
    public float maxSpeed = 25f;
    public float accelPerSecond = 0.05f;
    public float steeringSensRed = 75f;
    public float rotationSpeed = 0.1f;

    bool destroyed = false;

    void Start()
    {
        smoke.Play();
    }

    void FixedUpdate()
    {
        Moments();
        Move();
        UpdateRotation();
        standardSpeed += accelPerSecond * Time.time; // Acceleration
        standardSpeed = Mathf.Clamp(standardSpeed, 0, maxSpeed);
    }

    void Move()
    {
        Vector3 moveDir = transform.forward;
        moveDir.x += direction / steeringSensRed;
        moveDir.y = 0 - transform.position.y;
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("rock") && !destroyed)
        {
            destroyed = true;
            Debug.Log("HIT A ROCK");

            //End the game :)
            
            VoidController voidController = GameObject.FindObjectOfType<VoidController>();
            voidController.GameEnd();
        }
    }
}
