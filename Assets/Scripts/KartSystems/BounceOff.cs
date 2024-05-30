using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public void BounceOffCollision()
        {
            Debug.Log("BounceOffCollision");
        }
    }
}