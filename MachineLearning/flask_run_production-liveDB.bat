call C:\Users\korma\anaconda3\Scripts\activate.bat C:\Users\korma\anaconda3
call conda activate tensor_gpu

set FLASK_ENV=production
set BOILERPLATE_ENV=prod

call python .\Poseidon_Predictor_Server\server_prod.py

pause