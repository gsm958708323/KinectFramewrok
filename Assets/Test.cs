using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinectFrame;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public RawImage image;

    private PlayerGestureData gestureData;
    // Start is called before the first frame update
    void Start()
    {
        gestureData = GetComponent<PlayerGestureData>();
        PlayerGestureManager.Instance.SetGestureJointData(PlayerGestureInfo.GestureType.Start, gestureData);
    }

    // Update is called once per frame
    void Update()
    {
        image.texture = KINECTManager.Instance.GetLiveImage();

        if (Input.GetMouseButtonDown(0))
        {
            PlayerGestureManager.Instance.SetGestureJointData(PlayerGestureInfo.GestureType.Start, gestureData);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            PlayerGestureManager.Instance.SetGestureJointData(PlayerGestureInfo.GestureType.End, gestureData);
        }
    }
}
