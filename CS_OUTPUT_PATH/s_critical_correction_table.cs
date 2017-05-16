using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_critical_correction_table
{
	private s_critical_correction[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_critical_correction_table sInstance = null;
	public static s_critical_correction_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_critical_correction_table();
				sInstance.Load();
				}

			return sInstance; 
		}
	}

        	void Load()
	{
		Action<string> onTableLoad = (text) => {
		string[] lines = text.Split("\n\r".ToCharArray(),             StringSplitOptions.RemoveEmptyEntries);
		int count = lines.Length;
		if (count <= 0) {
        			return;
		}

		entities = new s_critical_correction[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_critical_correction();
			s_critical_correction entity = entities[i];


			entity.level= int.Parse(vals[0]);
			entity.critical= int.Parse(vals[1]);
			entity.anti_critical= int.Parse(vals[2]);
			keyIdxMap[ entity.level ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_critical_correction.FileName;
    TextAsset textAsset;
    if (AssemblySetting.LoadProtocInResource) {
        textAsset = Resources.Load<TextAsset>("Table/" + fileName);
    } else {
        textAsset = RuntimeResourceManager.Instance.LoadCachedAsset<TextAsset>(fileName); 
    }
    if (textAsset == null) {
        return;
    }
    onTableLoad(textAsset.text);}
	public s_critical_correction GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_critical_correction);
		}
	}

	public s_critical_correction GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_critical_correction);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
