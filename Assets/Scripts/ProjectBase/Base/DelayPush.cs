using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPush : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Push", 1);
    }

    void Push()
    {
        PoolMgr.GetInstance().PushObj(this.gameObject.name,this.gameObject);
    }
}
