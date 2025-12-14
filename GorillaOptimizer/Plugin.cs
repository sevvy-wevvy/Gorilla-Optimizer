using BepInEx;
using UnityEngine;
using UnityEngine.XR;
using SeveralBees;
using System.Collections.Generic;

namespace GorillaOptimizer
{
    [BepInPlugin("com.sev.gorillatag.GorillaOptimizer", "GorillaOptimizer", "1.0.0")]
    [BepInDependency("com.Sev.gorillatag.SeveralBees", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        internal string MainPageSbToken = "";

        void Awake()
        {
            Instance = this;
        }

        [System.Obsolete]
        void Start()
        {
            MainPageSbToken = Api.Instance.GenerateToken("<color=yellow>Gorilla Optimizer</color>", true, "Main");

            Api.Instance.SetButtonInfo(MainPageSbToken, new List<ModButtonInfo>
            {
                T("LowTextures","Low Textures",()=>QualitySettings.masterTextureLimit=3,()=>QualitySettings.masterTextureLimit=0),
                T("NoShadows","No Shadows",()=>QualitySettings.shadows=ShadowQuality.Disable,()=>QualitySettings.shadows=ShadowQuality.All),
                T("LowShadowRes","Low Shadow Res",()=>QualitySettings.shadowResolution=ShadowResolution.Low,()=>QualitySettings.shadowResolution=ShadowResolution.High),
                T("ZeroShadowDist","Zero Shadow Dist",()=>QualitySettings.shadowDistance=0f,()=>QualitySettings.shadowDistance=50f),
                T("NoAA","No AntiAliasing",()=>QualitySettings.antiAliasing=0,()=>QualitySettings.antiAliasing=4),
                T("NoAniso","No Anisotropic",()=>Texture.anisotropicFiltering=AnisotropicFiltering.Disable,()=>Texture.anisotropicFiltering=AnisotropicFiltering.ForceEnable),
                T("LowPixelLights","Zero Pixel Lights",()=>QualitySettings.pixelLightCount=0,()=>QualitySettings.pixelLightCount=4),
                T("NoSoftParticles","No Soft Particles",()=>QualitySettings.softParticles=false,()=>QualitySettings.softParticles=true),
                T("LowLODBias","Low LOD Bias",()=>QualitySettings.lodBias=0.25f,()=>QualitySettings.lodBias=1f),
                T("NoReflections","No Reflection Probes",()=>QualitySettings.realtimeReflectionProbes=false,()=>QualitySettings.realtimeReflectionProbes=true),
                T("NoVSync","No VSync",()=>QualitySettings.vSyncCount=0,()=>QualitySettings.vSyncCount=1),
                T("CapFPS","Cap FPS 60",()=>{Application.targetFrameRate=60;QualitySettings.vSyncCount=0;},()=>Application.targetFrameRate=-1),
                T("LowRenderScale","Low XR Scale",()=>XRSettings.eyeTextureResolutionScale=0.7f,()=>XRSettings.eyeTextureResolutionScale=1f),
                T("UltraLowRenderScale","Ultra Low XR Scale",()=>XRSettings.eyeTextureResolutionScale=0.55f,()=>XRSettings.eyeTextureResolutionScale=1f)
            });

            ApplyAll();
        }

        ModButtonInfo T(string key, string text, System.Action onEnable, System.Action onDisable)
        {
            return new ModButtonInfo
            {
                enabled = GetEnabled(key, false),
                buttonText = text,
                isTogglable = true,
                enableMethod = () => { SaveEnabled(true, key); onEnable(); },
                disableMethod = () => { SaveEnabled(false, key); onDisable(); }
            };
        }

        void ApplyAll()
        {
            foreach (var kv in PlayerPrefsKeys()) if (GetEnabled(kv)) ApplyKey(kv);
        }

        IEnumerable<string> PlayerPrefsKeys()
        {
            return new string[] { "LowTextures", "NoShadows", "LowShadowRes", "ZeroShadowDist", "NoAA", "NoAniso", "LowPixelLights", "NoSoftParticles", "LowLODBias", "NoReflections", "NoVSync", "CapFPS", "LowRenderScale", "UltraLowRenderScale" };
        }

        void ApplyKey(string k)
        {
            if (k == "LowTextures") QualitySettings.masterTextureLimit = 3;
            if (k == "NoShadows") QualitySettings.shadows = ShadowQuality.Disable;
            if (k == "LowShadowRes") QualitySettings.shadowResolution = ShadowResolution.Low;
            if (k == "ZeroShadowDist") QualitySettings.shadowDistance = 0f;
            if (k == "NoAA") QualitySettings.antiAliasing = 0;
            if (k == "NoAniso") Texture.anisotropicFiltering = AnisotropicFiltering.Disable;
            if (k == "LowPixelLights") QualitySettings.pixelLightCount = 0;
            if (k == "NoSoftParticles") QualitySettings.softParticles = false;
            if (k == "LowLODBias") QualitySettings.lodBias = 0.25f;
            if (k == "NoReflections") QualitySettings.realtimeReflectionProbes = false;
            if (k == "NoVSync") QualitySettings.vSyncCount = 0;
            if (k == "CapFPS") { Application.targetFrameRate = 60; QualitySettings.vSyncCount = 0; }
            if (k == "LowRenderScale") XRSettings.eyeTextureResolutionScale = 0.7f;
            if (k == "UltraLowRenderScale") XRSettings.eyeTextureResolutionScale = 0.55f;
        }

        internal void SaveEnabled(bool e, string k)
        {
            PlayerPrefs.SetInt("GorillaOptimizer-" + k, e ? 1 : 0);
        }

        internal bool GetEnabled(string k, bool d = false)
        {
            return PlayerPrefs.GetInt("GorillaOptimizer-" + k, d ? 1 : 0) == 1;
        }
    }
}