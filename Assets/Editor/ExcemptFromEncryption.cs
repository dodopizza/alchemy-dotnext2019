using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class ExcemptFromEncryption : IPostprocessBuildWithReport // Will execute after XCode project is built
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("ExcemptFromEncryption");
        if (report.summary.platform == BuildTarget.iOS) // Check if the build is for iOS 
        {
            var plistPath = report.summary.outputPath + "/Info.plist"; 

            var plist = new PlistDocument(); // Read Info.plist file into memory
            plist.ReadFromString(File.ReadAllText(plistPath));

            var rootDict = plist.root;
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false); 

            File.WriteAllText(plistPath, plist.WriteToString()); // Override Info.plist
        }
    }
}