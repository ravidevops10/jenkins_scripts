#-*- coding=utf8 -*-
"""
#Client.json
_jsonData = {
    "android": {
        "version": "1.4.8",
        "build": "70",
        "git": "0d5e68...",
        "size": "387.41 MB",
        "time": "2016-09-27 20:28",
        "name":"Xgame_build_dev_1.4.8_6e55.apk",
        "url": 'http://192.168.1.143:8080/view/client_publish/job/client_android_publish_reslocal_full/70/artifact/target/Xgame_build_dev_1.4.8_0d5e.apk'
    },
    "ios": {
        "version": "1.4.8",
        "build": "41",
        "git": "0d5e68...",
        "size": "365.42 MB",
        "time": "2016-09-27 20:29",
        "name":"Xgame_build_dev_1.4.8_6e55.ipa",
        "url": "itms-services://?action=download-manifest&url=https://oe5ing96n.qnssl.com/client1.4.8_41_0d5e68.plist"
    }
}
"""

import os
import shutil
import urllib2
import json
import time
import subprocess
# step 1: get env data

b_package = os.getenv("package_Name")
b_version = os.getenv("JOKER_VERSION")
b_build = os.getenv("BUILD_NUMBER")
#b_git = os.getenv("GIT_COMMIT")[:6]
b_time = time.ctime()
DeviceType = os.getenv("device_type").lower()
VersionType = os.getenv("version_type")
workspace = os.getenv("WORKSPACE")
SVN_REVISION_CODE = os.getenv("SVN_REVISION_2")# because of jekens passed SVN_REVISION_2
SVN_REVISION_RES = os.getenv("SVN_REVISION_3")# because of jekens passed SVN_REVISION_3
versioned_package_name = os.getenv("BUILD_PACKAGE_NAME")
ServerIP = "http://192.168.1.24"
history_url = ServerIP + r"/json/history.json"
remote_json  = ServerIP + r"/json/client.json"
remote_plist = ServerIP + r"/plist/client.plist"

def read_url_json(_json_url):
    """
    # read json data of a url json object
    """
    html_json = urllib2.urlopen(_json_url)
    return json.loads(html_json.read())
    
def get_url_json(_url):
    client_json = read_url_json(_url)
    client_json[DeviceType]["version"] = b_version
    client_json[DeviceType]["build"] = b_build
    client_json[DeviceType]["git"] = SVN_REVISION_CODE
    client_json[DeviceType]["time"] = b_time
    return client_json
    
def update_android_json(_json, _prefix, _remote):
    """
    # update all android-version-json-files' data
    # such as history.json, version.json, client.json
    """
    # initialize data for android json object.
    _name = "%s.apk" % (_prefix)
    _package = "./target/%s" % (_name)
    # update client json data
    _json[DeviceType]["name"] = _name
    _json[DeviceType]["size"] = "%.2f MB" % (os.path.getsize(_package) / 1024 / 1024)  
    _json[DeviceType]["url"] = "%s%s" % (_remote, _name)
    # refresh all json files. including history.json, version.json, client.json
    refresh_json_files(_name, _prefix, _json)
    return _json

def update_ios_json(_json, _prefix, _remote):
    """
    # update all ios-version-json-files' data
    # such as history.json, version.json, client.json, and ios system's plist file.
    """
    # initialize data for ios json object.
    _name = "%s.ipa" % (_prefix)
    _package = "./target/Xgame_build/build/Release-iphoneos/%s" % (_name)
    # update client json data
    _json[DeviceType]["name"] = _name
    _json[DeviceType]["size"] = "%.2f MB" % (os.path.getsize(_package) / 1024 / 1024) 
    _json[DeviceType]["url"] = "itms-services://?action=download-manifest&url=https://oe5ing96n.qnssl.com/%s.plist" % (_prefix) 
    # update plist data
    update_ios_plist(_json, _prefix, _remote)
    # refresh all json files. including history.json, version.json, client.json
    refresh_json_files(_name, _prefix, _json)
    return _json


def refresh_json_files(_name, _prefix,_json):
    # update history json data
    _history = read_url_json(history_url)
    _history[DeviceType][b_package].append({"name":_name,"time":b_time,"json":r"%s.json" % _prefix})
    # update version json data
    _version = {"git": SVN_REVISION_CODE, "svn": SVN_REVISION_RES, "build":"%s-%s-%s-%s" % (DeviceType, b_branch, b_version, b_build), "time":b_time}
    # write json data to files.
    write_local_file(r"history.json", json.dumps(_history))
    write_local_file(r"version.json", json.dumps(_version))
    write_local_file(r"client.json", json.dumps(_json))
    write_local_file(r"%s.json" % _prefix, json.dumps(_json))
    # show json data in jenkins page.
    print "current client json data is ", _json
    print "current version json data is ", _version


def update_ios_plist(_json, _prefix, _remote):
    # initialize plist arguments.
    _name = "%s.ipa" % (_prefix)    
    b_url = "%s%s" % (_remote, _name)
    _package = "./target/Xgame_build/build/Release-iphoneos/%s" % (_name)
    # get plist data
    _rt_plist_content = get_remote_plist(remote_plist, b_url, b_version, _name)
    print _rt_plist_content
    # write local plist
    write_local_file("%s.plist" % (_prefix), _rt_plist_content)
    # upload to qiniu server
    upload_to_qiniu("%s.plist" % (_prefix))
        
def upload_to_qiniu(_plist):
    if os.path.exists(_plist):
        # upload plist to qiniu cloud server
        cmd1 = ["/Users/xgame/qshell/qshell", "account", "KqZFGI68fk7zYn5SYudoAnfhouaeXc0r25OWD71F", "rkgO8a5B7vUVuUxH4PRqPGrIYYMowe36Zf93SHYO"]
        cmd2 = ["/Users/xgame/qshell/qshell", "fput", "joker", _plist, _plist]
        for _cmd in [cmd1,cmd2]:
            print "do cmd : ", _cmd
            p = subprocess.Popen(_cmd)
            p.communicate()
    else:
        print "found error!!!!!!!!!!! plist file is not exists."

def get_remote_plist(_remote, _url, _ver, _name):
    """
    # modify remote plist file's content
    """
    _plist =  urllib2.urlopen(_remote)
    _plist_data = _plist.read()
    import re
    sinfo1 = re.compile(r"{client_url}")
    sinfo2 = re.compile(r"{version}")
    sinfo3 = re.compile(r"{name}")
    content = sinfo1.sub(_url, _plist_data)
    content = sinfo2.sub(_ver, content)
    content = sinfo3.sub(_name, content)
    return content

def write_local_file(_file, _info):
    """
    # public write method.
    """
    with open(_file, "wb") as _f:
        _f.write(_info)            


    
def main():
    """
    # update all version json files' data such as history.json, version.json, client.json, and ios system's plist file.
    """
    print "ready to run python-based version info generator, the package name is ", versioned_package_name
    # step 1: prepare arguments.
    remote_package_url = ServerIP + r"/download/%s/%s/" % (DeviceType, b_package)
    #versioned_package_name = "%s_%s_%s_%s_%s_%s_build%s_%s" % (b_package, DeviceType, b_branch, b_version, b_git, SVN_REVISION, b_build, VersionType)
    # step 2: prepare client json
    client_json = get_url_json(remote_json)
    # step 3: update all json file.
    if DeviceType.lower() == "android" or DeviceType[0].lower() == "a":
        client_json = update_android_json(client_json, versioned_package_name, remote_package_url)
    elif DeviceType.lower() == "ios" or DeviceType[0].lower() == "i":
        client_json = update_ios_json(client_json, versioned_package_name, remote_package_url)
     
if __name__ == "__main__":

    main()
    
   


