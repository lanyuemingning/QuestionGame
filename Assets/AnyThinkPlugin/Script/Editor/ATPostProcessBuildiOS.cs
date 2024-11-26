#if UNITY_IOS || UNITY_IPHONE

using AnyThink.Scripts.IntegrationManager.Editor;
#if UNITY_2019_3_OR_NEWER
using UnityEditor.iOS.Xcode.Extensions;
#endif
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace AnyThink.Scripts.Editor
{
    [Serializable]
    public class SkAdNetworkData
    {
        [SerializeField] public string[] SkAdNetworkIds;
    }

    public class TopOnPostProcessBuildiOS
    {

        private static readonly List<string> AtsRequiringNetworks = new List<string>
        {
            "AdColony",
            "ByteDance",
            "Fyber",
            "Google",
            "GoogleAdManager",
            "HyprMX",
            "InMobi",
            "IronSource",
            "Smaato"
        };

        private static List<string> DynamicLibraryPathsToEmbed
        {
            get
            {
                var dynamicLibraryPathsToEmbed = new List<string>();
                dynamicLibraryPathsToEmbed.Add(Path.Combine("Pods/", "KSAdSDK/KSAdSDK.xcframework"));
                dynamicLibraryPathsToEmbed.Add(Path.Combine("Pods/", "StartAppSDK/StartApp.xcframework"));
                dynamicLibraryPathsToEmbed.Add(Path.Combine("Pods/", "BigoADS/BigoADS/BigoADS.xcframework"));
                dynamicLibraryPathsToEmbed.Add(Path.Combine("Pods/", "BigoADS/BigoADS/OMSDK_Bigosg.xcframework"));
                dynamicLibraryPathsToEmbed.Add(Path.Combine("Pods/", "HyBid/PubnativeLite/PubnativeLite/OMSDK-1.3.29/OMSDK_Pubnativenet.xcframework"));
                dynamicLibraryPathsToEmbed.Add(Path.Combine("Pods/", "Fyber_Marketplace_SDK/IASDKCore/IASDKCore.xcframework"));


                return dynamicLibraryPathsToEmbed;
            }
        }

        private static List<string> BunldePathsToAdd {
            get {

                var bunldePathsToAdd = new List<string>();
                bunldePathsToAdd.Add(Path.Combine("Pods/", "BigoADS/BigoADS/BigoADSRes.bundle"));

                return bunldePathsToAdd;
            }
        }

        private static readonly List<string> SwiftLanguageNetworks = new List<string>
        {
            "MoPub"
        };

        private static readonly List<string> EmbedSwiftStandardLibrariesNetworks = new List<string>
        {
            "Facebook",
            "MoPub"
        };

        [PostProcessBuildAttribute(int.MaxValue)]
        public static void TopOnPostProcessPbxProject(BuildTarget buildTarget, string buildPath)
        {
            var projectPath = PBXProject.GetPBXProjectPath(buildPath);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
            var unityMainTargetGuid = project.GetUnityMainTargetGuid();
            var unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();
#else
            var unityMainTargetGuid = project.TargetGuidByName(UnityMainTargetName);
            var unityFrameworkTargetGuid = project.TargetGuidByName(UnityMainTargetName);
#endif

            project.SetBuildProperty(unityMainTargetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            project.SetBuildProperty(unityMainTargetGuid, "ENABLE_BITCODE", "NO");

            project.SetBuildProperty(unityFrameworkTargetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            project.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");

            EmbedDynamicLibrariesIfNeeded(buildPath, project, unityMainTargetGuid);
            AddBunleIfNeeded(buildPath, project, unityMainTargetGuid);

            project.WriteToFile(projectPath);
        }

        [PostProcessBuildAttribute(int.MaxValue)]
        public static void TopOnPostProcessPlist(BuildTarget buildTarget, string path)
        {
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

#if UNITY_2018_2_OR_NEWER
            AddGoogleApplicationIdIfNeeded(plist);
#endif

            plist.WriteToFile(plistPath);
        }

        private static void AddBunleIfNeeded(string buildPath, PBXProject project, string targetGuid)
        {
            var bunldePathsPresentInProject = BunldePathsToAdd.Where(bunldePath => Directory.Exists(Path.Combine(buildPath, bunldePath))).ToList();
            if (bunldePathsPresentInProject.Count <= 0) return;
            ATLog.log("AddBunleIfNeeded");

#if UNITY_2019_3_OR_NEWER
            foreach (var bunldePath in bunldePathsPresentInProject)
            {
                var fileGuid = project.AddFile(bunldePath, bunldePath, PBXSourceTree.Source);
                project.AddFileToBuild(targetGuid, fileGuid);
            }
#endif
        }

        private static void EmbedDynamicLibrariesIfNeeded(string buildPath, PBXProject project, string targetGuid)
        {
            var dynamicLibraryPathsPresentInProject = DynamicLibraryPathsToEmbed.Where(dynamicLibraryPath => Directory.Exists(Path.Combine(buildPath, dynamicLibraryPath))).ToList();
            if (dynamicLibraryPathsPresentInProject.Count <= 0) return;

#if UNITY_2019_3_OR_NEWER
            foreach (var dynamicLibraryPath in dynamicLibraryPathsPresentInProject)
            {
                var fileGuid = project.AddFile(dynamicLibraryPath, dynamicLibraryPath);
                project.AddFileToEmbedFrameworks(targetGuid, fileGuid);
            }
#else
            string runpathSearchPaths;
#if UNITY_2018_2_OR_NEWER
            runpathSearchPaths = project.GetBuildPropertyForAnyConfig(targetGuid, "LD_RUNPATH_SEARCH_PATHS");
#else
            runpathSearchPaths = "$(inherited)";          
#endif
            runpathSearchPaths += string.IsNullOrEmpty(runpathSearchPaths) ? "" : " ";

            // Check if runtime search paths already contains the required search paths for dynamic libraries.
            if (runpathSearchPaths.Contains("@executable_path/Frameworks")) return;

            runpathSearchPaths += "@executable_path/Frameworks";
            project.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", runpathSearchPaths);
#endif
        }

#if UNITY_2018_2_OR_NEWER

        private static void AddGoogleApplicationIdIfNeeded(PlistDocument plist)
        {
            if (!ATConfig.isNetworkInstalledByName("Admob", ATConfig.OS_IOS))
            {   
                ATLog.log("addGoogleApplicationIdIfNeeded() >>> Admob not install.");
                return;
            }
            //获取admob app id
            var appId = ATConfig.getAdmobAppIdByOs(ATConfig.OS_IOS);

            if (string.IsNullOrEmpty(appId) || !appId.StartsWith("ca-app-pub-"))
            {
                ATLog.logError("AdMob App ID is not set. Please enter a valid app ID within the AnyThink Integration Manager window.");
                return;
            }

            const string googleApplicationIdentifier = "GADApplicationIdentifier";
            plist.root.SetString(googleApplicationIdentifier, appId);
        }
#endif
    }
}
#endif
