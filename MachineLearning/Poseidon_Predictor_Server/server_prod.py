import os
from app.main import create_app
from app import blueprint
from waitress import serve
import pandas as pd
from warnings import simplefilter

# import warnings filter
# ignore all future warnings
simplefilter(action='ignore', category=FutureWarning)
pd.options.mode.chained_assignment = None

app = create_app()
app.register_blueprint(blueprint)
app.app_context().push()


if __name__ == '__main__':
    serve(app, host='0.0.0.0', port=5000)
