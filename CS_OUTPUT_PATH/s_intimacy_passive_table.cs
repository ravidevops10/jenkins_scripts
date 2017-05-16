using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_intimacy_passive_table
{
	private s_intimacy_passive[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_intimacy_passive_table sInstance = null;
	public static s_intimacy_passive_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_intimacy_passive_table();
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

		entities = new s_intimacy_passive[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_intimacy_passive();
			s_intimacy_passive entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.role= int.Parse(vals[1]);
			entity.name = vals[2];
			entity.des = vals[3];
			entity.level= int.Parse(vals[4]);
			entity.hp= int.Parse(vals[5]);
			entity.mp= int.Parse(vals[6]);
			entity.attack= int.Parse(vals[7]);
			entity.defense= int.Parse(vals[8]);
			entity.critical= int.Parse(vals[9]);
			entity.critical_damage = float.Parse(vals[10]);
			entity.mp_recovery= int.Parse(vals[11]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_intimacy_passive.FileName;
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
	public s_intimacy_passive GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_intimacy_passive);
		}
	}

	public s_intimacy_passive GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_intimacy_passive);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
