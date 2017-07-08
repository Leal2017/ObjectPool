using UnityEngine;
using System.Collections;

public class DestroyOnParticlesDone : MonoBehaviour
{
	private string prefab;
	private ParticleSystem[] systems;

	// PIETER. I like my manual initializations...
	public void Initialize(string aPrefab){
		prefab = aPrefab;
		systems = gameObject.GetComponentsInChildren<ParticleSystem>(true);
		foreach(ParticleSystem system in systems) // Force any looping to false!! Let's face it we want to destroy particles when done...
			system.loop = false;

	}

	void Update(){
		bool destroy = true;
		foreach(ParticleSystem system in systems){
			if(system.IsAlive() || system.particleCount > 0){destroy = false; return;}
		}
		if (destroy)
			PoolManager.ReturnObjectToPool(prefab, gameObject); // IF THERE IS NO POOL THIS OBJECT WILL BE DESTROYED!!
	}
}