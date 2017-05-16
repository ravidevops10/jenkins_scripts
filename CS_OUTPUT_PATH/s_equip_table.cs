using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_equip_table
{
	private s_equip[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_equip_table sInstance = null;
	public static s_equip_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_equip_table();
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

		entities = new s_equip[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_equip();
			s_equip entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.name = vals[1];
			entity.equip_type= int.Parse(vals[2]);
			entity.weapon_type= int.Parse(vals[3]);
			entity.job= int.Parse(vals[4]);
			entity.color= int.Parse(vals[5]);
			entity.advanced_limit= int.Parse(vals[6]);
			entity.hurt_type = vals[7];
			entity.only= int.Parse(vals[8]);
			entity.animation = vals[9];
			entity.data_view = vals[10];
			entity.sell= int.Parse(vals[11]);
			entity.notice= int.Parse(vals[12]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_equip.FileName;
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
	public s_equip GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_equip);
		}
	}

	public s_equip GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_equip);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
