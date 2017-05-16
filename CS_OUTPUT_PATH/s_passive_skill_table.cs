using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_passive_skill_table
{
	private s_passive_skill[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_passive_skill_table sInstance = null;
	public static s_passive_skill_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_passive_skill_table();
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

		entities = new s_passive_skill[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_passive_skill();
			s_passive_skill entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.formula = vals[1];
			entity.battle_data= int.Parse(vals[2]);
			entity.pure_data= int.Parse(vals[3]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_passive_skill.FileName;
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
	public s_passive_skill GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_passive_skill);
		}
	}

	public s_passive_skill GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_passive_skill);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
