using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Security.Cryptography.ECCurve;

public class EyeOnGunAimer : MonoBehaviour
{
    #region Members
    LineRenderer trajectoryLine;
    [SerializeField, Tooltip("The marker will show where the projectile will hit")]
    Transform hitMarker;
    [SerializeField, Range(10, 100), Tooltip("The maximum number of points the LineRenderer can have")]
    int maxPoints = 50;
    [SerializeField, Range(0.01f, 0.5f), Tooltip("The time increment used to calculate the trajectory")]
    float increment = 0.025f;
    [SerializeField, Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long")]
    float rayOverlap = 1.1f;

    public float force;

    [SerializeField]
    Transform StartPosition;

    HVRSocket mySocket;
    public bool isAiming;

    public GameObject eyePrefab;
    #endregion

    private void Start()
    {
        mySocket = GetComponent<HVRSocket>();
        if (trajectoryLine == null)
            trajectoryLine = GetComponent<LineRenderer>();

        if (StartPosition == null)
            StartPosition = transform;

        //SetTrajectoryVisible(false);
    }

    private void Update()
    {
        if (isAiming)
        {
            PredictTrajectory(ProjectileData());
        }
    }

    public void PredictTrajectory(ProjectileProperties projectile)
    {
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass);
        Vector3 position = projectile.initialPosition;
        Vector3 nextPosition;
        float overlap;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++)
        {
            // Estimate velocity and update next predicted position
            velocity = CalculateNewVelocity(velocity, projectile.drag, increment);
            nextPosition = position + velocity * increment;

            // Overlap our rays by small margin to ensure we never miss a surface
            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            //When hitting a surface we want to show the surface marker and stop updating our line
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap))
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                break;
            }

            //If nothing is hit, continue rendering the arc without a visual marker
            hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position)); //Unneccesary to set count here, but not harmful
        }
    }
    /// <summary>
    /// Allows us to set line count and an induvidual position at the same time
    /// </summary>
    /// <param name="count">Number of points in our line</param>
    /// <param name="pointPos">The position of an induvidual point</param>
    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment)
    {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void MoveHitMarker(RaycastHit hit)
    {
        hitMarker.gameObject.SetActive(true);

        // Offset marker from surface
        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible)
    {
        if (GameManager.Instance.showAimTrajectory.isOn)
        {
            isAiming = visible;
            trajectoryLine.enabled = visible;
            hitMarker.gameObject.SetActive(visible);
        }
        else
        {
            ForceVisibleOff();
        }
        
    }

    public void ForceVisibleOff()
    {
        isAiming = false;
        hitMarker.gameObject.SetActive(false);
        trajectoryLine.enabled = false;
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = eyePrefab.GetComponent<Rigidbody>();

        properties.direction = StartPosition.forward;
        properties.initialPosition = StartPosition.position;
        properties.initialSpeed = force;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }
}
