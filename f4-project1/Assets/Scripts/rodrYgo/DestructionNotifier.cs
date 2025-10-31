using UnityEngine;
using System;

public class DestructionNotifier : MonoBehaviour
{
    public Action onDestroyed;

    void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
