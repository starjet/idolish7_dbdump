# idolish7_dbdump

DB dump:  
Grab master.db from /data/data/<app package name>/files/db.  
Grab Assembly-CSharp.dll and UnityEngine.dll from the APK.  
Place alongside WindowsFormsApplication2.exe.  
Run.  
(Useful output not guaranteed)  

File decryption:  
Grab Assembly-CSharp.dll and UnityEngine.dll from the APK.  
Place alongside WindowsFormsApplication2.exe.  
Run via command line: WindowsFormsApplication2 -decryptfile filename  
(Where filename should be replaced by the full path to the encrypted file)  
(This can also be used to decrypt a master.db directly downloaded from server, which can then be opened in SQLite Browser or other similar SQLite program, instead of dumping db from app data to CSV)  

v2: 
Added file decryption feature.  
To use this, run via command line: WindowsFormsApplication2 -decryptfile filename  
(Where filename should be replaced by the full path to the encrypted file)  

v3 (possibly final):  
Broken DB dump feature fixed.  
Output adjusted so that line breaks don't break the CSV format.  
