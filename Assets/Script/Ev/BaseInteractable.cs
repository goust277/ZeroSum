using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    [SerializeField] private Collider2D _collider2D;

    public abstract void Exe();
}
