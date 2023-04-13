using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsObject : MonoBehaviour
{

    Vector3 position;
    public Vector3 velocity = Vector3.zero;
    Vector3 direction = Vector3.zero;

    Vector3 acceleration = Vector3.zero;

    [SerializeField]
    float mass = 1f;

    public bool useGravity, useFriction;
    [SerializeField]
    Vector3 gravity = Vector3.down;
    [SerializeField]
    float coeff = 0.2f;

    [SerializeField]
    public float radius;

    public Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 mouseForce = calcMouse();
        // ApplyForce(mouseForce);

        if(useGravity){ApplyGravity(gravity);}
        if(useFriction){ApplyFriction(coeff);}
        
        velocity += acceleration * Time.deltaTime;

        position += velocity * Time.deltaTime;

        direction = velocity;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity);


        // CheckForBounce();

        transform.position = position;

        acceleration = Vector3.zero;



    }

    public void ApplyForce(Vector3 force){
        acceleration += force / mass;
    }

    void ApplyFriction(float coeff){
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        ApplyForce(friction);

    }
    
    void CheckForBounce(){
        Vector3 cameraMax = Camera.main.ScreenToWorldPoint(new Vector3((float)Camera.main.pixelWidth,(float)Camera.main.pixelHeight,0));
        if(transform.position.x > cameraMax.x-radius/2){
            velocity.x *= -1f;
            position.x = cameraMax.x-radius/2;
        } 
        if(transform.position.x < (cameraMax.x*-1f)+radius/2){
           velocity.x *=-1f;
           position.x = cameraMax.x * -1f + radius/2;
        }
        if(transform.position.y > cameraMax.y-radius/2){
            velocity.y *=-1f;
            position.y = cameraMax.y - radius/2;
        } 
        if(transform.position.y < (cameraMax.y*-1f)+radius/2){
            velocity.y *=-1f;
            position.y = cameraMax.y*-1f + radius/2;
        }
    }

    void ApplyGravity(Vector3 force){
        acceleration += force;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
