#-*- coding=utf8 -*-
"""
@author: albertcheng
@date: create-2015.9.10 modified-2017.6.10
"""
import os
import sys
import json
import logging
import time
logging.basicConfig(stream=sys.stdout, level=logging.DEBUG,\
    format='%(asctime)s - %(lineno)s - %(levelname)s - %(message)s')
logger = logging.getLogger('')
live2d_res_path = os.getcwd().decode("gbk")



def rebuild(_file):
    """
    nothing to say just rebuild the json file.
    """
    if not os.path.exists(_file):
        logger.error("rebuild file failed cause file is not exists - %s" % _file)
        return
    
    with open(_file, "rb") as _f:
        _json = json.load(_f)
        _motion = _json["motions"]
        for _k, _v in _motion.items():
            _motion[_k] = [{"file":_mtn["file"]} for _mtn in _v]#ignore sound config cause 广电总局的审核版本需要！！！！
        _json["motions"] = _motion
    
    with open(_file, "wb") as _f:
        json.dump(_json, _f)
        



def handle_moc(_data, _path, role):
    """
    parse moc file and check it.
    """
    role_moc = os.path.join(_path, _data)

    if not os.path.exists(role_moc):
        logger.error("moc file -%s - %s is not exists." % (role, os.path.basename(role_moc)))
    # to do -> parse moc file struct and return it's struct data.
    moc_data = None
    return moc_data  

def handle_textures(_data, _path, role):
    """
    check texture files.
    """
    role_textures = [os.path.join(_path, t) for t in _data]
    for _texture in role_textures:
        if not os.path.exists(_texture):
            logger.error("texture file - %s-%s is not exists." % (role, os.path.basename(_texture)))


def handle_hitareas(_data, _path, role):
    """
    check hit area config, and to ensure the key part config's correction is in todo-list.
    """
    if len(_data) == 0:
        logging.error("hit_areas config is empty - %s" % role)
    for _area in _data:
        if _area["name"] not in ["face", "body", "chest"]:
            logging.error("[%s] - [%s] is not correct. " % (role, repr(_area)))
        if _area["name"] == "face" and _area["id"] != "D_REF.Face":
            logging.error("[%s] - [%s] is not correct. " % (role, repr(_area)))
        if _area["name"] == "body" and _area["id"] != "D_REF.Body":
            logging.error("[%s] - [%s] is not correct. " % (role, repr(_area)))
        if _area["name"] == "chest" and _area["id"] != "D_REF.Chest":
            logging.error("[%s] - [%s] is not correct. " % (role, repr(_area)))


def handle_expressions(_data, _path, role):
    """
    handle expressions data.
    """
    if len(_data) == 0:
        logging.error("[%s] - expressions config is empty." % role)
    for _exp in _data:
        _file = os.path.join(_path, _exp["file"])
        if not os.path.exists(_file):
            logging.error("[%s] - [%s] is not exists. " % (role, _exp["file"]))
        else:
            with open(_file, "rb") as _f:
                try:
                    json.load(_f)
                except:
                    logger.error("parse expression file faild - [%s] - [%s]" % (role, _exp["file"]))
def handle_motions(_data, _path, role):
    """
    check motion data.
    """
    if len(_data) == 0:
        logging.error("[%s] - motions config is empty. " % role)
        return
    
    if "moxiong" not in _data:
        logging.error("[%s] - moxiong is not in motions config. " % role)
    if "shake" not in _data:
        logging.error("[%s] - shake is not in motions config. " % role)

    for _group, _motions in _data.items():
        if _group.strip() == "":
            logger.error("motion config error! find empty group name in setting - %s" % role)
        
        for _motion in _motions:
            if "sound" in _motion and not os.path.exists(os.path.join(_path, _motion["sound"])):
                logger.error("sound config error! could not find sound file %s in %s" % (os.path.basename(_motion["sound"]), role))
            if not "file" in _motion:
                logger.error("motion config error! could not find file config in [%s]s' motion config group - %s" % (_group, role))
            elif not os.path.exists(os.path.join(_path, _motion["file"])):
                logger.error("motion file - %s is not exists." % _motion["file"])


def handle_physics(_data, _path, role):
    """
    check physics data.
    """
    physics_json = os.path.join(_path, _data)
    if not os.path.exists(physics_json):
        logging.error("[%s] is not exists. " % physics_json)
    else:
        # todo:
        # 暂时不对physics的json数据进行检查
        # 因为其格式虽然固定，但数据内容比较繁杂，自动检查效果一般
        # 且一般都是通过编辑器导出生成，所以在viewer里面查看表现效果比较直接简便
        pass


def main():
    """
    only running this code in the live2d resources root directory.
    """
    logger.info("tool started.")
    start_time = time.time()
    # step 1: get role uids from root resources dirnames.
    role_uids = dict([(x, os.path.join(live2d_res_path, x)) for x in os.listdir(live2d_res_path) if ".py" not in x])

    # step 2: check roles' datas
    for role_id, role_path in role_uids.items():
        root_files = os.listdir(role_path)
        root_path = os.path.join(live2d_res_path, role_path)
        logger.info("-"*20)
        _mocs = [os.path.join(root_path, x) for x in root_files if x.endswith(".moc")]
        _settings = dict([(os.path.basename(x)[:-4], x.replace(".moc", ".json")) for x in _mocs])
        logger.info("try to check role - [%s]s' live2d config." % role_id)
        check_role_config(_settings, root_path)
    logger.info("finished all task -- used time : %.2fs" % (time.time() - start_time))

def rename_unity_files(_file):
    """
    rename file to unity bytes file.
    """
    _new_file = _file + ".bytes"
    if os.path.exists(_new_file):
        os.remove(_new_file)
    if not os.path.exists(_file):
        logger.error("try to rename an none exists file - %s" % _file)
    else:
        os.rename(_file, _new_file)
    
def check_role_config(_files, _path):
    """
    """
    npc_role_list = ["30401", "30301", "30200", "30501", "30101"]
    for _id, _file in _files.items():
        role_setting_file = os.path.basename(_file)
        logger.info("handling with role id - %s" % _id)
        if not os.path.exists(_file):
            logger.error("setting file - %s is not exists." % role_setting_file)
            continue
    

        try:
            with open(_file, "rb") as _setting:
                role_json = json.load(_setting)
        except Exception, e:
            logger.error("parse setting file's json data error - %s" % role_setting_file)
            logger.error(e)
            continue

        if "model" in role_json:
            handle_moc(role_json["model"], _path, _id)
        else:
            logger.error("setting file does not contain key [model] in file - %s" % _id)
        
        if "textures" in role_json:
            handle_textures(role_json["textures"], _path, _id)
        else:
            logger.error("setting file does not contain key [textures] in file - %s" % _id)
        
        if "physics" in role_json:
            handle_physics(role_json["physics"], _path, _id)
        else:
            logger.error("setting file does not contain key [physics] in file - %s" % _id)
        
        if _id in npc_role_list:
            continue
        

        if "hit_areas" in role_json:
            handle_hitareas(role_json["hit_areas"], _path, _id)
        else:
            logger.error("setting file does not contain key [hit_areas] in file - %s" % _id)

        if "motions" in role_json:
            handle_motions(role_json["motions"], _path, _id)
        else:
            logger.error("setting file does not contain key [motions] in file - %s" % _id)
       
        if "expressions" in role_json:
            handle_expressions(role_json["expressions"], _path, _id)
        else:
            logger.error("setting file does not contain key [expressions] in file - %s" % _id)
        
        logger.info("setting file - %s has finished checking." % _id)
        rebuild(_file)

main()
#os.system("pause")