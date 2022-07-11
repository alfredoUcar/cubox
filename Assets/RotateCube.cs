using System;
using Unity.VisualScripting;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private const float ROTATION_TIME_SECONDS = 1.5f;
    private const float ROTATION_SPEED = 250f;
    private Vector2 firstPress;
    private Vector2 secondPress;
    private Vector2 swipe;

    private Vector3 left;

    public GameObject cubeCenter;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        left = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != target.transform.rotation)
        {
            // continue rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, ROTATION_SPEED * Time.deltaTime);
        } else {
            // listen rotation events
            HandleSwipe();
            HandleArrowKeys();
        }
    }

    private void HandleSwipe()
    {
        if (Input.GetMouseButtonDown(((int)MouseButton.Left)))
        {
            firstPress = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(((int)MouseButton.Left)))
        {
            secondPress = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            swipe = (secondPress - firstPress).normalized;

            if (IsLeftSwipe(swipe))
            {
                target.transform.Rotate(Vector3.up * 90, Space.World);
            }
            else if (IsRightSwipe(swipe))
            {
                target.transform.Rotate(Vector3.down * 90, Space.World);
            }
            else if (IsUpSwipe(swipe))
            {
                target.transform.Rotate(Vector3.right * 90, Space.World);
            }
            else if (IsDownSwipe(swipe))
            {
                target.transform.Rotate(Vector3.left * 90, Space.World);
            }
        }
    }

    private void HandleArrowKeys()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            target.transform.Rotate(Vector3.up * 90, Space.World);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            target.transform.Rotate(Vector3.down * 90, Space.World);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            target.transform.Rotate(Vector3.right * 90, Space.World);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            target.transform.Rotate(Vector3.left * 90, Space.World);
        }
    }

    private bool IsLeftSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && Math.Abs(swipe.y) < 0.5f;
    }

    private bool IsRightSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && Math.Abs(swipe.y) < 0.5f;
    }

    private bool IsUpSwipe(Vector2 swipe)
    {
        return swipe.y > 0 && Math.Abs(swipe.x) < 0.5f;
    }

    private bool IsDownSwipe(Vector2 swipe)
    {
        return swipe.y < 0 && Math.Abs(swipe.x) < 0.5f;
    }
}
