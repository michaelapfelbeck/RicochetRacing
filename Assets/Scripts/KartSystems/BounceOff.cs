using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace KartGame.KartSystems
{
    [RequireComponent(typeof(ArcadeKart))]
    public class BounceOff : MonoBehaviour
    {

        [Tooltip("How much impulse should be applied to the kart when it collides?")]
        public float BounceFactor = 10f;
        [Tooltip("How fast should the kart reorient itself when colliding? The higher the value the more snappy it looks")]
        public float RotationSpeed = 2f;
        [Tooltip("What audio clip should play when the kart collides with a wall")]
        public AudioClip BounceSound;

        private ArcadeKart kart;
        private void Start()
        {
            kart = GetComponent<ArcadeKart>();
        }
        public void BounceOffCollision(BounceData data)
        {
            Vector3 origin = transform.position;
            Vector3 reflectionVector;
            origin.y += 0.4f;// HeightOffset;

            // Calculate the incident vector of the kart colliding into whatever object
            Vector3 incidentVector = data.point - origin;

            // Calculate the reflection vector using the incident vector of the collision
            Vector3 hitNormal = data.normal.normalized;
            reflectionVector = incidentVector - 2 * Vector3.Dot(incidentVector, hitNormal) * hitNormal;
            reflectionVector.y = 0;

            kart.Rigidbody.velocity /= 2f;
            // Apply the bounce impulse with the reflectionVector
            kart.Rigidbody.AddForce(reflectionVector.normalized * BounceFactor, ForceMode.Impulse);

            if (BounceSound)
            {
                AudioUtility.CreateSFX(BounceSound, transform.position, AudioUtility.AudioGroups.Collision, 0f);
            }
        }
    }
}