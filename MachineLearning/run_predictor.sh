#!/bin/bash

echo "Activate pose_env"
source $HOME/pose_env/bin/activate

echo "Install requirement packages"
pip3 install -r $HOME/predictor/requirements.txt

echo "Run predictor"
export FLASK_ENV=production
export BOILERPLATE_ENV=prod
python3 $HOME/predictor/server_prod.py 1>> $HOME/log/predictor_log.log 2>> $HOME/log/predictor_error.log&


