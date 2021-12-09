using System;
using UnityEngine;

[RequireComponent(typeof(PlaneCollider))]
    public class GlobalPlane : MonoBehaviour
    {
        public PlaneCollider PlaneCollider { get; private set; }
        private void Awake() => PlaneCollider = GetComponent<PlaneCollider>();
    }