using System;
using Modding;
using UnityEngine;

namespace WorldBoundaryKiller
{
	public class Mod : ModEntryPoint
	{
		public override void OnLoad()
        {
            GameObject gameObject = GameObject.Find("ModControllerObject");
            if (!gameObject)
            {
                gameObject = new GameObject("ModControllerObject");
                GameObject.DontDestroyOnLoad(gameObject);
            }
            gameObject.AddComponent<ModMainUI>();
            // Called when the mod is loaded.
        }
	}
}
