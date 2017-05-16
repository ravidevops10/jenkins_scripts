using System;
using System.Collections.Generic;
using Joker.ResourceManager;
using UnityEngine;

namespace Table {
public class s_weapon_correction_table
{
	private s_weapon_correction[]	entities;

	private Dictionary<string, int>	        keyIdxMap = new Dictionary<string, int>();

	private int count;
	public int Count
	{
        		get { return this.count; }
	}

	static s_weapon_correction_table sInstance = null;
	public static s_weapon_correction_table Instance
        	{
		get
		{
			if (sInstance == null)
			{
            				sInstance = new s_weapon_correction_table();
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

		entities = new s_weapon_correction[count];
		int realcount = 0;
		for (int i = 0; i < count; ++i) {
			string line = lines[i];
			if (string.IsNullOrEmpty(line)) continue;
			string[] vals = line.Split('\t');
			if(vals.Length < 2) continue;

			entities[i] = new s_weapon_correction();
			s_weapon_correction entity = entities[i];


			entity.id = vals[0];
			entity.normal_pve = float.Parse(vals[1]);
			entity.light_pve = float.Parse(vals[2]);
			entity.middle_pve = float.Parse(vals[3]);
			entity.heavy_pve = float.Parse(vals[4]);
			entity.normal_evp = float.Parse(vals[5]);
			entity.light_evp = float.Parse(vals[6]);
			entity.middle_evp = float.Parse(vals[7]);
			entity.heavy_evp = float.Parse(vals[8]);
			entity.normal_pvp = float.Parse(vals[9]);
			entity.light_pvp = float.Parse(vals[10]);
			entity.middle_pvp = float.Parse(vals[11]);
			entity.heavy_pvp = float.Parse(vals[12]);
			entity.normal_pve_restraint= int.Parse(vals[13]);
			entity.light_pve_restraint= int.Parse(vals[14]);
			entity.middle_pve_restraint= int.Parse(vals[15]);
			entity.heavy_pve_restraint= int.Parse(vals[16]);
			entity.normal_evp_restraint= int.Parse(vals[17]);
			entity.light_evp_restraint= int.Parse(vals[18]);
			entity.middle_evp_restraint= int.Parse(vals[19]);
			entity.heavy_evp_restraint= int.Parse(vals[20]);
			entity.normal_pvp_restraint= int.Parse(vals[21]);
			entity.light_pvp_restraint= int.Parse(vals[22]);
			entity.middle_pvp_restraint= int.Parse(vals[23]);
			entity.heavy_pvp_restraint= int.Parse(vals[24]);
			keyIdxMap[ entity.id ] = i;
			++realcount;
		}


        		this.count = realcount;
	};

    string fileName = s_weapon_correction.FileName;
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
	public s_weapon_correction GetEntityByKey(string key)            
	{
		int idx;
		if (keyIdxMap.TryGetValue(key, out idx))
		{
			return entities[idx];
		}
		else
		{
			return default(s_weapon_correction);
		}
	}

	public s_weapon_correction GetEntityByIdx(int idx)
	{
		if(idx < 0 || idx > count)
		{
			return default(s_weapon_correction);
		}
		else
		{
			return entities[idx];
		}
	}

}
}
