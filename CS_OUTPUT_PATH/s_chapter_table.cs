using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_chapter_table
{
	private s_chapter[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_chapter_table sInstance = null;
	public static s_chapter_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_chapter_table();
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

		entities = new s_chapter[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_chapter();
			s_chapter entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.type= int.Parse(vals[1]);
			entity.index= int.Parse(vals[2]);
			entity.level= int.Parse(vals[3]);
			entity.star_reward_condition = vals[4];
			entity.star_reward = vals[5];
			entity.name= int.Parse(vals[6]);
			entity.des= int.Parse(vals[7]);
			entity.notice= int.Parse(vals[8]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_chapter.FileName;
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
	public s_chapter GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_chapter);
		}
	}

	public s_chapter GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_chapter);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
