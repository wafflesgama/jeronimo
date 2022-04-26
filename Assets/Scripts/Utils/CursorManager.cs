using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    public Texture2D defaultCursor;
    [SerializeField]
    public Texture2D clickCursor;
    public Vector2 hotspot;

    bool showing;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (!showing) return;

        if (Input.anyKey)
        {
            Cursor.SetCursor(clickCursor, hotspot, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.ForceSoftware);
        }

    }

    public void ShowCursor()
    {
        showing = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        showing = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
