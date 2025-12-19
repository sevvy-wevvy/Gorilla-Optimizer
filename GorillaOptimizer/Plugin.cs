using BepInEx;
using UnityEngine;
using UnityEngine.XR;
using SeveralBees;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace GorillaOptimizer
{
    [BepInPlugin("com.sev.gorillatag.GorillaOptimizer", "GorillaOptimizer", "1.1.0")]
    [BepInDependency("com.Sev.gorillatag.SeveralBees", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        internal string MainPageSbToken = "";

        void Awake()
        {
            Instance = this;
            SeveralBees.Plugin.Instance.Startup.Add(InstanceStartThing);
        }

        [System.Obsolete]
        void InstanceStartThing()
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
                T("UltraLowRenderScale","Ultra Low XR Scale",()=>XRSettings.eyeTextureResolutionScale=0.55f,()=>XRSettings.eyeTextureResolutionScale=1f),

                T("DisableMotionVectors","Disable Motion Vectors",DisableMotionVectors,EnableMotionVectors),
                T("LowPhysicsRate","Low Physics Rate",LowPhysicsRate,NormalPhysicsRate),
                T("CullRendererShadows","Cull Renderer Shadows",CullRendererShadows,RestoreRendererShadows),
                T("DisablePostProcessing","Disable Post Processing",DisablePostProcessing,EnablePostProcessing),
                T("AggressiveGC","Aggressive GC",AggressiveGC,NormalGC),

                T("NoParticles","No Particles",DisableParticles,EnableParticles),
                T("LowParticleCount","Low Particle Count",LowParticleCount,NormalParticleCount),
                T("PauseOffscreenParticles","Pause Offscreen Particles",PauseOffscreenParticles,ResumeOffscreenParticles)
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
            return new string[]
            {
                "LowTextures","NoShadows","LowShadowRes","ZeroShadowDist","NoAA","NoAniso","LowPixelLights",
                "NoSoftParticles","LowLODBias","NoReflections","NoVSync","CapFPS","LowRenderScale","UltraLowRenderScale",
                "DisableMotionVectors","LowPhysicsRate","CullRendererShadows","DisablePostProcessing","AggressiveGC",
                "NoParticles","LowParticleCount","PauseOffscreenParticles"
            };
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

            if (k == "DisableMotionVectors") DisableMotionVectors();
            if (k == "LowPhysicsRate") LowPhysicsRate();
            if (k == "CullRendererShadows") CullRendererShadows();
            if (k == "DisablePostProcessing") DisablePostProcessing();
            if (k == "AggressiveGC") AggressiveGC();

            if (k == "NoParticles") DisableParticles();
            if (k == "LowParticleCount") LowParticleCount();
            if (k == "PauseOffscreenParticles") PauseOffscreenParticles();
        }

        void DisableParticles()
        {
            foreach (var ps in FindObjectsOfType<ParticleSystem>())
            {
                var em = ps.emission;
                em.enabled = false;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var r = ps.GetComponent<ParticleSystemRenderer>();
                if (r != null) r.enabled = false;
            }
        }

        void EnableParticles()
        {
            foreach (var ps in FindObjectsOfType<ParticleSystem>())
            {
                var em = ps.emission;
                em.enabled = true;
                var r = ps.GetComponent<ParticleSystemRenderer>();
                if (r != null) r.enabled = true;
            }
        }

        void LowParticleCount()
        {
            foreach (var ps in FindObjectsOfType<ParticleSystem>())
            {
                var main = ps.main;
                if (main.maxParticles > 32) main.maxParticles = 32;
            }
        }

        void NormalParticleCount()
        {
            foreach (var ps in FindObjectsOfType<ParticleSystem>())
            {
                var main = ps.main;
                main.maxParticles = 1000;
            }
        }

        void PauseOffscreenParticles()
        {
            foreach (var ps in FindObjectsOfType<ParticleSystem>())
            {
                var r = ps.GetComponent<Renderer>();
                if (r != null && !r.isVisible) ps.Pause();
            }
        }

        void ResumeOffscreenParticles()
        {
            foreach (var ps in FindObjectsOfType<ParticleSystem>()) ps.Play();
        }

        void DisableMotionVectors() { foreach (var cam in Camera.allCameras) cam.depthTextureMode = DepthTextureMode.None; }
        void EnableMotionVectors() { foreach (var cam in Camera.allCameras) cam.depthTextureMode = DepthTextureMode.Depth; }

        void LowPhysicsRate()
        {
            Time.fixedDeltaTime = 0.0333f;
            Physics.defaultSolverIterations = 4;
            Physics.defaultSolverVelocityIterations = 1;
        }

        void NormalPhysicsRate()
        {
            Time.fixedDeltaTime = 0.02f;
            Physics.defaultSolverIterations = 6;
            Physics.defaultSolverVelocityIterations = 1;
        }

        void CullRendererShadows() { foreach (var r in FindObjectsOfType<Renderer>()) r.shadowCastingMode = ShadowCastingMode.Off; }
        void RestoreRendererShadows() { foreach (var r in FindObjectsOfType<Renderer>()) r.shadowCastingMode = ShadowCastingMode.On; }

        void DisablePostProcessing()
        {
            foreach (var b in FindObjectsOfType<Behaviour>()) if (b.GetType().Name.Contains("Post")) b.enabled = false;
        }

        void EnablePostProcessing()
        {
            foreach (var b in FindObjectsOfType<Behaviour>()) if (b.GetType().Name.Contains("Post")) b.enabled = true;
        }

        void AggressiveGC()
        {
            System.GC.Collect();
            Application.lowMemory += ForceGC;
        }

        void NormalGC()
        {
            Application.lowMemory -= ForceGC;
        }

        void ForceGC()
        {
            System.GC.Collect();
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
