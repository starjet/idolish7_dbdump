# idolish7_dbdump

Grab master.db from /data/data/<app package name>/files/db.
Grab Assembly-CSharp.dll and UnityEngine.dll from the APK.
Place alongside WindowsFormsApplication2.exe.
Run.

(Useful output not guaranteed)

v2:
Added file decryption feature.
To use this, run via command line: WindowsFormsApplication2 -decryptfile filename
(Where filename should be replaced by the full path to the encrypted file)
