#-*- coding=utf8 -*-
"""
test_live2d_res.py
@author: albertcheng
@date : create-2015.9.10
        modified-2017.6.10
        rewrite with pytest - 2017.6.17
@useage:py.test -q test_live2d.py
"""
import os
import sys
import json
import time
import pytest
import allure
LIVE2D_RES_PATH = os.getenv("LIVE2D_RES_PATH") or os.getcwd()

class Live2dResources(object):
    """
    need to write sth.
    """
    def __init__(self, cloth_path):
        self.npc_role_list = ["30401", "30301", "30200", "30501", "30101"]
        self.cloth_json = self.read_json(cloth_path)
        self.cloth_id = os.path.basename(cloth_path)[:-5].strip()
        self.cloth_root = os.path.dirname(cloth_path)
        self.hit_conf = {"face":"D_REF.Face", "body":"D_REF.Body", "chest":"D_REF.Chest"}

    @staticmethod
    def get_l2d_settings(role_path):
        """
        need to write sth.
        """
        _mocs = [os.path.join(role_path, x) for x in os.listdir(role_path) if x.endswith(".moc")]
        return [x.replace(".moc", ".json") for x in _mocs]

    @staticmethod
    def get_l2d_clothes():
        """
        need to write sth.
        """
        role_ids = [os.path.join(LIVE2D_RES_PATH, x)\
            for x in os.listdir(LIVE2D_RES_PATH)\
                if ".py" not in x and x.isdigit()]
        clothes = []
        for role in role_ids:
            clothes.extend(Live2dResources.get_l2d_settings(role))
        return clothes

    @staticmethod
    def read_json(cloth_data):
        """
        need to write sth.
        """
        with open(cloth_data, "rb") as _setting:
            return json.load(_setting)

@pytest.fixture(scope="module", params = Live2dResources.get_l2d_clothes())
def l2d_res(request):
    return Live2dResources(request.param)

def test_model(l2d_res):
    assert "model" in l2d_res.cloth_json
    moc_file = os.path.join(l2d_res.cloth_root, l2d_res.cloth_json["model"])
    assert os.path.exists(moc_file)

def test_texture(l2d_res):
    assert "textures" in l2d_res.cloth_json
    assert len(l2d_res.cloth_json["textures"]) > 0
    tt_files = [os.path.join(l2d_res.cloth_root, x) for x in l2d_res.cloth_json["textures"]]
    for texture in tt_files:
        assert os.path.exists(texture)

def test_hitareas(l2d_res):
    if l2d_res.cloth_id in l2d_res.npc_role_list:
        return
    assert "hit_areas" in l2d_res.cloth_json
    l2d_hit_areas = l2d_res.cloth_json["hit_areas"]
    assert len(l2d_hit_areas) == 3
    for _cfg in l2d_hit_areas:
        assert _cfg["name"] in l2d_res.hit_conf
        assert _cfg["id"] in l2d_res.hit_conf[_cfg["name"]]
        # to do: check if id-data is in moc config.

def test_motions(l2d_res):
    assert "motions" in l2d_res.cloth_json
    l2d_mtns = l2d_res.cloth_json["motions"]
    assert len(l2d_mtns) > 0
    for _group, _mtns in l2d_mtns.items():
        assert _group.strip() > 0
        for _mtn in _mtns:
            assert "file" in _mtn
            mtn_path = os.path.join(l2d_res.cloth_root, _mtn["file"])
            assert os.path.exists(mtn_path)
            if "sound" in _mtn:
                sound_path = os.path.join(l2d_res.cloth_root, _mtn["sound"])
                assert os.path.exists(sound_path)
    if l2d_res.cloth_id not in l2d_res.npc_role_list:
        assert "shake" in l2d_mtns
        assert "moxiong" in l2d_mtns

def test_physics(l2d_res):
    assert "physics" in l2d_res.cloth_json
    l2d_physics = os.path.join(l2d_res.cloth_root, l2d_res.cloth_json["physics"])
    assert os.path.exists(l2d_physics)

def test_expression(l2d_res):
    assert "expressions" in l2d_res.cloth_json
    l2d_exps = l2d_res.cloth_json["expressions"]
    assert len(l2d_exps) > 0
    alreay_check = []
    for _exp in l2d_exps:
        assert _exp["name"] not in alreay_check
        alreay_check.append(_exp["name"])
        exp_file = os.path.join(l2d_res.cloth_root, _exp["file"])
        assert os.path.exists(exp_file)
        l2d_res.read_json(exp_file)