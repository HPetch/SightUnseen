using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TwoPointsLine : MonoBehaviour
{
    //Credit to https://youtu.be/xyoswWxj_6o
    public Transform pointA;
    public Transform pointB;
    private LineRenderer line;

    //Used for contexts that change between teleporter movement and smooth movement
    public bool hasTeleporterAlternate = false;
    public TwoPointsLine teleporterAlternate;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.positionCount = 2;
        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);
    }
}
