using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static readonly object lockObj = new object();
        private static T _instance;
        public static T Instance
        {
            get
            {
                lock (lockObj)
                    if (_instance == null)
                        _instance = SingletonCreator.CreateSingleton<T>();
                return _instance;
            }
        }


        protected virtual void Dispose()
        {
            _instance = null;
        }
    }

}
