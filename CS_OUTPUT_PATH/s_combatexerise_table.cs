using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_combatexerise_table
{
	private s_combatexerise[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_combatexerise_table sInstance = null;
	public static s_combatexerise_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_combatexerise_table();
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

		entities = new s_combatexerise[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_combatexerise();
			s_combatexerise entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.next_id= int.Parse(vals[1]);
			entity.type= int.Parse(vals[2]);
			entity.icon = vals[3];
			entity.time_limit= int.Parse(vals[4]);
			entity.map_id = vals[5];
			entity.reward = vals[6];
			entity.prize= int.Parse(vals[7]);
			entity.search = vals[8];
			entity.notice= int.Parse(vals[9]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_combatexerise.FileName;
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
	public s_combatexerise GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_combatexerise);
		}
	}

	public s_combatexerise GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_combatexerise);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
