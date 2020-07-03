using UnityEngine;

public class Projectile : MonoBehaviour
{

    // The default Position
    Vector2 startPos;

    public GameObject Arrow;
    public float MiminimumAngle;
    public float MaxAngle;
    private Animator _anim;

    private bool _isColliding;

    Vector2 difference = Vector2.zero;
    float distance;
    bool pressed, shoot, firstpoint,flying;
    public Vector2 inputPoint;
    public Vector2 lastPoint = Vector2.zero;
    int maxDistance;
    int maxForce;
    // Use this for initialization
    void Start()
    {
        maxDistance = GameManager.Instance.MaxDistance;
        maxForce = GameManager.Instance.MaxForce;
        Arrow.SetActive(false);
        _isColliding = false;
        startPos = transform.position;
        _anim = GameObject.FindGameObjectWithTag("Arm").GetComponent<Animator>();
    }

    private void Update()
    {
        if (!flying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pressed = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                pressed = false;
                shoot = true;
            }

            if (pressed && !flying)
            {
                _pointToTrajectory();
            }

            if (shoot)
            {
                _shootApp();
            }
        }

    }

    private void _pointToTrajectory()
    {
        if(lastPoint != Vector2.zero)
        {
            Arrow.SetActive(true);
        }


        //Get the first inputPoint
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!firstpoint)
        {
            firstpoint = true;
            inputPoint = p;
        }

        //Calculate the vector
        difference = (p - new Vector2(inputPoint.x, inputPoint.y)).normalized;

        _activateDistancePointers();

        // Calculate the angle of the input
        float angleRotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (difference.x > 0 && difference.y > 0)
        {
            //Quadrant 1
            if (angleRotation >= 90)
            {
                angleRotation = 180 - angleRotation;
            }
            else if (angleRotation >= 180)
            {
                angleRotation = 270 - angleRotation;
            }
            else if (angleRotation >= 270)
            {
                angleRotation = 360 - angleRotation;
            }

            //Guard to check to not allow the user for strange angles.
            if (angleRotation > MiminimumAngle && angleRotation < MaxAngle)
            {
                Arrow.transform.rotation = Quaternion.Euler(0f, 0f, angleRotation - 45);
            }
            else
            {
                if (angleRotation <= MiminimumAngle)
                {
                    Arrow.transform.rotation = Quaternion.Euler(0f, 0f, MiminimumAngle - 45);
                }
                else
                {
                    Arrow.transform.rotation = Quaternion.Euler(0f, 0f, MaxAngle - 45);
                }
            }
            lastPoint = p;
        }
        else if (difference.x < 0 && difference.y < 0)
        {
            //Quadrant 3
            if (angleRotation > 0)
            {
                angleRotation = -angleRotation;
            }
            if (angleRotation > -90)
            {
                angleRotation -= 90;
            }

            //Debug.Log(" Fixed angle " + angleRotation);

            //Convert rotation to 1st Quadrant
            angleRotation += 180;
            Arrow.transform.rotation = Quaternion.Euler(0f, 0f, angleRotation - 45);

            lastPoint = p;

            difference = new Vector2(Mathf.Abs(difference.x), Mathf.Abs(difference.y));
        }
    }

    private void _shootApp()
    {
        if(!flying)
            _anim.SetTrigger("shoot");

        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().AddForce(difference * _forceCalculator(difference));
        Arrow.SetActive(false);
        firstpoint = false;
        inputPoint = Vector2.zero;
        shoot = false;
        flying = true;
    }


    float _forceCalculator(Vector2 dir)
    {
        if (distance > maxForce)
        {
            return maxForce;
        }
        else
        {
            return (maxForce * distance) / maxDistance;
        }
    }

    void _activateDistancePointers()
    {
        distance = Vector2.Distance(inputPoint, lastPoint);

        if (distance > maxDistance)
            distance = maxDistance;

        int value = (int)((distance * MaxTrajectoryNodes()) / (float)maxDistance + 0.5f);

        //print("Value " + value);


        for (int i = 0 ; i < Arrow.transform.childCount; i++)
        {
            Arrow.transform.GetChild(i).gameObject.SetActive(false);

        }

        for (int i = 0; i< value ; i++)
        {
            Arrow.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ring"))
            //Debug.Log("============================================== Fiz trigger ==============================================");
            GameManager.Instance.WentThroughRing(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (_isColliding)
            return;

        if (coll.collider.gameObject.tag == "Obstacle")
            GameManager.Instance.OnTouchedBarrier(coll.collider.gameObject);

        _isColliding = true;

        if (!IsInvoking("NotifyEndTurn"))
        {
            Invoke("NotifyEndTurn", 1.5f);
        }
    }


    void NotifyEndTurn()
    {

        GameManager.Instance.LostAttempt(); //needs to be called before call to GM
        GameManager.Instance.EndTurn();
        flying = false;
        Destroy(this.gameObject);
    }

    int MaxTrajectoryNodes() {
        if (GameManager.Instance.HasPurchasedFullTrajectory()) {
            return Arrow.transform.childCount;
        }
        return 6;
    }
}
