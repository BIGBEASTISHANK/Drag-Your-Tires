using TMPro;
using TigerForge;
using UnityEngine;
using System.Collections;

public class CarControll : MonoBehaviour
{
    // Variables
    /////////////
    private int sec;
    private int min;
    private int hrs;
    private int Tsec;
    private int Tmin;
    private int Thrs;

    private float speed;
    private float motorForce;
    private float verticalInput;
    private float horizontalInput;
    private float currentSteerAngle;
    private float currentbreakForce;

    private bool isBreaking;

    private InputSys inpSys;
    private Vector2 moveInput;
    private EasyFileSave fileSave;

    [Header("Values")]
    [SerializeField] private float motorSpeed;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float boostMotorSpeed;

    [Header("Components")]
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [Space]
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [Space]
    [SerializeField] private TMP_Text speedTxt;
    [SerializeField] private TMP_Text totalTimerTxt;
    [SerializeField] private TMP_Text currentTimerTxt;
    [Space]
    [SerializeField] private ParticleSystem[] exhauseParticle;

    // References
    //////////////
    private void Awake() { inpSys = new InputSys(); }

    private void Start()
    {
        // Initializing 
        fileSave = new EasyFileSave("game_timer");

        // Load times
        if (fileSave.Load())
        {
            Tsec = fileSave.GetInt("Tsec");
            Tmin = fileSave.GetInt("Tmin");
            Thrs = fileSave.GetInt("Thrs");

            fileSave.Dispose();
        }
    }

    private void FixedUpdate()
    {
        // Functions
        Boost();
        GetInput();
        TotalTimer();
        HandleMotor();
        UnRotateCar();
        UpdateWheels();
        CurrentTimer();
        HandleSteering();

        StartCoroutine(nameof(CalculateSpeed));
    }

    // Boost function
    private void Boost()
    {
        // Multiplying the moterForce, 2.5 times for the boost
        if (inpSys.Player.Boost.IsPressed())
        { motorForce = boostMotorSpeed; exhauseParticle[0].Play(); exhauseParticle[1].Play(); }
        else
        { motorForce = motorSpeed; exhauseParticle[0].Stop(); exhauseParticle[1].Stop(); }
    }

    // CalculateSpeed function
    IEnumerator CalculateSpeed()
    {
        Vector3 lastPosition = transform.position;
        yield return new WaitForFixedUpdate();
        speed = (lastPosition - transform.position).magnitude / Time.fixedDeltaTime;

        speedTxt.text = speed.ToString("00"); // Showing speed text
    }

    // Show total time
    private void TotalTimer()
    { if (fileSave.Load()) { totalTimerTxt.text = Thrs.ToString("00") + ":" + Tmin.ToString("00") + ":" + Tsec.ToString("00"); fileSave.Dispose(); } }

    // Show current time
    private void CurrentTimer()
    { currentTimerTxt.text = hrs.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00"); Invoke(nameof(IncTimer), 1); }

    // IncTimer function
    private void IncTimer()
    {
        // Increasing Seconds
        sec++; Tsec++;

        // Setting hrs and minute according to seconds
        if (sec >= 60) { min++; sec = 0; }
        if (min >= 60) { hrs++; min = 0; }

        // Setting total time
        if (Tsec >= 60) { Tmin++; Tsec = 0; }
        if (Tmin >= 60) { Thrs++; Tmin = 0; }

        // Saving time
        fileSave.Add("Tsec", Tsec);
        fileSave.Add("Tmin", Tmin);
        fileSave.Add("Thrs", Thrs);
        fileSave.Save();

        CancelInvoke(nameof(IncTimer));
    }

    // Unflip car when it is fliped
    private void UnRotateCar() { if (inpSys.Player.UnRotateCar.IsPressed()) { transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0); } }

    // Getting input
    private void GetInput()
    {
        // Move Input
        moveInput = inpSys.Player.Movement.ReadValue<Vector2>();
        verticalInput = moveInput.y; horizontalInput = moveInput.x;

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    // HandleMotor function
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    // ApplyBreaking function
    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    // HandleSteering function
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    // Wheel transform turning
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);

        // UpdateSingleWheel functions
        void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }
    }

    // When GameObject get enabled
    private void OnEnable() { inpSys.Player.Enable(); }
    // When GameObject get disables
    private void onDisable() { inpSys.Player.Disable(); }
}