using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_controller : MonoBehaviour
{
    [Header("Looking")]
    [SerializeField] GameObject mPitchController = null;
    [SerializeField] [Range(0.1f, 3f)] float mSensitivity = 1.0f;
    [SerializeField] float mYawSpeed = 2.0f;
    [SerializeField] float mPitchSpeed = 2.0f;
    [SerializeField] [Range(-90, 0)] float mMinPitch = -90.0f;
    [SerializeField] [Range(0, 90f)] float mMaxPitch = 90.0f;
    private float mYaw = 0.0f;
    private float mPitch = 0.0f;

    [Header("Moving")]
    [SerializeField] CharacterController mCharacterController = null;
    [SerializeField] [Range(0f, 10f)] float mSpeed = 5.0f;
    [SerializeField] [Range(1f, 4f)] float mRunMultiplier = 2.0f;
    private float mVerticalSpeed = 0.0f;
    private bool mOnGround = false;
    private bool mContactAbove = false;
    private bool mSprinting = false;
    private Vector3 mMomentum = new Vector3();

    [Header("Jumping")]
    [SerializeField] float mJumpHeight = 2.0f;
    [SerializeField] float mHalfJump = 4.0f;
    [SerializeField] [Range(0f, 2f)] float mAirControl = 0.75f;
    [SerializeField] [Range(0f, 1f)] float mInertia = 0.5f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Move();
    }

    private void Rotate(float mouseAxisX, float mouseAxisY)
    {
        mYaw += mouseAxisX * mYawSpeed * mSensitivity;
        mPitch -= mouseAxisY * mPitchSpeed * mSensitivity;
        mPitch = Mathf.Clamp(mPitch, mMinPitch, mMaxPitch);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, mYaw, 0.0f));
        mPitchController.transform.localRotation = Quaternion.Euler(new Vector3(mPitch, 0.0f, 0.0f));
    }

    private void Move()
    {
        //speed math
        Vector3 lMovement = new Vector2();
        Vector3 forward = new Vector3(Mathf.Sin(mYaw * Mathf.Deg2Rad), 0.0f, Mathf.Cos(mYaw * Mathf.Deg2Rad));
        Vector3 right = new Vector3(Mathf.Sin((mYaw + 90.0f) * Mathf.Deg2Rad), 0.0f, Mathf.Cos((mYaw + 90.0f) * Mathf.Deg2Rad));
        //hard coded controls fuck you
        if (Input.GetKey(KeyCode.W)) lMovement += forward;
        else if (Input.GetKey(KeyCode.S)) lMovement -= forward;
        if (Input.GetKey(KeyCode.D)) lMovement += right;
        else if (Input.GetKey(KeyCode.A)) lMovement -= right;
        lMovement.Normalize();
        lMovement *= Time.deltaTime * mSpeed * (mSprinting ? mRunMultiplier : 1) * (mOnGround ? 1 : mAirControl);
        //momentum gets carried on jumps
        if (!mOnGround) lMovement = new Vector3(Mathf.SmoothStep(mMomentum.x, lMovement.x, mInertia), 0.0f, Mathf.SmoothStep(mMomentum.z, lMovement.z, mInertia));
        else mMomentum = lMovement;
        //gravity lol
        mVerticalSpeed -= Time.deltaTime * 2 * mJumpHeight * mSpeed * mRunMultiplier * mSpeed * mRunMultiplier / Mathf.Pow(mHalfJump, 2);
        lMovement.y += mVerticalSpeed * Time.deltaTime;

        CollisionFlags collisions = mCharacterController.Move(lMovement);

        mOnGround = (collisions & CollisionFlags.Below) != 0;
        if (mContactAbove && mVerticalSpeed > 0.0f) mVerticalSpeed = 0.0f;
        if (mOnGround)
        {
            mSprinting = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKey(KeyCode.Space)) mVerticalSpeed = 2 * mJumpHeight * mSpeed * mRunMultiplier / mHalfJump;
            else mVerticalSpeed = 0.0f;
        }
    }
}
