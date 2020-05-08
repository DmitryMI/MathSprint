import os
import errno
from PIL import Image
import numpy as np
import sys

if len(sys.argv) != 3:
    sys.stderr.write('Arguments error:\n')
    sys.exit(1)

input = sys.argv[1]
input2 = sys.argv[2]

img = Image.open(input)
data = np.asarray(img)
f = open(input2 + "/auc.metric", "w")
f.write(str(np.sum(data)+1))

