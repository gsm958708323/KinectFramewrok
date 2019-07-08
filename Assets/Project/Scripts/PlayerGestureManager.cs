using UnityEngine;
using System.Collections;
using Frame;

namespace KinectFrame
{
    public class PlayerGestureManager : MonoSingleton<PlayerGestureManager>
    {
        #region 固定的四肢关节数组
        //固定的四肢关节数组
        private KinectInterop.JointType[] HandRightJoints = new KinectInterop.JointType[]
        {
             KinectInterop.  JointType.ShoulderRight,//右肩
             KinectInterop.  JointType.ElbowRight,//右肘
             KinectInterop.  JointType.HandRight,//右手
        };
        private KinectInterop.JointType[] HandLeftJoints = new KinectInterop.JointType[]
        {
             KinectInterop.  JointType.ShoulderLeft,
            KinectInterop.   JointType.ElbowLeft,
              KinectInterop. JointType.HandLeft
        };
        private KinectInterop.JointType[] FootRightJoints = new KinectInterop.JointType[]
        {
              KinectInterop. JointType.HipRight,//右髋关节
              KinectInterop. JointType.KneeRight,//右膝盖
             KinectInterop.  JointType.FootRight,//右脚
        };
        private KinectInterop.JointType[] FootLeftJoints = new KinectInterop.JointType[]
        {
            KinectInterop.JointType.HipLeft,
              KinectInterop. JointType.KneeLeft,
               KinectInterop.JointType.FootLeft
        };
        #endregion

        #region 手势信息录入

        public void SetGestureJointData(PlayerGestureInfo.GestureType gestureType, PlayerGestureData playerGestureData)
        {
            SetGestureJointData(gestureType, playerGestureData, HandRightJoints);
            SetGestureJointData(gestureType, playerGestureData, HandLeftJoints);
            SetGestureJointData(gestureType, playerGestureData, FootRightJoints);
            SetGestureJointData(gestureType, playerGestureData, FootLeftJoints);
        }

        private void SetGestureJointData(PlayerGestureInfo.GestureType gestureType, PlayerGestureData playerGestureData, KinectInterop.JointType[] jointTypes)
        {
            for (int i = 0; i < jointTypes.Length - 1; i++)
            {
                Vector3 pos1 = KINECTManager.Instance.GetJointColorMapPos(0, jointTypes[i]);
                Vector3 pos2 = KINECTManager.Instance.GetJointColorMapPos(0, jointTypes[i + 1]);
                Vector3 dir = (pos2 - pos1).normalized;
                playerGestureData.playerGestureInfo.SetPlayerGestureJointData(gestureType, jointTypes[i], dir);
            }
        }
        #endregion
    }

}
