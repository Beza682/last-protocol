using System;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public struct PlayerComponent
{
    public GameObject Player;
    public Transform PlayerTransform;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public float Speed;
}