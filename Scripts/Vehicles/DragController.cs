using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public int motorTorque = 100;

    public float vertical;
    public float horizontal;
    public bool handBrake;
    public float steeringMax = 20;
    public float brakePower = 90000;

    public float wheelsRPM;

    public float engineRPM;
    public float totalPower;

    public float maxRPM = 5600f;
    public float minRPM = 3000f;

    private bool flag = false;

    public float[] upgradePower = new float[5];



    public float KPH;

    public GameObject[] wheelMesh = new GameObject[4];

    public Rigidbody rigidbody;

    public float[] gears;
    public int gearNum = 1;

    public AnimationCurve enginePower;

    private float smoothTime = 0.01f;

    [HideInInspector] public bool test; //engine sound boolean

    public float lastValue;

    public inputManager IM;

    public bool automatic = false;

    private bool reverse = false;

    void Start()
    {
        if (this.gameObject.tag == "Player") {
            GameObject.Find("SpeedometerManager").GetComponent<SpeedometerManager>().carController = this;
            GameObject.Find("SpeedometerManager").GetComponent<SpeedometerManager>().initialized = true;
            GameObject.Find("GearHelper").GetComponent<GearHelper>().carController = this;
            GameObject.Find("GearHelper").GetComponent<GearHelper>().initialized = true;
        }
    }

    private void Update()
    {
        vertical = IM.vertical;
        horizontal = IM.horizontal;
        handBrake = IM.handbrake;

        AnimateWheels();

        lastValue = engineRPM;

        CalculateEnginePower();

    }

    private void MoveVehicle()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque = totalPower / 4;
        }

        for (int i = 0; i < wheels.Length - 2; i++)
        {
            wheels[i].steerAngle = horizontal * steeringMax;
        }

        KPH = rigidbody.velocity.magnitude * 3.6f;

        if (handBrake)
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = brakePower;
        }
        else
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = 0;
        }
    }

    private void AnimateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    private void CalculateEnginePower()
    {
        WheelRPM();

        if (vertical != 0)
        {
            rigidbody.drag = 0.005f;
        }
        if (vertical == 0)
        {
            rigidbody.drag = 0.1f;
        }
        totalPower = 2f * enginePower.Evaluate(engineRPM) * (vertical) * (this.gameObject.tag != "AI" ? upgradePower[PlayerPrefs.GetInt("carHP", 1) - 1] : 1.2f);

        float velocity = 0.0f;
        if (engineRPM >= maxRPM || flag)
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM - 500, ref velocity, 0.05f);

            flag = (engineRPM >= maxRPM - 450) ? true : false;

            test = (lastValue > engineRPM) ? true : false;
        }
        else
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum - 1])), ref velocity, smoothTime);
            test = false;
        }
        if (engineRPM >= maxRPM + 1000) engineRPM = maxRPM + 1000; // clamp at max
        MoveVehicle();
        Shifter();
    }
    
    private void WheelRPM()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;

        if(wheelsRPM < 0 && !reverse)
        {
            reverse = true;
        }
        else if(wheelsRPM > 0 && reverse)
        {
            reverse = false;
        }
    }

    private void Shifter()
    {
        // if gearbox is automatic the logic is different
        if (automatic)
        {
            if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse)
            {
                gearNum++;
                return;
            }
            if (engineRPM < minRPM && gearNum > 1)
            {
                gearNum--;
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && gearNum != gears.Length)
            {
                gearNum++;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && gearNum != 1)
            {
                gearNum--;
            }
        }
    }
}
