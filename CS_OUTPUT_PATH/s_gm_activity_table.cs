using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_gm_activity_table
{
	private s_gm_activity[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_gm_activity_table sInstance = null;
	public static s_gm_activity_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_gm_activity_table();
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

		entities = new s_gm_activity[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_gm_activity();
			s_gm_activity entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.switches= int.Parse(vals[1]);
			entity.icon = vals[2];
			entity.order_id= int.Parse(vals[3]);
			entity.name = vals[4];
			entity.des = vals[5];
			entity.banner = vals[6];
			entity.timer_type= int.Parse(vals[7]);
			entity.enable_time = vals[8];
			entity.enable_last_time= int.Parse(vals[9]);
			entity.open_time = vals[10];
			entity.open_last_time= int.Parse(vals[11]);
			entity.template_type = vals[12];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_gm_activity.FileName;
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
	public s_gm_activity GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_gm_activity);
		}
	}

	public s_gm_activity GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_gm_activity);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
