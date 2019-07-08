using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Frame
{
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class
        {
            //获得私有构造函数
            var cons = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            //获得无参的构造函数
            var con = Array.Find(cons, c => c.GetParameters().Length == 0);
            if (con == null)
                throw new Exception(typeof(T) + "未找到私有无参构造方法");
            return con.Invoke(null) as T;
        }
    }
}
