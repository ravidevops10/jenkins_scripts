#-*- coding=utf8 -*-
# author : albertcheng
# date   : 2017.5.8
# script : used for gernerate version config data for jenkins. It's coded for jenkins automatic jobs.


import os
import json
import time

b_branch = os.getenv("BRANCH")
b_version = os.getenv("JOKER_VERSION")
b_build = os.getenv("BUILD_NUMBER")
DeviceType = os.getenv("device_type")
VersionType = os.getenv("version_type")
workspace = os.getenv("WORKSPACE")
CODE_SVN_REVISION = os.getenv("SVN_REVISION_2")# because of jekens passed SVN_REVISION_2
RES_SVN_REVISION = os.getenv("SVN_REVISION_3")# because of jekens passed SVN_REVISION_3
versionFile = "version.json"
# only for host url json data
dev_host = os.getenv("RES_HOST_DEV")
publish_host = os.getenv("RES_HOST_PUBLISH")
launch_server = os.getenv("Launch_Server_Host")

Host_Config_File = os.path.join(os.getcwd(), r"./Assets/Plugins/JokerFrameWork/ResourceManager/Resources/resource_path.json")
Server_Config_File = os.path.join(os.getcwd(), r"./Assets/Resources/server_config.json")


def modify_server_json(_file):
    """
    # modify host url json data and return json.
    """
    with open(_file, "rb") as _f:
        _config = json.load(_f)
        _config["server_list"]["default"] = launch_server
        _config["server_list"]["lan"] = launch_server
    return _config


def modify_host_json(_file):
    """
    # modify host url json data and return json.
    """
    with open(_file, "rb") as _f:
        _config = json.load(_f)
        _config["HostInfo"]["Publish"] = publish_host
        _config["HostInfo"]["Dev"] = dev_host
    
    return _config

def write_json(_file, _json):
    """
    # write json data to file.
    """
    with open(_file, "wb") as _f:
        json.dump(_json, _f)
    return _json
        
def init_ver():
    """
    # initialized all version argus.
    """
    b_time = time.ctime()
    return {    
                "git": b_version + "_" + CODE_SVN_REVISION,
                "svn": SVN_REVISION, 
                "build":"%s-%s-%s-%s" % (DeviceType, b_branch, b_version, b_build), 
                "time":b_time
                }


if __name__ == "__main__":
    print write_json(versionFile, init_ver())
    import shutil
    shutil.copy(versionFile, "Assets/StreamingAssets/%s/" % DeviceType)
    os.remove(versionFile)
    print "current path is ", os.getcwd()
    if os.path.exists(Host_Config_File):
        host_json = modify_host_json(Host_Config_File)
        print "HostInfo data is modified, and now is ", host_json["HostInfo"]
        write_json(Host_Config_File, host_json)
    else:
        print "HostInfo File is not exists, skipped modify HostInfo file."

    if os.path.exists(Server_Config_File):
        server_json = modify_server_json(Server_Config_File)
        print "Launch Server Host data is modified, and now is ", server_json["server_list"]
        write_json(Server_Config_File, server_json)

    else:
        print "Launch Server Host File is not exists, skipped modify Launch Server Host file."
    