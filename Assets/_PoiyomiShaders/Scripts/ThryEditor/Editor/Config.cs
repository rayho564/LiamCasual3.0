// Material/Shader Inspector for Unity 2017/2018
// Copyright (C) 2019 Thryrallo

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Thry
{
    public class Config
    {
        // consts
        private const string PATH_CONFIG_FILE = "Thry/Config.json";
        private const string VERSION = "1.7.1";

        // static
        private static Config config;

        public static void OnCompile()
        {
            if (!File.Exists(PATH_CONFIG_FILE))
            {
                //Settings.firstTimePopup();
            }
            else
            {
                string prevVersion = Get().verion;
                string installedVersion = VERSION;
                int versionComparision = Helper.compareVersions(installedVersion, prevVersion);
                if (versionComparision != 0)
                {
                    config.verion = VERSION;
                    config.save();
                }
                if (versionComparision == 1)
                {
                    Settings.updatedPopup(versionComparision);
                }
                else if (versionComparision == -1)
                {
                    config.OnUpgrade(prevVersion);
                    Debug.Log(">>> Thry Editor has been updated to version " + installedVersion);
                }
            }
        }

        //load the config from file
        private static Config LoadConfig()
        {
            if (File.Exists(PATH_CONFIG_FILE))
                return JsonUtility.FromJson<Config>(FileHelper.ReadFileIntoString(PATH_CONFIG_FILE));
            new Config().save();
            return new Config();
        }

        public static Config Get()
        {
            if (config == null) config = LoadConfig();
            return config;
        }

        //actual config class
        public TextureDisplayType default_texture_type = TextureDisplayType.small;
        public bool showRenderQueue = true;
        public bool renderQueueShaders = false;

        public string locale = "English";

        public string gradient_name = "gradient_<hash>.png";

        public bool restore_materials = true;
        public string verion = VERSION;

        public bool share_user_data = false;
        public bool share_installed_unity_version = true;
        public bool share_installed_editor_version = true;
        public bool share_used_shaders = true;

        public void save()
        {
            FileHelper.WriteStringToFile(JsonUtility.ToJson(this), PATH_CONFIG_FILE);
        }

        private void OnUpgrade(string oldVersion)
        {
            if (Helper.compareVersions(oldVersion, "1.4.0") < 1)
            {
                renderQueueShaders = false;
                save();
            }
        }
    }
}
