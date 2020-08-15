using System;
using System.IO;

namespace TowerJump.Save.provider
{
    public class JsonBinaryDiskLoadProvider : JsonDiskLoadProvider
    {
        protected override void BeginObjectLoad(Guid objectGuid)
        {
            var filePath = JsonDiskSaveProvider.GetSavePath(objectGuid);
            var binaryText = File.ReadAllText(filePath);

            //decrypt the file and write it back to the file
            File.WriteAllText(filePath, JsonBinaryDiskSaveProvider.DecryptString(binaryText));
            //let the baseclass read it
            base.BeginObjectLoad(objectGuid);
            //encrypt it again
            File.WriteAllText(filePath,binaryText);
        }
    }
}