using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class Manager_location : MonoBehaviour
{
    public float latA, lonA;
    float lonB, latB, overallDistance, lastDistance, timer, lastTime, speed, speed0, acceleration;
    bool firstTime;

    void Awake()
    {
        overallDistance = 0;
        lastDistance = 0;
        timer = 0;
        lastTime = 0;
        speed = 0;
        speed0 = 0;

        firstTime = true;
    }

    public void re_start()
    {
        StartCoroutine(Start());
    }

    IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            this.GetComponent<App>().carrot.delay_function(5f,this.re_start);
            yield break;
        }
            

        // Start service before querying location
        Input.location.Start(1, 1);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            lonA = Input.location.lastData.longitude;
            latA = Input.location.lastData.latitude;
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    void Update()
    {
        if (this.GetComponent<App>().get_status_play()==1)
        {
            timer += Time.deltaTime;

            if (lonA != Input.location.lastData.longitude || latA != Input.location.lastData.latitude)
            {
                CalculateDistances(lonA, latA, Input.location.lastData.longitude, Input.location.lastData.latitude);
                lonA = Input.location.lastData.longitude;
                latA = Input.location.lastData.latitude;

                lastTime = timer;
                timer = 0;


                speed0 = speed;

                CalculateSpeed();
                CalculateAcceleration();
            }
        }

    }

    public static float Radians(float x)
    {
        return x * Mathf.PI / 180;
    }

    public void CalculateDistances(float firstLon, float firstLat, float secondLon, float secondLat)
    {

        float dlon = Radians(secondLon - firstLon);
        float dlat = Radians(secondLat - firstLat);

        float distance = Mathf.Pow(Mathf.Sin(dlat / 2), 2) + Mathf.Cos(Radians(firstLat)) * Mathf.Cos(Radians(secondLat)) * Mathf.Pow(Mathf.Sin(dlon / 2), 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(distance), Mathf.Sqrt(1 - distance));

        lastDistance = 6371 * c * 1000;

        overallDistance += lastDistance; 

        StartCoroutine(Overall());
    }

    IEnumerator Overall()
    {
        if (firstTime)
        {
            firstTime = false;

            yield return new WaitForSeconds(2);

            if (overallDistance > 6000000)
            {
                overallDistance = 0;
                lastDistance = 0;
            }
        }

        overallDistance += lastDistance;
        GameObject.Find("App").GetComponent<App>().show_digit_price_bill(overallDistance);
    }

    void CalculateSpeed()
    {
        speed = lastDistance / lastTime * 3.6f;
    }

    void CalculateAcceleration()
    {
        acceleration = (speed - speed0) / lastTime;
    }

    public void reset_location()
    {
        overallDistance = 0;
        lastDistance = 0;
        timer = 0;
        lastTime = 0;
        speed = 0;
        speed0 = 0;
    }

    public void show_map_locations()
    {
        Application.OpenURL("https://maps.google.com?q="+this.latA.ToString().Replace(",",".")+","+this.lonA.ToString().Replace(",", ".") + "");
    }
}
