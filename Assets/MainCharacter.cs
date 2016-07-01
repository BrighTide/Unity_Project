using UnityEngine;
using System.Collections;

public class MainCharacter : MonoBehaviour {

    private Rigidbody2D rigidBody;
    public float movmentForce = 1;
    public float inAirFactor = 1;
    private float lastJumpTime = -10000;
    public float jumpForce = 1;
    public RaycastHit2D lastRayHit;
    public Collision2D lastCollision;
    private bool jumping = false;
    public float maxSpeed;
    public AnimationCurve acceleration;

    // Use this for initialization
    void Start () {
        Debug.Log(this);
        rigidBody = GetComponent<Rigidbody2D>();


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Runs ever physics update
    void FixedUpdate ()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        if (lastCollision == null)
        {
            rigidBody.AddForce(input.normalized * movmentForce * inAirFactor); // Air Movment
        }
        else
        {
            Vector2 relativeVelocity;
//            if (lastCollision.rigidbody)
  //          {
    //            relativeVelocity = rigidBody.velocity - lastCollision.rigidbody.velocity;
      //      }
        //    else
          //  {
                relativeVelocity = rigidBody.velocity;

            //}

            if (input.x != 0)
            {
                float endpoint = input.x * maxSpeed;                    // -20 or 20
                float difference = endpoint - relativeVelocity.x;       // (-20) - current vel

                //var factor = acceleration.Evaluate(difference.magnitude);

                if (difference > maxSpeed)
                {
                    difference = maxSpeed;
                }

                float velicytToApply = difference / maxSpeed;
                rigidBody.AddForce(new Vector2(velicytToApply * 1000f, 0));

                //Debug.Log(factor);
                //Debug.DrawLine(transform.position, transform.position + (Vector3)input, Color.black);
            }
        }
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8, so we just inverse the mask.
        layerMask = ~layerMask;

        //bool imminentCollision = Physics2D.Raycast((Vector2)transform.position, rigidBody.velocity.normalized, 2.0f, layerMask).collider;

        if (Input.GetKey(KeyCode.Space))
        {
            if (lastCollision != null)
            {
                Jump();
            }
            else
            {
                lastJumpTime = Time.time;
            }
        }

       // Debug.DrawRay((Vector2)transform.position, rigidBody.velocity.normalized * 2, imminentCollision ? Color.red : Color.white);
    }
 
    void Jump()
    {
        if (!jumping)
        {
            rigidBody.AddForce(Vector3.Slerp(lastCollision.contacts[0].normal, Vector3.up, 0.5f) * jumpForce);
            jumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        jumping = false;
        lastCollision = coll;
        if (Time.time - lastJumpTime < 0.100)
        {
            Jump();
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        lastCollision = coll;
    }

    void OnCollisionExit2D()
    {
        lastCollision = null;
    }
}
