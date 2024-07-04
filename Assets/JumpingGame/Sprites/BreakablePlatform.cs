using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : PlatForm
{
    public override void PlatformAction()
    {
        Destroy(gameObject);
    }
}
