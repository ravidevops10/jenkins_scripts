#-*- coding=utf-8 -*-
"""
# author : albertcheng
# date   : 2017.5.8
# script : used for read data from excel, and upload data to mysql. It's coded for jenkins automatic jobs.

"""

import xlrd
import os
import re
import platform
import shutil, time

if platform.system().lower() == "windows":
    defaultEncoding = "gbk"
else:
    defaultEncoding = "utf8"

WriteToPath = "./textconfig/%s.txt"
Name_Start_Row = 1
Data_Start_Row = 4

class EasyXLS(object):
    """
    used for read excel.
    """

    def __init__(self):
        print "tool initialled."
        self.st = time.time()

    def getSheets(self, _file):
        if not os.path.exists(_file):
            print "xls not exists -- %s" % _file
            return []
        else:
            print "found excel : %s" % _file
            _workbook = xlrd.open_workbook(filename = _file, encoding_override = "utf8")
            return [_sheet for _sheet in _workbook.sheets() if _sheet.name.startswith("s_")] # return all static config sheet.

    def CheckInt(self, _data):
        # all(ord(c) < 128 for c in keyword)
        if type(_data) in [float, int]:
            return str(int(_data)) if str(_data).endswith(".0") else str(_data)
        elif type(_data) == unicode:
            return _data.encode("utf-8")
        else:
            return _data

    def appendStr(self, _row, data = []):
        #print _row
        if len([x for x in _row if len(str(x) if type(x) in [float, int] else x) > 0]) > 1:
            data.append("\t".join([self.CheckInt(cell) for cell in _row ]) + "\n")
            
            #print "\t".join([self.CheckInt(cell) for cell in _row ]) + "\n"
        return data

    def readNameRow(self, _sheet):
        self.name_row = _sheet.row_values(Name_Start_Row)
        self.name_ignore_index = [self.name_row.index(x) for x in self.name_row if x.startswith("_")]
        return self.appendStr([x for x in self.name_row if not x.startswith("_")], [])

    def readDataRow(self, _row, _sheet):
        if _sheet.name == "s_dropOut":
        ##    print len([_sheet.cell(_row, _col).value for _col in range(_sheet.ncols) if _col not in self.name_ignore_index])
        #    print len(_sheet.row_values(_row))
            print self.name_row
            print _sheet.row_values(_row)
            print [_sheet.cell(_row, _col).value for _col in range(_sheet.ncols) if _col not in self.name_ignore_index]
        #print "*" * 20
        return [_sheet.cell(_row, _col).value for _col in range(_sheet.ncols) if _col not in self.name_ignore_index]
        #return  _sheet.row_values(_row)

    def readDataFromSheet(self, _sheets):
        for nSheet in _sheets:
            print "read data from excel - [%s]" % nSheet.name
            SheetData = self.readNameRow(nSheet)
            for nRow in range(Data_Start_Row, nSheet.nrows):
                dataRow = self.readDataRow(nRow, nSheet)
                SheetData = self.appendStr(dataRow, SheetData)          

            print "read data from excel - [%s] finished." % nSheet.name
            writeToText(nSheet.name, SheetData)
            print "write data to text file finished."
            print "used time : %.2fs" % (time.time() - self.st)
            self.st = time.time()

def scanFiles():
    return ["./config/" + x.decode(defaultEncoding) for x in os.listdir("./config")]

def writeToText(_name, _data):
    with open(WriteToPath % _name, "wb") as _f:
        _f.writelines(_data)


if __name__ == "__main__":
    
    st = time.time()
    if os.path.exists("./textconfig"):shutil.rmtree("./textconfig")
    os.makedirs("./textconfig")
    xlsreader = EasyXLS()
    print "initialled time : %.2fs" % (time.time() - st)
    for nExcel in scanFiles():
        xlsreader.readDataFromSheet(xlsreader.getSheets(nExcel))
    print "totally used time : %.2fs" % (time.time() - st)