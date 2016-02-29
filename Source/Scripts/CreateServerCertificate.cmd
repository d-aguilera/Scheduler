@echo off

set "sdk=C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin"
set "cahome=C:\Users\Daniel\Google Drive\Confidencial"
set "target=%userprofile%\Desktop"

"%sdk%\makecert.exe" ^
-n "CN=localhost" ^
-iv "%cahome%\CA for Visual Studio Development Private Key.pvk" ^
-ic "%cahome%\CA for Visual Studio Development.cer" ^
-pe ^
-a sha512 ^
-len 4096 ^
-sky exchange ^
-eku 1.3.6.1.5.5.7.3.1 ^
-sv "%target%\localhost.pvk" ^
"%target%\localhost.cer"

"%sdk%\pvk2pfx.exe" ^
-pvk "%target%\localhost.pvk" ^
-spc "%target%\localhost.cer" ^
-pfx "%target%\localhost.pfx" ^
-po Scheduler ^
-f

del "%target%\localhost.cer"
del "%target%\localhost.pvk"
