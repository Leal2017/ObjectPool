using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Pool manager.
/// Pool stuff for re-use!!
/// </summary>
public static class PoolManager
{
	public static GameObject poolHolder; // Main object that will hold all unique pool holders and their pool objects
	public static Dictionary<string, PoolData> availablePools; // Dictionary of all available pools
	public static PoolVisualizer poolVisualizer; // Script reference so you can see all pools in Editor (look for the Pools object)
	public static bool debug = false; // Shows debug logs.

	public static void Initialize()
	{
		GameObject tPoolHolder = GameObject.Find("Pools"); 
		if (tPoolHolder == null) poolHolder = new GameObject("Pools"); 
		else poolHolder = tPoolHolder; 
		// Add the visualizer
		poolVisualizer = poolHolder.AddComponent<PoolVisualizer>(); // make new dict.
		availablePools = new Dictionary<string, PoolData>(); 
	}

	// Create a pool
	public static PoolData CreatePool(string aPath, string aPrefab, int anAmount)
	{
		PoolData poolData = new PoolData(); // New data ref.
		poolData.Initialize(aPath, aPrefab, anAmount); // Initialize
		availablePools[aPrefab] = poolData; // store
		poolVisualizer.allPoolData.Add(poolData); // add to list so I can see it in Editor
		// return it, so you can do stuff with it when necessary
		return poolData;
	}

	/// <summary>
	/// Does the pool exist.
	/// </summary>
	/// <returns><c>true</c>, if pool exist, <c>false</c> otherwise.</returns>
	/// <param name="aPoolName">A pool name.</param>
	public static bool DoesPoolExist(string aPoolName){
		return availablePools.ContainsKey(aPoolName); // does it exist?
	}

	/// <summary>
	/// Gets an object from pool.
	/// </summary>
	/// <returns>The object from pool.</returns>
	/// <param name="aPoolName">A pool name.</param>
	public static GameObject GetObjectFromPool(string aPoolName){
		if(!availablePools.ContainsKey(aPoolName)){if(debug) Debug.Log("[PoolManager] GetObjectFromPool. This is a last resort fallback! There is no pool with this name: " + aPoolName); return null;} 
		return availablePools[aPoolName].GetObject();
	}

	/// <summary>
	/// Returns an object to its pool.
	/// </summary>
	/// <param name="aPoolName">A pool name.</param>
	/// <param name="anObject">An object.</param>
	public static void ReturnObjectToPool(string aPoolName, GameObject anObject){
		if(!availablePools.ContainsKey(aPoolName)){
			if(debug)Debug.Log("[PoolManager] ReturnObjectToPool. This is a last resort fallback! There is no pool with this name: " + aPoolName + ". This object will be destroyed instead.");
			Object.Destroy(anObject);
		} else availablePools[aPoolName].ReturnObject(anObject);
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public static void Reset(){
		// Clear pools
		List<PoolData> allPoolData = new List<PoolData>();
		allPoolData.AddRange(availablePools.Values);
		PoolData poolData;
		for (int i = allPoolData.Count-1; i >= 0; i--) {
			poolData = allPoolData[i];
			allPoolData.RemoveAt(i);
			poolData.Destroy();
		}

		// new dictionary
		availablePools = new Dictionary<string, PoolData>();
	}
}

