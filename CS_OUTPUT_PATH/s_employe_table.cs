using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_employe_table
{
	private s_employe[]	entities;

	private Dictionary<int, int>	        keyIdxMap = new Dictionary<int, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_employe_table sInstance = null;
	public static s_employe_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_employe_table();
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

		entities = new s_employe[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_employe();
			s_employe entity = entities[i];


			entity.id= int.Parse(vals[0]);
			entity.name= int.Parse(vals[1]);
			entity.job= int.Parse(vals[2]);
			entity.level_beginning= int.Parse(vals[3]);
			entity.quality_beginning= int.Parse(vals[4]);
			entity.star_beginning= int.Parse(vals[5]);
			entity.element_type = vals[6];
			entity.skill = vals[7];
			entity.color= int.Parse(vals[8]);
			entity.clothes_beginning = vals[9];
			entity.equip_beginning = vals[10];
			entity.beginning_emotion= int.Parse(vals[11]);
			entity.clothes = vals[12];
			entity.like_item = vals[13];
			entity.role_piece = vals[14];
			entity.role_compose = vals[15];
			entity.clothes_group = vals[16];
			entity.move_speed = vals[17];
			entity.weapon_id= int.Parse(vals[18]);
			entity.notice= int.Parse(vals[19]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_employe.FileName;
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
	public s_employe GetEntityByKey(int key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_employe);
		}
	}

	public s_employe GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_employe);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
