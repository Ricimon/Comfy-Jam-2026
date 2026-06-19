using Svelto.ECS;

public delegate void QueryIndexCallback<T>(uint i, ref T t);
public delegate void QueryIndexCallback<T1, T2>(uint i, ref T1 t1, ref T2 t2);
public delegate void QueryIndexCallback<T1, T2, T3>(uint i, ref T1 t1, ref T2 t2, ref T3 t3);
public delegate void QueryIndexCallback<T1, T2, T3, T4>(uint i, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);

public delegate void QueryEgidCallback<T>(EGID egid, ref T t);
public delegate void QueryEgidCallback<T1, T2>(EGID egid, ref T1 t1, ref T2 t2);
public delegate void QueryEgidCallback<T1, T2, T3>(EGID egid, ref T1 t1, ref T2 t2, ref T3 t3);
public delegate void QueryEgidCallback<T1, T2, T3, T4>(EGID egid, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);