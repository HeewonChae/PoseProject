@echo off

ECHO ### Warning ~!!! Critical ~!!! ###

PAUSE

FOR /f "tokens=1-3 delims=- " %%i in ('date/t') do @SET DT=%%i%%j%%k
FOR /f "tokens=1-2 delims=: " %%i in ('time/t') do @SET TM=%%i%%j

SET DST=C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe
SET SRC=C:\Hod_Source\hod
SET HOST=C:\Hod_Script\rsa_host.txt

ECHO ######################## Delete Exist RSA !! #########################
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pz "PoseServerRSA" 

ECHO ######################## RSA Publish  !! #########################
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pi "PoseServerRSA" "PoseServerRSA_Key.xml" -exp
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pa "PoseServerRSA" "IIS APPPOOL\PoseSportsPredictPool"

ECHO ######################## Check Result  !! #########################
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -px "PoseServerRSA" "Test_HodGameRSAKey.xml" -pri

:END