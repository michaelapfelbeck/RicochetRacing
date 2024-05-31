using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KartGame.KartSystems
{
    public struct BounceData
    {
        // world point
        public Vector3 point;
        // normal of the surface hit
        public Vector3 normal;
    }

    /// <summary>
    /// Creates bounce collisions that other systems react to.
    /// </summary>
    [RequireComponent(typeof(ArcadeKart))]
    public class BounceProvider : MonoBehaviour
    {
        /// <summary>
        /// Represents a single frame where the bounce was actually triggered. Use this as a flag 
        /// to detect when bouncing was invoked. This flag is managed by the VehicleBounce.
        /// </summary>
        public bool BounceFlag { get; private set; }

        [Tooltip("What layer should this GameObject collider with?")]
        public LayerMask CollisionLayer;
        [Tooltip("How far ahead should the vehicle detect a bounce? This should be a positive value!")]
        public float RayDistance = 1f;
        [Tooltip("When can the kart bounce again if it bounced once?")]
        public float CoolOffTime = 0.5f;
        [Tooltip("Do we need to adjust the y height of the ray's origin to compensate for larger vehicles?")]
        public float HeightOffset;
        [Tooltip("How many raycasts do we intend to shoot out to detect the bounce and at what angles are we doing so? " +
            "Enable DrawGizmos to help you debug what the ray casts look like when selecting angles.")]
        public float[] Angles;

        [Tooltip("Should gizmos be drawn for debugging purposes? This is helpful for checking the rays.")]
        public bool DrawGizmos;

        ArcadeKart kart;
        float resumeTime;
        bool hasCollided;

        public UnityEvent<BounceData> OnBounce;

        void Start()
        {
            kart = GetComponent<ArcadeKart>();
        }

        void Update()
        {
            // Reset the trigger flag
            if (BounceFlag)
            {
                BounceFlag = false;
            }
            Vector3 origin = transform.position;
            origin.y += HeightOffset;

            for (int i = 0; i < Angles.Length; i++)
            {
                Vector3 direction = GetDirectionFromAngle(Angles[i], Vector3.up, transform.forward);

                if (Physics.Raycast(origin, direction, out RaycastHit hit, RayDistance, CollisionLayer) && Time.time > resumeTime && !hasCollided && kart.LocalSpeed() > 0)
                {
                    // If the hit normal is pointing up, then we don't want to bounce
                    if (Vector3.Dot(hit.normal, Vector3.up) > 0.2f)
                    {
                        return;
                    }

                        BounceData data = new BounceData { 
                            normal = hit.normal,
                            point = hit.point
                        };

                    resumeTime = Time.time + CoolOffTime;

                    OnBounce.Invoke(data);

                    return;
                }
            }
        }

        void LateUpdate()
        {
            if (Time.time > resumeTime && hasCollided)
            {
                hasCollided = false;
            }
        }

        void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Vector3 origin = transform.position;
                origin.y += HeightOffset;

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(origin, origin + transform.forward);
                Gizmos.color = Color.red;
                for (int i = 0; i < Angles.Length; i++)
                {
                    var direction = GetDirectionFromAngle(Angles[i], Vector3.up, transform.forward);
                    Gizmos.DrawLine(origin, origin + (direction.normalized * RayDistance));
                }
            }
        }

        Vector3 GetDirectionFromAngle(float degrees, Vector3 axis, Vector3 zerothDirection)
        {
            Quaternion rotation = Quaternion.AngleAxis(degrees, axis);
            return (rotation * zerothDirection);
        }
    }
}

