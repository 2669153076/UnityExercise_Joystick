using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick",CheckDirChange);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(dir, Space.World);
    }

    private void CheckDirChange(Vector2 dir)
    {
        this.dir.x = dir.x;
        this.dir.z = dir.y;
    }
}
