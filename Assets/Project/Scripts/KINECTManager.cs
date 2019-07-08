using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frame;

namespace KinectFrame
{
    public class KINECTManager : Singleton<KINECTManager>
    {
        #region SDK提供的脚本
        private KinectManager kinectManager;
        private BackgroundRemovalManager backgroundRemovalManager;
        #endregion

        private KINECTManager()
        {
            kinectManager = new GameObject("[KinectManager]").AddComponent<KinectManager>();
            kinectManager.gameObject.AddComponent<PortraitBackground>();

            backgroundRemovalManager = kinectManager.gameObject.AddComponent<BackgroundRemovalManager>();
        }

        #region 获取图像
        /// <summary>
        /// 获取实时图像
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Texture2D GetLiveImage(KinectImageType type)
        {
            if (kinectManager == null)
            {
                Debug.LogError("KinectManager实例化失败！");
                return null;
            }
            switch (type)
            {
                case KinectImageType.colour:
                    if (kinectManager.IsInitialized())
                        return kinectManager.GetUsersClrTex();
                    break;
                case KinectImageType.depthmask:
                    if (kinectManager.IsInitialized())
                        return kinectManager.GetUsersLblTex();
                    break;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// 获取前景(人像抠图)实时图像
        /// </summary>
        /// <returns></returns>
        public Texture GetLiveImage()
        {
            return backgroundRemovalManager.GetForegroundTex();
        }
        #endregion

        #region 获取关节位置
        /// <summary>
        /// 根据用户ID，获取关节位置
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetJointKinectPosition(long UserID, KinectInterop.JointType jointType)
        {
            if (!kinectManager.IsInitialized())
                return Vector3.zero;
            if (!kinectManager.IsUserDetected())
                return Vector3.zero;

            return kinectManager.GetJointKinectPosition(UserID, (int)jointType);
        }

        /// <summary>
        /// 根据用户索引，获取关节位置
        /// </summary>
        /// <param name="UserIndex"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetJointKinectPosition(int UserIndex, KinectInterop.JointType jointType)
        {
            if (!kinectManager.IsInitialized())
                return Vector3.zero;
            if (!kinectManager.IsUserDetected())
                return Vector3.zero;

            long userID = kinectManager.GetUserIdByIndex(UserIndex);
            return kinectManager.GetJointKinectPosition(userID, (int)jointType);
        }

        /// <summary>
        /// 根据用户索引,获取颜色贴图纹理上的关节位置。
        /// </summary>
        /// <param name="UserIndex"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetJointColorMapPos(int UserIndex, KinectInterop.JointType jointType)
        {
            if (!kinectManager.IsInitialized())
                return Vector3.zero;
            if (!kinectManager.IsUserDetected())
                return Vector3.zero;

            long userID = kinectManager.GetUserIdByIndex(UserIndex);
            Vector3 pos = GetJointColorMapPos(userID, jointType);
            return pos;
        }

        /// <summary>
        /// 根据用户ID，获取颜色贴图纹理上的关节位置。
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetJointColorMapPos(long UserID, KinectInterop.JointType jointType)
        {
            if (!kinectManager.IsInitialized())
                return Vector3.zero;
            if (!kinectManager.IsUserDetected())
                return Vector3.zero;
            Vector3 pos = kinectManager.GetJointColorMapPos(UserID, (int)jointType);
            return pos;
        }

        /// <summary>
        /// 根据用户索引，获取给定关节在彩色图像上的3d叠加位置。
        /// </summary>
        /// <param name="userIndex"></param>
        /// <param name="jointType"></param>
        /// <param name="camera"></param>
        /// <param name="offsetRect"></param>
        /// <returns></returns>
        public Vector3 GetJointPosColorOverlay(int userIndex, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            if (!kinectManager.IsInitialized())
                return Vector3.zero;
            if (!kinectManager.IsUserDetected())
                return Vector3.zero;
            long userID = kinectManager.GetUserIdByIndex(userIndex);
            Vector3 pos = kinectManager.GetJointPosColorOverlay(userID, (int)jointType, camera, offsetRect);
            return pos;
        }
        /// <summary>
        /// 根据用户ID，获取给定关节在彩色图像上的3d叠加位置。
        /// </summary>
        /// <param name="userIndex"></param>
        /// <param name="jointType"></param>
        /// <param name="camera"></param>
        /// <param name="offsetRect"></param>
        /// <returns></returns>
        public Vector3 GetJointPosColorOverlay(long userID, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            if (!kinectManager.IsInitialized())
                return Vector3.zero;
            if (!kinectManager.IsUserDetected())
                return Vector3.zero;
            Vector3 pos = kinectManager.GetJointPosColorOverlay(userID, (int)jointType, camera, offsetRect);
            return pos;
        }
        //-------------------------线上是3D关节位置，线下是2D关节位置-------------------------
        /// <summary>
        /// 根据用户索引,获取颜色贴图纹理上的关节2D位置。
        /// </summary>
        /// <param name="userIndex">限制0~5</param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetJointColorMapPos2D(int userIndex, KinectInterop.JointType jointType)
        {
            Vector3 pos = GetJointColorMapPos(userIndex, jointType);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }

        /// <summary>
        /// 根据用户索引，获取给定关节在彩色图像上的2D位置
        /// </summary>
        /// <param name="userIndex">限制0~5</param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetIndexJointPos2D(int userIndex, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            Vector3 pos = GetJointPosColorOverlay(userIndex, jointType, camera, offsetRect);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }

        /// <summary>
        /// 根据用户ID，获取颜色贴图纹理上的2D关节位置。
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetUserIDJointPos2D(long userID, KinectInterop.JointType jointType)
        {
            Vector3 pos = GetJointColorMapPos(userID, jointType);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }

        /// <summary>
        /// 根据用户ID，获取给定关节在彩色图像上的2D位置。
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetUserIDJointPos2D(long userID, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            Vector3 pos = GetJointPosColorOverlay(userID, jointType, camera, offsetRect);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }
        #endregion


        #region 获取用户的ID
        /// <summary>
        /// 根据索引获取用户ID
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetUserIdByIndex(int userIndex)
        {
            long userID = KinectManager.Instance.GetUserIdByIndex(userIndex);
            return userID;
        }

        /// <summary>
        /// 获取主要用户ID
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetMainUserIdByIndex()
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            return userID;
        }
        #endregion

    }


    /// <summary>
    /// 图像类型
    /// </summary>
    public enum KinectImageType
    {
        /// <summary>
        /// 彩色
        /// </summary>
        colour,
        /// <summary>
        /// 深度遮罩
        /// </summary>
        depthmask,

        /// <summary>
        /// 前景图（人像抠图）
        /// </summary>
        Foreground,
    }

}
