using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GiftReceiver : MonoBehaviour
    {
        [SerializeField]private Vector3 offset = Vector3.zero;

        public Vector3 ReceivePosition { get => transform.position + offset; }
    }
}
