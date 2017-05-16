#-*- coding=utf8 -*-
'''
Created at 2015.12.11， modified at 2017.5.16
@author: albertcheng
This script is used to generate csharp table\class files from txt config files.
And it's also design to used in jenkins jobs.
'''
import subprocess
import os
import shutil
import re
from module import *
from config import *
import json
import time
import sys
import hashlib
import telnetlib
import base64

CS_OUTPUT_PATH = os.getenv("CS_OUTPUT_PATH") or "./CS_OUTPUT_PATH"
TXT_INPUT_PATH = os.getenv("TXT_INPUT_PATH") or "./TXT_INPUT_PATH"

def check_path(_path):
    """
    initialize path.
    """
    if os.path.exists(_path):
        shutil.rmtree(_path)
    os.makedirs(_path)


class ResTool(object):
    """
    restool class
    """
    def __init__(self):
        """
        class initialized
        """
        check_path(CS_OUTPUT_PATH)

        for _file in os.listdir(TXT_INPUT_PATH):
            if not _file.endswith(".txt"):
                continue
            _sheet = os.path.join(TXT_INPUT_PATH, _file)
            print "handling with sheet file : ", _sheet

            _type = sheet_data[0]
            _name = sheet_data[1]
            _comment = sheet_data[2]
            _comment2 = sheet_data[3]
            if _type == None and _name == None and _comment == None:
                logger.error(log_msg_3 % _table_Name)
                continue
            logger.info("*" * 40)
            logger.info(log_msg_5 % _table_Name)
            self.gen_CS_From_Config(self.bin_res_path, _table_Name, _comment, _name, _type)
            self.gen_CS_TABLE_From_Config(self.bin_res_path,\
                    _table_Name, _comment, _name, _type)
            self.gen_Bin_Data_From_Config(self.bin_res_path, \
                    _table_Name, _comment, _name, _type, sheet_data[4:])
    def gen_CS_From_Config(self, outpath, filename, _comment, _name, _type):
        """
        gen cs from config txt files
        """
        out_filename = os.path.join(outpath, filename + ".cs")
        
        out_stream = u"using System;\nusing System.Collections.Generic;\nusing System.Text;\nusing UnityEngine;\n\nnamespace Table {\n"
        out_stream += u"public class %s\n{\n" % filename
        
        for _type_idx, _type_val in enumerate(_type):
            typeField = _type_val
            nameField = _name[_type_idx]
            commentField = _comment[_type_idx]
            if typeField.strip() == "" or nameField.strip() == "" or nameField.startswith("_"):
                continue
            
            if typeField.lower() == "boolean": typeField = "bool"
            
            if typeField != None: 
                out_stream += u"\t/// %s\n" % commentField
                out_stream += u"\tpublic %s %s;\n" % (typeField, nameField)
            
        out_stream += u"\n\tpublic \t static string FileName = \"%s\";\n}\n}\n" % filename
        
        with open(out_filename, "wb") as out_file:
            out_file.write(out_stream.encode("utf8"))
            logger.info(log_msg_6 % filename + ".cs")
            
    def gen_CS_TABLE_From_Config(self, outpath, filename, _comment, _name, _type):
        
        out_filename = os.path.join( outpath, filename + "_table.cs")
        
        out_stream = u"using System;\nusing System.Collections.Generic;\nusing System.Text;\nusing UnityEngine;\n\nnamespace Table {\n"
        out_stream += u"public class %s_table\n{\n" % filename
        out_stream += u"\tprivate %s[]\tentities;\n\n" % filename
        
        if _type[0] == "string":
            out_stream += u"\tprivate Dictionary<string, int>    keyIdxMap = new Dictionary<string, int>();\n\n"
        else:
            out_stream += u"\tprivate Dictionary<int, int>    keyIdxMap = new Dictionary<int, int>();\n\n"
            
        out_stream += u"\tprivate int count;\n\tpublic int Count\n\t{\n\t\tget { return this.count; }\n\t}\n\n"
        out_stream += u"\tstatic %s_table sInstance = null;\n\tpublic static %s_table Instance\n\t{\n\t\tget\n\t\t{\n" % (filename, filename)
        out_stream += u"\t\t\tif (sInstance == null)\n\t\t\t{\n\t\t\t\tsInstance = new %s_table();\n\t\t\t\tsInstance.Load();\n\t\t\t\t}\n\n" % filename
        out_stream += u"\t\t\treturn sInstance; \n\t\t}\n\t}\n\n\tvoid Load()\n\t{\n\t\tAction<byte[]> onTableLoad = (data) => {\n"
        out_stream += u"\t\tstring text = FileMgr.ReadTxt(data);\n\t\tstring[] lines = text.Split(\"\\n\\r\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);\n"
        out_stream += u"\t\tint count = lines.Length;\n\t\tif (count <= 0) {\n\t\t\treturn;\n\t\t}\n\n"
        out_stream += u"\t\tentities = new %s[count];\n\t\tint realcount = 0;\n" %  filename
        out_stream += u"\t\tfor (int i = 0; i < count; ++i) {\n\t\t\tstring line = lines[i];\n"
        out_stream += u"\t\t\t" + u"if (string.IsNullOrEmpty(line)) continue;\n"
        out_stream += u"\t\t\t" + u"string[] vals = line.Split('\\t');\n"
        out_stream += u"\t\t\t" + u"if(vals.Length < 2) continue;\n\n"
        out_stream += u"\t\t\t" + u"entities[i] = new %s();\n" % filename
        out_stream += u"\t\t\t" + u"%s entity = entities[i];\n\n\n" % filename
        
        fieldIndex = 0
        for _type_idx, _type_val in enumerate(_type):
            typeField = _type_val
            nameField = _name[_type_idx]
            
            if typeField.strip() == "" or nameField.strip() == "" or nameField.startswith("_"):
                continue
            
            if typeField.lower() == "boolean": typeField = "bool"
            
            if typeField.lower() == "int":
                out_stream += u"\t\t\t" + u"entity.%s= int.Parse(vals[%s]);\n" % (nameField, fieldIndex)
            elif typeField.lower() == "string":
                out_stream += u"\t\t\t" + u"entity.%s = vals[%s];\n" % (nameField, fieldIndex)
            elif typeField.lower() == "float":
                out_stream += u"\t\t\t" + u"entity.%s = float.Parse(vals[%s]);\n" % (nameField, fieldIndex)
            elif typeField.lower() == "bool":
                out_stream += u"\t\t\t" + u"entity.%s = int.Parse(vals[%s]) != 0;\n" % (nameField, fieldIndex)
            else:
                out_stream += u"\t\t\t" + u"entity.%s = (Table.%s) ( int.Parse(vals[%s]) );\n" % (nameField, _type_idx, fieldIndex)
                
            fieldIndex += 1
        
        out_stream += u"\t\t\tkeyIdxMap[ entity.%s ] = i;\n\t\t\t++realcount;\n\t\t}\n\n\n\t\tthis.count = realcount;\n\t};\n\n" % _name[0]
        out_stream += u"\tstring file = VerMgr.localPathRoot + \"/Table/\";\n\tfile = file + %s.FileName + \".bin\";\n" % filename
        out_stream += u"\tFileMgr.OpenFileBin(file, onTableLoad);\n}\n\n\n"
        
        
        if _type[0] == "string":
            out_stream += u"\tpublic %s GetEntityByKey(string key)\n\t{\n\t\tint idx;\n" %filename
        else:
            out_stream += u"\tpublic %s GetEntityByKey(int key)\n\t{\n\t\tint idx;\n" %filename
        out_stream += u"\t\t" + u"if (keyIdxMap.TryGetValue(key, out idx))\n\t\t{\n"
        out_stream += u"\t\t\t" + u"return entities[idx];\n"
        out_stream += u"\t\t" + u"}\n"
        out_stream += u"\t\t" + u"else\n"
        out_stream += u"\t\t" + u"{\n"
        out_stream += u"\t\t\t" + u"return default(%s);\n" % filename
        out_stream += u"\t\t" + u"}\n"
        out_stream += u"\t" + u"}\n\n"
        
        
        out_stream += u"\t" + u"public %s GetEntityByIdx(int idx)\n" % filename
        out_stream += u"\t" + u"{\n"
        out_stream += u"\t\t" + u"if(idx < 0 || idx > count)\n"
        out_stream += u"\t\t" + u"{\n"
        out_stream += u"\t\t\t" + u"return default(%s);\n" % filename
        out_stream += u"\t\t" + u"}\n"
        out_stream += u"\t\t" + u"else\n"
        out_stream += u"\t\t" + u"{\n"
        out_stream += u"\t\t\t" + u"return entities[idx];\n"
        out_stream += u"\t\t" + u"}\n"
        out_stream += u"\t" + u"}\n\n}\n}\n\n"
        
        with open(out_filename, "wb") as out_file:
            out_file.write(out_stream.encode("utf8"))
            logger.info(log_msg_6 % filename  + "_table.cs")
      
    def gen_Bin_Data_From_Config(self, outpath, filename, _comment, _name, _type, _data):
        
        out_filename = os.path.join( outpath, filename + ".bin")
        
        out_stream = u""
        
        for line in _data:
            if line == None or str(line[0]).strip() == "":
                continue
            for _idx in range(len(line)):
                _data_field = line[_idx]
                if type(_data_field) != unicode: 
                    _data_field = str(_data_field).strip()
                else:
                    _data_field = _data_field.strip()
                _type_field = _type[_idx].strip().lower()
                _name_field = _name[_idx].strip()
                if _name_field.startswith("_") or len(_type_field) == 0:
                    continue
                
                if _data_field.endswith(".0"):
                    _data_field = _data_field[:-2]
                    
                if _type_field in ["bool", "boolean", "int", "float"] :
                    if _data_field == "":
                        out_stream += u"0\t"
                    else:
                        out_stream += _data_field + u"\t"
                
                elif _type_field == "string":
                    out_stream += _data_field + u"\t"
                else:
                    logger.error(log_msg_9 % filename )
            out_stream += u"\r\n"   
        with open(out_filename, "wb") as out_file:
            out_file.write(out_stream.encode("utf8"))
            logger.info(log_msg_6 % filename + ".bin")
    
    def export_updateFromSVN(self, dirs):
        """
        # export coder's config resource from svn.
        """
        st = time.time()
            
        arugs = ["svn", "export",
                 SVN_UPDATE_PATH + dirs,  
                 self.update_res_path + "tool" + dirs,
                 "--username", "joker", "--password",  "joker123",
                 "--force"]
        p = subprocess.Popen(arugs, shell = True)
        p.communicate()
        logger.info(log_msg_35 % (time.time() - st))
        
    def export_ConfigFromSVN(self):
        """
        # export design's config resource from svn.
        """
        
        if os.path.exists(self.excel_res_path):
            shutil.rmtree(self.excel_res_path)
        
        arugs = ["svn", "export",
                 SVN_CONFIG_PATH, 
                 self.excel_res_path,
                 "--username", SVN_USER, "--password",  SVN_PASSWD,
                 "--depth","files"]
        p = subprocess.Popen(arugs, shell = True)
        p.communicate()
        logger.info(log_msg_2)
        # list needed excels
        self.ConfigExcels = [os.path.join(self.excel_res_path, x.decode("gbk")) for x in os.listdir(self.excel_res_path) if x.endswith(".xlsx")]
        # delete other files and dirs
        unusedConfigs = [shutil.rmtree(os.path.join(self.excel_res_path, x)) for x in os.listdir(self.excel_res_path) if os.path.isdir(x) or not x.endswith(".xlsx")]
        
    def export_Live2dFromSVN(self):
        """
        # export live2d resource from svn.
        """
        
        
        if os.path.exists(self.LIVE2D_res_path):
            shutil.rmtree(self.LIVE2D_res_path)
        
        arugs = ["svn", "export",
                 SVN_LIVE2D_PATH, 
                 self.LIVE2D_res_path,
                 "--username", SVN_USER, "--password", SVN_PASSWD]
        p = subprocess.Popen(arugs, shell = True)
        p.communicate()
        logger.info(log_msg_1)

    def execute_config_tests(self):
        
        self.config = {
                        "BinData":{
                                   "role":LoadSingleKey(self.bin_res_path + "Role", "id"),
                                   "Clothes":LoadSingleKey(self.bin_res_path + "Clothes", "id"),
                                   "ClassroomEmotion":LoadDoubleKey(self.bin_res_path + "ClassroomEmotion", "role", "id"),
                                   "TrainEmotion":LoadDoubleKey(self.bin_res_path + "TrainEmotion", "role", "id"),
                                   "Play":LoadSingleKey(self.bin_res_path + "Play", "id"),
                                   "TextDialogue":LoadSingleKey(self.bin_res_path + "TextDialogue", "id")
                                   },
                        "Live2D":{
                                  },
                        "RoleLive2d":{
                                      }
                        }
        logger.info("-" * 40)
        logger.info("*" * 40)
        logger.info(log_msg_13)
        st = time.time()
        self.start() 
        logger.info("*" * 40)
        logger.info(log_msg_8 % (time.time() - st))
        st = time.time()
        while time.time() - st < 5:
            time.sleep(1)
        
    def start(self):
        def __getCellData(data, title, pref, role):
            _ = []
            if pref not in title:
                return _
            else:
                for x in data[title.index(pref)].split("|"):
                    if "_" in x:
                        _.extend([(x, role) for x in x.split("_")])
                    else:
                        _.append((x, role))
                return _
            
        """
        # check role data and get clothes.
        """
        role_clothes = []
        roles = self.config["BinData"]["role"]
        role_table = roles["__table"]
        role_Plays = []
        role_live2d = []
        for roleID in roles:
            if roleID == "__table" or roleID not in self.EnableRole:continue# ignore table header
            role = roles[roleID] # get role data
            role_Plays.extend(self.checkRole(roleID, role, role_table))
            role_clothes.extend(__getCellData(role, role_table, "clothes_beginning", roleID))
            role_clothes.extend(__getCellData(role, role_table, "clothes", roleID))
        """
        # check clothes data and get live2d data
        """
        role_clothes = list(set(role_clothes))    
        for _cloth in role_clothes:
            role_live2d.extend(self.checkClothes(_cloth))  
        
        """
        # check live2d data
        """
        role_live2d = list(set(role_live2d))
        for _live2d in role_live2d:
            _live2d_name, _roleID = _live2d
            _live2d_path = self.LIVE2D_res_path + _live2d_name + ".json"
            if not os.path.exists(_live2d_path):
                logger.error(log_msg_11 % (_roleID, _live2d_name))
            else:
                if _live2d_name not in self.config["Live2D"]:
                    with open(_live2d_path) as _live2dFile:
                        live2dJson = json.load(_live2dFile)
                        self.config["Live2D"][_live2d_name] = live2dJson
                self.config["RoleLive2d"][_roleID] = _live2d_name
        """
        # check emotion config data and get play config 
        """
        ClassroomEmotions = self.config["BinData"]["ClassroomEmotion"]
        for __roleID in ClassroomEmotions:
            if __roleID == "__table":continue
            if __roleID not in self.config["BinData"]["role"]:
                logger.error(log_msg_12 % __roleID)
            else:
                role_ClassroomEmotion = ClassroomEmotions[__roleID]
                for nemotion in role_ClassroomEmotion:
                    role_Plays.extend(self.checkEmotion(nemotion, role_ClassroomEmotion[nemotion], ClassroomEmotions["__table"], __roleID))
        """
        # check live2d's Play data
        """
        
        role_Plays = list(set(role_Plays))
        config_plays = self.config["BinData"]["Play"]
        for nplayID, nroleID in role_Plays:
            self.checkPlays(nplayID, config_plays[nplayID], config_plays["__table"], nroleID)
        
        """
        # check Live2d's resource config 
        """
        for live2d in role_live2d:
            self.checkLive2d(live2d)
    
    def checkPlays(self, _playID, _play, config_plays_table, _roleID):
        #print _roleID, self.config["RoleLive2d"]
        if _roleID not in self.EnableRole:
            return
        
        if _roleID not in self.config["RoleLive2d"]:
            logger.error(log_msg_15 % _roleID)
            return
        
        _live2d_json = self.config["Live2D"][self.config["RoleLive2d"][_roleID]]
        
        # dialogue_id
        if "dialogue_id" not in config_plays_table:
            logger.error(log_msg_16 % "dialogue_id")
        else:
            dialogue_id = _play[config_plays_table.index("dialogue_id")]
            if dialogue_id not in self.config["BinData"]["TextDialogue"]:
                logger.error(log_msg_17 % (_playID, dialogue_id))
        # voice
        if "voice" not in config_plays_table:
            logger.error(log_msg_16 % "voice")
        else:
            voice = _play[config_plays_table.index("voice")]
            if voice.strip() != "" and voice not in _live2d_json["motions"]:
                logger.error(log_msg_18 % (_roleID, voice,  _live2d_json["model"]))
        # expression 
        if "expression" not in config_plays_table:
            logger.error(log_msg_16 % "expression")
        else:
            expression = _play[config_plays_table.index("expression")]
            if expression.strip() == "":
                logger.error(log_msg_32 % (_roleID, _playID))
            elif expression not in [x["name"] for x in _live2d_json["expressions"]]:
                logger.error(log_msg_19 % (_roleID, expression, _live2d_json["model"]))
        # action
        if "action" not in config_plays_table:
            logger.error(log_msg_16 % "action")
        else:
            motions = _play[config_plays_table.index("action")].split("|")
            for motion in motions:
                if motion.strip() == "":
                    logger.error(log_msg_33 % (_roleID, _playID)) 
                else:
                    action, idx, weight = motion.split("_")
                    if action not in _live2d_json["motions"]:
                        logger.error(log_msg_20 % (_roleID, action, _live2d_json["model"]))
                       
    def checkRole(self, roleID, role, role_table):
        """
        # check role data 
        """
        special_title = ["success_wait", "change_fail", "change_fail", "shake"]
        special_play = []
        for nemotion in special_title:
            if nemotion not in role_table:
                logger.error(log_msg_21 % nemotion)
            else:
                emotionID = role[role_table.index(nemotion)]
                if emotionID not in self.config["BinData"]["Play"]:
                    logger.error(log_msg_22 % (nemotion, emotionID))    
                else:
                    special_play.append((emotionID, roleID))
        return special_play
    
    def checkClothes(self, __cloth):
        """
        # check clothes data
        """
        clothID, roleID = __cloth
        __live2d = []
        
        if clothID not in self.config["BinData"]["Clothes"]:
            logger.error(log_msg_23 % (roleID, clothID))
            #raise ValueError
        else:
            cloth = self.config["BinData"]["Clothes"][clothID]
            cloth_table = self.config["BinData"]["Clothes"]["__table"]
            if "live_2d" not in cloth_table:
                logger.error(log_msg_24)
            else:
                clothID = cloth_table.index("live_2d")
                try:
                    __live2d.append((cloth[clothID], roleID))
                except:
                    logger.error(log_msg_25 % (roleID, clothID))
        return __live2d
           
    def checkEmotion(self, _emotionID, _emotion, emotion_table, _roleID):
        """
        """
        # emtion_level
        if "emotion_level" not in emotion_table:
            logger.error(log_msg_26 % "emotion_level")
        else:
            emotion_level = _emotion[emotion_table.index("emotion_level")]
            if int(emotion_level) not in range(1,6):
                logger.error(log_msg_27 % (_emotionID, "emotion_level", emotion_level))
        # classroomEmotion
        if "emotion_value" not in emotion_table:
            logger.error(log_msg_26 % "emotion_value")
        else:
            emotion_value = _emotion[emotion_table.index("emotion_value")]
            if int(emotion_value) not in range(1,101):
                logger.error(log_msg_27 % (_emotionID, "emotion_value", emotion_value))
        # area
        if "area" not in emotion_table:
            logger.error(log_msg_26 % "area")
        else:
            area = _emotion[emotion_table.index("area")]
            if area not in ["face", "body", "chest"]:
                logger.error(log_msg_27 % (_emotionID, "area", area))
        PlayIDS = []
        # value
        if "value" not in emotion_table:
            logger.error(log_msg_26 % "value")
        else:
            values = _emotion[emotion_table.index("value")].split("|")
            for nvalue in values:
                if nvalue.strip() == "":continue
                try:
                    playID, weight = nvalue.split("_")
                    if playID not in self.config["BinData"]["Play"]:
                        logger.error(log_msg_28 % (_emotionID, "value", playID))
                    else:
                        PlayIDS.append((playID, _roleID))
                except:
                    logger.error(log_msg_29 % (_emotionID, nvalue))
        return PlayIDS               
        
    def checkLive2d(self, __live2d):
        _live2d_Name, _roleID = __live2d
        _live2d_Data = self.config["Live2D"][_live2d_Name]
        # check live2d data
        for nkey in ["textures", "hit_areas", "expressions", "motions", "physics"]:
            if nkey not in _live2d_Data:
                logger.error(log_msg_30 % (_live2d_Name, nkey))
        
if __name__ == '__main__':
    ResTool()
    