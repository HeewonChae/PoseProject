call C:\Users\korma\anaconda3\Scripts\activate.bat C:\Users\korma\anaconda3
call conda activate tensor_gpu

set FLASK_ENV=development
set BOILERPLATE_ENV=dev

call python .\Poseidon_Predictor_Server\server_dev.py

pause