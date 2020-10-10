using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CC
{
    /// <summary>
    /// 音频播放器类型
    /// </summary>
    public enum AudioType
    {
        SOUND = 0,
        MUSIC = 1
    }

    public enum AudioName
    {
        BACK,
        FORE,
        GUI,
        FUI,
        USER,
        FUSER,
        SCENE,
        NPC,
        MONSTER
    }

    public struct AudioAttribute
    {
        public string name;
        public AudioType type;

        public AudioAttribute(string name = "DefaultAudioPlayer", AudioType type = AudioType.SOUND)
        {
            this.name = name;
            this.type = type;
        }
    }

    public static class AudioSystem
    {
        public class AudioPlayer
        {
            public readonly GameObject node;
            public readonly AudioSource audio;
            public readonly AudioType audioType;

            public AudioPlayer(AudioAttribute audioAttribute)
            {
                node = new GameObject($"{audioAttribute.name}_AudioPlayer");
                node.transform.SetParent(helper.transform);
                audio = node.AddComponent<AudioSource>();
                audioType = audioAttribute.type;
                audio.playOnAwake = false;
            }
        }

        private static bool muteMusic;
        private static bool muteSound;
        private static float globalMusicVolume;
        private static float globalSoundVolume = 0;
        private static GameObject helper;
        private static Dictionary<string, AudioPlayer> audios;
        private const string AudioPath = "Music/";

        static AudioSystem()
        {
            Debug.Log("AudioSystem Awake");
            helper = new GameObject("AudioHelper");
            Object.DontDestroyOnLoad(helper);
            audios = new Dictionary<string, AudioPlayer>();
            CreateDefaultAudios();
            InitAudioState();
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
        }

        /// <summary>
        /// 初始化音频状态
        /// </summary>
        private static void InitAudioState()
        {
            bool soundState = LoadAudioState(AudioType.SOUND) == 1;
            bool musicState = LoadAudioState(AudioType.MUSIC) == 1;

            SetAudioState(AudioType.SOUND, soundState);
            SetAudioState(AudioType.MUSIC, musicState);
        }

        /// <summary>
        /// 创建初始音频组
        /// </summary>
        public static void CreateDefaultAudios()
        {
            int i;

            for (i = 0; i < CCConfig.DefaultAudios.Length; i++)
            {
                AddAudioPlayer(CCConfig.DefaultAudios[i], CCConfig.DefaultAudioTypes[i].IntToEnum<AudioType>());
            }
        }

        #region 音频状态管理

        /// <summary>
        /// 切换音频状态
        /// </summary>
        /// <param name="audioType"></param>
        /// <returns></returns>
        public static bool SwitchAudioState(AudioType audioType)
        {
            var state = audioType == AudioType.MUSIC ? !muteMusic : !muteSound;

            SetAudioState(audioType, state);
            SetGlobalMute(audioType, state);
            SaveAudioState(audioType);
            return state;
        }

        /// <summary>
        /// 获取音频播放器静音状态
        /// </summary>
        /// <returns></returns>
        public static bool GetAudioState(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.SOUND:
                    return muteSound;
                case AudioType.MUSIC:
                    return muteMusic;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 设置音频状态
        /// </summary>
        /// <param name="audioType">Audio type.</param>
        /// <param name="state">State.</param>
        private static void SetAudioState(AudioType audioType, bool state)
        {
            switch (audioType)
            {
                case AudioType.SOUND:
                    muteSound = state;
                    break;
                case AudioType.MUSIC:
                    muteMusic = state;
                    break;
            }
        }

        /// <summary>
        /// 存储音频状态设置
        /// </summary>
        /// <param name="audioType">Audio type.</param>
        private static void SaveAudioState(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.SOUND:
                    PlayerPrefs.SetInt(CCConfig.KEY_FOR_SOUND, muteSound ? 1 : 0);
                    break;
                case AudioType.MUSIC:
                    PlayerPrefs.SetInt(CCConfig.KEY_FOR_MUSIC, muteMusic ? 1 : 0);
                    break;
            }

            PlayerPrefs.Save();
        }

        /// <summary>
        /// 音频播放器静音参数设置
        /// </summary>
        /// <param name="audioType">Audio type.</param>
        /// <param name="state">If set to <c>true</c> state.</param>
        private static void SetGlobalMute(AudioType audioType, bool state)
        {
            if (audios.Count == 0) return;
            foreach (KeyValuePair<string, AudioPlayer> pair in audios)
            {
                if (pair.Value.audioType != audioType) continue;
                pair.Value.audio.mute = state;
            }
        }

        /**
         * 设置全局音量大小
         * @param type
         * @param volume
         */
        private static void SetGlobalVolume(AudioType audioType, float volume)
        {
            switch (audioType)
            {
                case AudioType.SOUND:
                    if (Math.Abs(globalSoundVolume - volume) <= 0) break;
                    globalMusicVolume = volume;
                    SetAllAudioVolume(audioType, volume);
                    break;
                case AudioType.MUSIC:
                    if (Math.Abs(globalMusicVolume - volume) <= 0) break;
                    globalMusicVolume = volume;
                    SetAllAudioVolume(audioType, volume);
                    break;
            }
        }

        /// <summary>
        /// 设置所有音频音量
        /// </summary>
        /// <param name="audioType"></param>
        /// <param name="volume"></param>
        private static void SetAllAudioVolume(AudioType audioType, float volume)
        {
            if (audios.Count == 0) return;
            foreach (KeyValuePair<string, AudioPlayer> pair in audios)
            {
                if (pair.Value.audioType != audioType) continue;
                pair.Value.audio.volume = volume;
            }
        }

        /// <summary>
        /// 获取真实音量
        /// </summary>
        /// <param name="audioType"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        private static float GetRealVolume(AudioType audioType, float volume)
        {
            switch (audioType)
            {
                case AudioType.SOUND:
                    volume = globalSoundVolume * volume;
                    break;
                case AudioType.MUSIC:
                    volume = globalMusicVolume * volume;
                    break;
            }

            return volume;
        }

        /// <summary>
        /// 读取音频状态设置
        /// </summary>
        /// <returns>The audio state.</returns>
        /// <param name="audioType">Audio type.</param>
        private static int LoadAudioState(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.SOUND:
                    return PlayerPrefs.GetInt(CCConfig.KEY_FOR_SOUND);
                case AudioType.MUSIC:
                    return PlayerPrefs.GetInt(CCConfig.KEY_FOR_MUSIC);
                default:
                    return 0;
            }
        }

        /**
         * 加载音频资源
         * @param {String} clipName
         * @param {Function} callback
         */
        private static AudioClip LoadAudioRes(string clipName)
        {
            return Resources.Load<AudioClip>(AudioPath + clipName);
        }

        #endregion

        #region 音频播放器管理

        /// <summary>
        /// 添加音频播放器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="audioType"></param>
        /// <returns> AudioPlayer</returns>
        public static AudioPlayer AddAudioPlayer(String name, AudioType audioType = AudioType.SOUND)
        {
            if (audios.ContainsKey(name))
            {
                Debug.Log(string.Format("{0}播放器已存在，无需重复添加！", name));
                return audios[name];
            }

            AudioPlayer audioPlayer = new AudioPlayer(new AudioAttribute(name, audioType));
            audios.Add(name, audioPlayer);
            return audioPlayer;
        }

        /// <summary>
        /// 添加音频播放器
        /// </summary>
        /// <param name="audioAttribute"></param>
        /// <returns></returns>
        public static AudioPlayer AddAudioPlayer(AudioAttribute audioAttribute)
        {
            if (audios.ContainsKey(audioAttribute.name))
            {
                Debug.Log($"{audioAttribute.name}播放器已存在，无需重复添加！");
                return audios[audioAttribute.name];
            }

            AudioPlayer audioPlayer = new AudioPlayer(audioAttribute);
            audios.Add(audioAttribute.name, audioPlayer);
            return audioPlayer;
        }

        /// <summary>
        /// 移除音频播放器
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveAudioPlayer(string name)
        {
            AudioPlayer audioPlayer = audios[name];
            audios.Remove(name);
            Object.Destroy(audioPlayer.node);
        }

        /// <summary>
        /// 音频播放器是否静音
        /// </summary>
        /// <param name="audioPlayer"></param>
        /// <returns></returns>
        public static bool IsMute(AudioPlayer audioPlayer)
        {
            return (muteMusic && audioPlayer.audioType == AudioType.MUSIC) ||
                   (muteSound && audioPlayer.audioType == AudioType.SOUND);
        }

        /// <summary>
        /// 音频播放(可响应音乐/音效开关控制),音频从路径载入
        /// </summary>
        /// <param name="audioPlayerName"></param>
        /// <param name="clipPath"></param>
        /// <param name="loop"></param>
        /// <param name="volume"></param>
        public static void Play(string audioPlayerName, string clipPath, bool loop = false, float volume = 1.0f)
        {
            AudioPlayer audioPlayer = audios[audioPlayerName];

            audioPlayer.audio.mute = IsMute(audioPlayer);
            audioPlayer.audio.loop = loop;

            if (!audioPlayer.audio.isPlaying || audioPlayer.audio.loop == loop)
            {
                audioPlayer.audio.Stop();
                audioPlayer.audio.clip = null;
            }

            audioPlayer.audio.clip = LoadAudioRes(clipPath);
            audioPlayer.audio.volume = volume;
            audioPlayer.audio.Play();
            audioPlayer.audio.PlayOneShot(LoadAudioRes(clipPath), volume);
        }

        /// <summary>
        /// 音频播放(可响应音乐/音效开关控制),需指定音频类型文件
        /// </summary>
        /// <param name="audioPlayerName"></param>
        /// <param name="clip"></param>
        /// <param name="loop"></param>
        /// <param name="volume"></param>
        public static void Play(string audioPlayerName, AudioClip clip, bool loop = false, float volume = 1.0f)
        {
            AudioPlayer audioPlayer = audios[audioPlayerName];

            audioPlayer.audio.mute = IsMute(audioPlayer);
            audioPlayer.audio.loop = loop;

            if (!audioPlayer.audio.isPlaying || audioPlayer.audio.loop != loop)
            {
                audioPlayer.audio.clip = clip;
                audioPlayer.audio.volume = volume;
                audioPlayer.audio.Play();
            }
            else audioPlayer.audio.PlayOneShot(clip, volume);
        }

        /// <summary>
        /// 开启静音
        /// </summary>
        /// <param name="audioName"></param>
        private static void MuteOn(string audioName)
        {
            if (!audios.ContainsKey(audioName)) throw new Exception(audioName + "不存在");
            audios[audioName].audio.mute = true;
        }

        /// <summary>
        /// 关闭静音
        /// </summary>
        /// <param name="audioName"></param>
        private static void MuteOff(string audioName)
        {
            if (!audios.ContainsKey(audioName)) throw new Exception(audioName + "不存在");
            audios[audioName].audio.mute = false;
        }

        /// <summary>
        /// 继续播放音频播放器
        /// </summary>
        /// <param name="audioPlayerName"></param>
        public static void Resume(string audioPlayerName)
        {
            var audioPlayer = audios[audioPlayerName];
            audioPlayer.audio.UnPause();
        }

        /// <summary>
        /// 停止音频播放器
        /// </summary>
        /// <param name="audioPlayerName"></param>
        public static void Stop(string audioPlayerName)
        {
            AudioPlayer audioPlayer = audios[audioPlayerName];
            audioPlayer.audio.Stop();
        }

        /// <summary>
        /// 暂停音频播放器
        /// </summary>
        /// <param name="audioPlayerName"></param>
        public static void Pause(string audioPlayerName)
        {
            AudioPlayer audioPlayer = audios[audioPlayerName];
            audioPlayer.audio.Pause();
        }

        /// <summary>
        /// 获取音频播放器上的AudioSource组件
        /// </summary>
        /// <param name="audioPlayerName"></param>
        /// <returns></returns>
        public static AudioSource GetAudioSource(string audioPlayerName)
        {
            if (audios.TryGetValue(audioPlayerName, out var audioPlayer)) return audioPlayer.audio;
            throw new Exception($"没有这个{audioPlayerName}播放器");
        }

        /// <summary>
        /// 清空音频管理器下的所有音频播放器
        /// </summary>
        public static void Clear()
        {
            if (audios.Count == 0) return;
            foreach (var pair in audios)
            {
                Object.Destroy(pair.Value.node);
                audios.Remove(pair.Key);
            }
        }

        #endregion
    }
}