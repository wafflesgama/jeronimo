using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public abstract class DragArea : MonoBehaviour
{
    public RectTransform rect { get; private set; }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public abstract void OnDrag(Transform rectTransform);
 
}
