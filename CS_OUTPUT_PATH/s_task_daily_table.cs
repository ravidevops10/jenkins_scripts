using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_task_daily_table
{
	private s_task_daily[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_task_daily_table sInstance = null;
	public static s_task_daily_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_task_daily_table();
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

		entities = new s_task_daily[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_task_daily();
			s_task_daily entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.level= int.Parse(vals[1]);
			entity.conditions_type= int.Parse(vals[2]);
			entity.conditions_data = vals[3];
			entity.award = vals[4];
			entity.award_daily= int.Parse(vals[5]);
			entity.icon = vals[6];
			entity.name = vals[7];
			entity.dec_task = vals[8];
			entity.link = vals[9];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_task_daily.FileName;
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
	public s_task_daily GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_task_daily);
		}
	}

	public s_task_daily GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_task_daily);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
