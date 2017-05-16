using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_rb_static_table
{
	private s_rb_static[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_rb_static_table sInstance = null;
	public static s_rb_static_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_rb_static_table();
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

		entities = new s_rb_static[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_rb_static();
			s_rb_static entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.nick_name = vals[1];
			entity.level= int.Parse(vals[2]);
			entity.vip_exp= int.Parse(vals[3]);
			entity.icon_id= int.Parse(vals[4]);
			entity.employe = vals[5];
			entity.equip1 = vals[6];
			entity.equip2 = vals[7];
			entity.equip3 = vals[8];
			entity.ranking= int.Parse(vals[9]);
			entity.fight= int.Parse(vals[10]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_rb_static.FileName;
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
	public s_rb_static GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_rb_static);
		}
	}

	public s_rb_static GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_rb_static);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
