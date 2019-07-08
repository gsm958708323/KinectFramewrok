using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    public class InputFactory : Singleton<InputFactory>
    {
        private IInput input;
        private InputFactory() { }
        /// <summary>
        /// 根据不同SDK创建不同输入方式
        /// </summary>
        /// <returns></returns>
//        public IInput Create()
//        {
//            if (input == null)
//#if WAVR_VR
//                input = new WaveVRInput();
//#elif PICO_VR
//                input = new PicoVRInput();
//#else 
//                //input = new SteamVRInput();
//#endif 
//            return input;
//        }
    }
}
