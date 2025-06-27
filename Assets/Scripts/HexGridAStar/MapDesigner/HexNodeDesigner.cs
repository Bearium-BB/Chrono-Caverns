using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HexNodeDesigner : MonoBehaviour
{
    public HexNode HexNode;

    public bool isWalkable = true;
    public bool isRamp;
    public bool isCliff;
    public bool isFlyOnly;
    public int heightLevel = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            HexNode.isWalkable = isWalkable;
            HexNode.isRamp = isRamp;
            HexNode.isCliff = isCliff;
            HexNode.isFlyOnly = isFlyOnly;
            HexNode.heightLevel = heightLevel;

            Debug.Log(HexNode.isWalkable);
            Debug.Log("Update");
        }
    }
}
