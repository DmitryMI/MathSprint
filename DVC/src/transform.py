import os
import sys
from PIL import Image
import numpy as np


if len(sys.argv) != 2:
    sys.stderr.write('Arguments error. Usage:\n')
    sys.stderr.write('\tpython prepare.py data\n')
    sys.exit(1)
input = sys.argv[1]


for (dirpath, dirnames, filenames) in os.walk(input):

    if filenames:
        filename_csv = dirpath.split("\\")[0] + "/../prepared/" + dirpath.split("\\")[1] + ".npy"
        if not os.path.exists(dirpath.split("\\")[0] + "/../prepared"):
            os.mkdir(dirpath.split("\\")[0] + "/../prepared")
        data_mas = None
        for filename in filenames:
            img = Image.open(dirpath + "/" + filename)
            data = np.asarray(img)
            data = np.expand_dims(data, axis=0)
            if data_mas is None:
                data_mas = np.array(data)
            else:
                if data_mas.shape[1] == data.shape[1]:
                    data_mas = np.append(data_mas, data, axis=0)
        np.savez_compressed(filename_csv, data_mas)

