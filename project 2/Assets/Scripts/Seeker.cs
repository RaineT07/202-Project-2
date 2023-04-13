using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Seeker : Agent
{
    [SerializeField]
    PhysicsObject targetObj;

    [SerializeField]
    float seekScaler = 5f;
    public override void CalculateSteeringForces(){
        //seek a target
        totalForces += Seek(targetObj.transform.position) * seekScaler;
        PhysicsObject thisObj = GetComponent<PhysicsObject>();
        Bounds seekBounds = new Bounds(transform.position, new Vector3(thisObj.radius, thisObj.radius, 0));
        Bounds fleeBounds = new Bounds(targetObj.transform.position, new Vector3(thisObj.radius, thisObj.radius, 0));
        if(Math.Sqrt( Math.Pow(seekBounds.center.x-fleeBounds.center.x,2) + Math.Pow(seekBounds.center.y - fleeBounds.center.y, 2)) < (thisObj.radius + targetObj.radius)){
            Vector3 cameraMax = Camera.main.ScreenToWorldPoint(new Vector3((float)Camera.main.pixelWidth,(float)Camera.main.pixelHeight,0));
            //this absolutely insane line of code just programs a random location for the flee-er
            targetObj.transform.position = new Vector3(UnityEngine.Random.Range(cameraMax.x*-1f, cameraMax.x), UnityEngine.Random.Range(cameraMax.y*-1f, cameraMax.y),0);
        }
    }
}
