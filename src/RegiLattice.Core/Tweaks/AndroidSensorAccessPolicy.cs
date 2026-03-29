// RegiLattice.Core — Tweaks/AndroidSensorAccessPolicy.cs
// WSA Android sensor access controls — accelerometer, gyroscope, proximity, fingerprint — Sprint 470.
// Category: "Android Sensor Access Policy" | Slug: wsasnsr
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Sensors

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AndroidSensorAccessPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Sensors";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsasnsr-block-accelerometer",
                Label = "Block Accelerometer Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android applications in WSA from accessing the device accelerometer sensor, preventing motion-based fingerprinting and keystroke inference attacks via accelerometer data.",
                Tags = ["wsa", "android", "accelerometer", "sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Accelerometer blocked for Android apps; motion-based tracking and keystroke inference prevented.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAccelerometer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAccelerometer")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAccelerometer", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-gyroscope",
                Label = "Block Gyroscope Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android applications in WSA from accessing the gyroscope, preventing orientation and rotation tracking that can be used for covert activity inference.",
                Tags = ["wsa", "android", "gyroscope", "sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Gyroscope blocked for Android apps; rotation-based side-channel attacks prevented.",
                ApplyOps = [RegOp.SetDword(Key, "BlockGyroscope", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockGyroscope")],
                DetectOps = [RegOp.CheckDword(Key, "BlockGyroscope", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-proximity-sensor",
                Label = "Block Proximity Sensor Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android applications in WSA from reading the proximity sensor, preventing apps from detecting physical presence near the device for surveillance or power-state manipulation.",
                Tags = ["wsa", "android", "proximity", "sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Proximity sensor blocked for Android apps; presence detection by Android apps prevented.",
                ApplyOps = [RegOp.SetDword(Key, "BlockProximitySensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockProximitySensor")],
                DetectOps = [RegOp.CheckDword(Key, "BlockProximitySensor", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-light-sensor",
                Label = "Block Ambient Light Sensor Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android applications from accessing the ambient light sensor in WSA, preventing light-level side-channel information from being used to infer room or user context.",
                Tags = ["wsa", "android", "light-sensor", "sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Ambient light sensor blocked for Android apps; auto-brightness in WSA apps disabled.",
                ApplyOps = [RegOp.SetDword(Key, "BlockLightSensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockLightSensor")],
                DetectOps = [RegOp.CheckDword(Key, "BlockLightSensor", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-barometer",
                Label = "Block Barometer Sensor Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android applications in WSA from reading barometric pressure data, preventing apps from using pressure data for floor-level location inference.",
                Tags = ["wsa", "android", "barometer", "sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Barometer blocked for Android apps; floor-level location inference via pressure data prevented.",
                ApplyOps = [RegOp.SetDword(Key, "BlockBarometerSensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockBarometerSensor")],
                DetectOps = [RegOp.CheckDword(Key, "BlockBarometerSensor", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-fingerprint-sensor",
                Label = "Block Fingerprint Sensor API for Android Apps in WSA",
                Category = "Android Sensor Access Policy",
                Description =
                    "Prevents Android apps in WSA from accessing the Windows Hello fingerprint hardware via the Android fingerprint API, stopping Android banking apps from using the Windows biometric sensor incorrectly.",
                Tags = ["wsa", "android", "fingerprint", "biometric", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Fingerprint API blocked for Android WSA apps; biometric auth in Android apps falls back to PIN.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidFingerprintSensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidFingerprintSensor")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidFingerprintSensor", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-magnetometer",
                Label = "Block Magnetometer/Compass Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android apps in WSA from accessing the magnetometer/digital compass sensor, preventing directional tracking and compass-based location correlation.",
                Tags = ["wsa", "android", "magnetometer", "compass", "sensor", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Magnetometer blocked for Android apps; compass navigation and direction-based apps disabled.",
                ApplyOps = [RegOp.SetDword(Key, "BlockMagnetometerSensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMagnetometerSensor")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMagnetometerSensor", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-block-temperature-sensor",
                Label = "Block Temperature Sensor Access for Android Apps",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android apps in WSA from reading CPU or board temperature sensors, preventing apps from using thermal data to infer workload patterns or detect sandboxed execution.",
                Tags = ["wsa", "android", "temperature", "sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Temperature sensor blocked; Android apps cannot infer CPU workload via thermal readings.",
                ApplyOps = [RegOp.SetDword(Key, "BlockTemperatureSensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockTemperatureSensor")],
                DetectOps = [RegOp.CheckDword(Key, "BlockTemperatureSensor", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-disable-sensor-fusion",
                Label = "Disable Android Sensor Fusion in WSA",
                Category = "Android Sensor Access Policy",
                Description =
                    "Disables the Android sensor fusion layer in WSA that aggregates multiple sensor streams into virtual sensors (rotation vector, gravity, linear acceleration), reducing composite tracking attack surface.",
                Tags = ["wsa", "android", "sensor-fusion", "virtual-sensor", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sensor fusion disabled; Android virtual sensor APIs (rotation vector, gravity) return empty data.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSensorFusion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSensorFusion")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSensorFusion", 1)],
            },
            new TweakDef
            {
                Id = "wsasnsr-disable-step-counter",
                Label = "Disable Step Counter Sensor for Android Apps in WSA",
                Category = "Android Sensor Access Policy",
                Description =
                    "Blocks Android fitness and health apps in WSA from accessing the step counter hardware sensor, preventing pedometer-based location tracking and activity inference.",
                Tags = ["wsa", "android", "step-counter", "fitness", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Step counter blocked for Android apps; fitness tracking apps in WSA will not count steps.",
                ApplyOps = [RegOp.SetDword(Key, "DisableStepCounterSensor", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStepCounterSensor")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStepCounterSensor", 1)],
            },
        ];
}
