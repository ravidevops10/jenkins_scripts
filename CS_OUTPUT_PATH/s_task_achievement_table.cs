using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_task_achievement_table
{
	private s_task_achievement[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_task_achievement_table sInstance = null;
	public static s_task_achievement_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_task_achievement_table();
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

		entities = new s_task_achievement[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_task_achievement();
			s_task_achievement entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.type= int.Parse(vals[1]);
			entity.next= int.Parse(vals[2]);
			entity.conditions_type= int.Parse(vals[3]);
			entity.conditions_data = vals[4];
			entity.achievement_points= int.Parse(vals[5]);
			entity.icon = vals[6];
			entity.name = vals[7];
			entity.dec_task = vals[8];
			entity.link = vals[9];
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_task_achievement.FileName;
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
	public s_task_achievement GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_task_achievement);
		}
	}

	public s_task_achievement GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_task_achievement);
		}
		else
		{
			return entities[idx];
		}
	}

}
}