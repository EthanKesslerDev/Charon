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
    float acceleration;
    public float steeringSensRed = 75f;
    public float rotationSpeed = 0.1f;

    bool destroyed = false;

    //Variables for fuel
    [Header("Fuel")]
    public Light furnaceLight;
    float maxLight;
    float currentIntensity;
    float lightLerp = 0f;
    public float consumptionInterval = 16f;
    float nextConsumptionTime;
    public GameObject barrelEffect;
    bool outOfFuel = false;


    void Start()
    {
        smoke.Play();
        maxLight = furnaceLight.intensity;
        nextConsumptionTime = consumptionInterval;
        acceleration = accelPerSecond;
    }

    void FixedUpdate()
    {
        Moments();
        Move();
        UpdateRotation();
        standardSpeed += acceleration * Time.time; // Acceleration
        standardSpeed = Mathf.Clamp(standardSpeed, 0, maxSpeed); //Came back to this a day later and Im so proud of my past self because this has helped so much
        if(standardSpeed <= 1f){
            destroyed = true;
            VoidController voidController = GameObject.FindObjectOfType<VoidController>();
            voidController.GameEnd();
        }
        Fuel();
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
        targetZ = Mathf.Clamp(targetZ, -40f, 40f); //This might mess with things :/
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

    void Fuel()
    {
        if(Time.time > nextConsumptionTime)
        {
            //Now we do what we want to with the fuel
            Debug.Log("Knock knock, its the tax man and he's come for your barrels.");
            //Select a random barrel
            if(weights.Count > 0)
            {
                outOfFuel = false;
                int barrelIndex = Random.Range(0, weights.Count);
                if(weights[barrelIndex])
                {
                    GameObject thisFuel = weights[barrelIndex];
                    weights.Remove(weights[barrelIndex]);
                    thisFuel.SetActive(false);
                    GameObject effect = GameObject.Instantiate(barrelEffect, thisFuel.transform.position, Random.rotation);
                    Destroy(thisFuel, 1f);
                    Destroy(effect, 1.6f);
                    //Thats all done for the barrel

                    nextConsumptionTime += consumptionInterval;
                    currentIntensity = maxLight;
                    Debug.Log("RESET FURNACE, Destroyed a barrel ");
                    lightLerp = 0;
                    acceleration = accelPerSecond;
                }
            } else {
                outOfFuel = true;
                acceleration = -0.0002f; //Slowing down the boat
            }

            //Some kind of SFX?
        }
        //Interpolating between light intensity :)
        lightLerp += Time.deltaTime / consumptionInterval;
        currentIntensity = Mathf.Lerp(maxLight, 0, lightLerp);
        currentIntensity = Mathf.Clamp(currentIntensity, 0 , maxLight);
        furnaceLight.intensity = currentIntensity;
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
