#if NET20 || NET30

#region func
/// <summary>
/// 委托方法 func 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
public delegate T Func<T>();
/// <summary>
/// 委托方法 func 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <param name="t"></param>
/// <returns></returns>
public delegate T1 Func<T, T1>(T t);
/// <summary>
/// 委托方法 func 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <returns></returns>
public delegate T2 Func<T, T1, T2>(T t, T1 t1);
/// <summary>
/// 委托方法 func 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <returns></returns>
public delegate T3 Func<T, T1, T2, T3>(T t, T1 t1, T2 t2);
/// <summary>
/// 委托方法 func 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <returns></returns>
public delegate T4 Func<T, T1, T2, T3,T4>(T t, T1 t1, T2 t2,T3 t3);
/// <summary>
/// 委托方法 func
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
/// <returns></returns>
public delegate T5 Func<T, T1, T2, T3, T4,T5>(T t, T1 t1, T2 t2, T3 t3,T4 t4);
/// <summary>
/// 委托方法 func
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
/// <param name="t5"></param>
/// <returns></returns>
public delegate T6 Func<T, T1, T2, T3, T4, T5,T6>(T t, T1 t1, T2 t2, T3 t3, T4 t4,T5 t5);
/// <summary>
/// 委托方法 func
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
/// <param name="t5"></param>
/// <param name="t6"></param>
/// <returns></returns>
public delegate T7 Func<T, T1, T2, T3, T4, T5, T6,T7>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,T6 t6);
#endregion func



#region action
/// <summary>
/// 委托方法 Action
/// </summary>
public delegate void Action();
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="t"></param>
public delegate void Action<T>(T t);
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
public delegate void Action<T, T1>(T t,T1 t1);
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
///<param name="t2"></param>
public delegate void Action<T, T1, T2>(T t, T1 t1,T2 t2);
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
public delegate void Action<T, T1, T2, T3>(T t, T1 t1, T2 t2,T3 t3);
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
public delegate void Action<T, T1, T2, T3, T4>(T t, T1 t1, T2 t2, T3 t3,T4 t4);
/// <summary>
/// 委托方法 func
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
/// <param name="t5"></param>
public delegate void Action<T, T1, T2, T3, T4, T5>(T t, T1 t1, T2 t2, T3 t3, T4 t4,T5 t5);
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
/// <param name="t5"></param>
///  <param name="t6"></param>
public delegate void Action<T, T1, T2, T3, T4, T5, T6>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,T6 t6);
/// <summary>
/// 委托方法 Action
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <param name="t"></param>
/// <param name="t1"></param>
/// <param name="t2"></param>
/// <param name="t3"></param>
/// <param name="t4"></param>
/// <param name="t5"></param>
/// <param name="t6"></param>
/// <param name="t7"></param>
public delegate void Action<T, T1, T2, T3, T4, T5, T6, T7>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,T7 t7);
#endregion action



#endif

#if NET20 || NET30 || NET35
#region tuple
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [System.Serializable]
#endif
public class Tuple<T>
{
    /// <summary>
    /// 
    /// </summary>
    public T Item1 { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public Tuple(T t)
    {
        this.Item1 = t;
    }
}


#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
[System.Serializable]
#endif
public class Tuple<T,T1>:Tuple<T>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public Tuple(T t,T1 t1):base(t)
    {
        this.Item2 = t1;
    }
    public T1 Item2 { get; private set; }
}

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
[System.Serializable]
#endif
public class Tuple<T,T1,T2>:Tuple<T,T1>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public Tuple(T t,T1 t1,T2 t2):base(t,t1)
    {
        this.Item3 = t2;
    }
    public T2 Item3 { get; private set; }
}

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
[System.Serializable]
#endif
public class Tuple<T,T1,T2,T3>:Tuple<T,T1,T2>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public Tuple(T t,T1 t1,T2 t2,T3 t3):base(t,t1,t2)
    {
        this.Item4 = t3;
    }
    public T3 Item4 { get; private set; }
}

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
[System.Serializable]
#endif
public class Tuple<T,T1,T2,T3,T4>:Tuple<T,T1,T2,T3>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public Tuple(T t,T1 t1,T2 t2,T3 t3,T4 t4):base(t,t1,t2,t3)
    {
        this.Item5 = t4;
    }
    public T4 Item5 { get; private set; }
}
#endregion tuple

#endif




/// <summary>
/// 数据库改变
/// </summary>
public delegate void DatabaeChannged();
/// <summary>
/// 默认方法
/// </summary>
internal delegate void DefaultMethod();
