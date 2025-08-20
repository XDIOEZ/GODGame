using System;
using System.Threading;
using TapTap.TapAd;
using TapTap.TapAd.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Threading.Tasks;
using static TapAdDemo;

public class TapADN : MonoBehaviour
{
    // banner 竖屏id
    private const int PORTRAIT_BANNER_ID = 1048214;
    // banner 横屏id
    private const int LANDSCAPE_BANNER_ID = 1048214;
    // rewardVideo 竖屏id
    private const int PORTRAIT_REWARD_ID = 1048214;
    // rewardVideo 横屏id
    private const int LANDSCAPE_REWARD_ID = 1048214;
    // splash 竖屏id
    private const int PORTRAIT_SPLASH_ID = 1048214;
    // splash 横屏id
    private const int LANDSCAPE_SPLASH_ID = 1048214;

    // interstitial 竖屏id
    private const int PORTRAIT_INTERSTITIAL_ID = 1048214;
    // interstitial 横屏id
    private const int LANDSCAPE_INTERSTITIAL_ID = 1048214;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private InputField bannerBaselineInput;
    [SerializeField]
    private InputField bannerOffsetInput;

    [SerializeField]
    private Button LoadVideo;
    [SerializeField]
    private Button playVideo;

    private TapRewardVideoAd _tapRewardAd;
    private TapBannerAd _tapBannerAd;
    private TapSplashAd _tapSplashAd;
    private TapInterstitialAd _tapInterstitialAd;

    // Unity 主线程ID:
    private static int mainThreadId;



    //回调函数
    public delegate void SimpleCallback();

    private void Awake()
    {
        mainThreadId = Thread.CurrentThread.ManagedThreadId;


        LoadVideo.onClick.RemoveAllListeners();
        LoadVideo.onClick.AddListener(() => LoadRewardVideoAd(true));
        playVideo.onClick.RemoveAllListeners();
        playVideo.onClick.AddListener(PlayRewardVideoAd);
    }

    private void Start()
    {
        SideInit();
    }
    private void ShowText(string content)
    {
        content = string.Format($"[Unity:TapAd] {content} | Time: {DateTime.Now.ToString("g")}");
        infoText.text = content;
        Debug.LogFormat(content);
    }

    public void SideInit()
    {
        Init(RequestPermission);
    }

    private void Init(SimpleCallback callback)
    {
        TapAdConfig config = null;
        ICustomController customController = null;

        config = new TapAdConfig.Builder()
            .MediaId(1013079)
            .MediaName("OnlyFly")
            .MediaKey("J2fMX54uxbfrYXRCApdMkrTjmPOjIVYbWm8GEqEjwnisiQzUJmBbUL3Rk70x5mDf")
            .EnableDebugLog(true)
            .MediaVersion("1")
            .Channel("taptap2")
            .TapClientId("j6qukxgc7zcndtzs7a")
            .Build();
        customController = new FormalCustomControllerWrapper(this);
        Debug.Log(customController);
        TapAdSdk.Init(config, customController);
        Debug.Log(TapAdSdk.IsInited);
        ShowText("初始化完毕");
        callback();
    }



    /// <summary>
    /// 请求用户各项权限
    /// </summary>
    private void RequestPermission()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        TapAdSdk.RequestPermissionIfNecessary();
        ShowText("请求权限");
    }

    /// <summary>
    /// 加载video广告——激励视频
    /// </summary>
    /// <param name="horizontal"></param>
    private async void LoadRewardVideoAd(bool horizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapRewardAd != null)
        {
            _tapRewardAd.Dispose();
            _tapRewardAd = null;
        }
        int adId = horizontal ? LANDSCAPE_REWARD_ID : PORTRAIT_REWARD_ID;
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .Extra1("{}")
            .RewardName("测试视频")
            .RewardCount(9)
            .UserId("123")
            .Build();
        _tapRewardAd = new TapRewardVideoAd(request);
        // 创建监听器并订阅"可播放"事件
        var loadListener = new RewardVideoAdLoadListener(this);
        loadListener.OnAdReadyToPlay += (ad) =>
        {
            // 广告加载并缓存完成后，自动播放
            PlayRewardVideoAd(); // 直接调用播放方法
        };
        _tapRewardAd.SetLoadListener(loadListener);
        _tapRewardAd.Load();
    }
    /// <summary>
    /// 播放video广告——激励视频
    /// </summary>
    private void PlayRewardVideoAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapRewardAd != null)
        {
            _tapRewardAd.SetInteractionListener(new RewardVideoInteractionListener(this));
            _tapRewardAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    /// <summary>
    /// 加载video广告——Banner视频
    /// </summary>
    /// <param name="isHorizontal"></param>
    private async void LoadBannerAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapBannerAd != null)
        {
            _tapBannerAd.Dispose();
            _tapBannerAd = null;
        }

        int adId = isHorizontal ? LANDSCAPE_BANNER_ID : PORTRAIT_BANNER_ID;

        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .UserId("123")
            .Build();
        _tapBannerAd = new TapBannerAd(request);
        // 创建监听器并订阅"可播放"事件
        var loadListener = new BannerAdLoadListener(this);
        loadListener.OnAdReadyToPlay += (ad) =>
        {
            // 广告加载并缓存完成后，自动播放
            PlayRewardVideoAd(); // 直接调用播放方法
        };
        _tapBannerAd.SetLoadListener(new BannerAdLoadListener(this));
        _tapBannerAd.Load();
    }
    /// <summary>
    /// 播放video广告——Banner视频
    /// </summary>
    private void PlayBannerAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapBannerAd != null)
        {
            int baseLine = 0;
            int offset = 0;
            int.TryParse(bannerBaselineInput.text, out baseLine);
            if (baseLine != 0 && baseLine != 1)
            {
                Debug.LogErrorFormat("Banner Baseline 只能设置成 0 或者 1");
            }
            int.TryParse(bannerOffsetInput.text, out offset);
            _tapBannerAd.baseline = baseLine;
            _tapBannerAd.offset = offset;
            _tapBannerAd.SetInteractionListener(new BannerInteractionListener(this));
            _tapBannerAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    /// <summary>
    /// 加载video广告——Splash视频
    /// </summary>
    /// <param name="isHorizontal"></param>
    private async void LoadSplashAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapSplashAd != null)
        {
            _tapSplashAd.Dispose();
            _tapSplashAd = null;
        }
        int adId = isHorizontal ? LANDSCAPE_SPLASH_ID : PORTRAIT_SPLASH_ID;
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .Extra1("{}")
            .UserId("123")
            .Build();
        _tapSplashAd = new TapSplashAd(request);
        _tapSplashAd.SetLoadListener(new SplashAdLoadListener(this));
        _tapSplashAd.Load();
    }

    /// <summary>
    /// 播放video广告——Splash视频
    /// </summary>
    private void PlaySplashAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapSplashAd != null)
        {
            _tapSplashAd.SetInteractionListener(new SplashInteractionListener(this));
            _tapSplashAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    /// <summary>
    /// 加载video广告——interstitial视频
    /// </summary>
    /// <param name="isHorizontal"></param>
    private async void LoadInterstitialAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapInterstitialAd != null)
        {
            _tapInterstitialAd.Dispose();
            _tapInterstitialAd = null;
        }

        int adId = isHorizontal ? LANDSCAPE_INTERSTITIAL_ID : PORTRAIT_INTERSTITIAL_ID;
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .Extra1("{}")
            .UserId("123")
            .Build();
        _tapInterstitialAd = new TapInterstitialAd(request);
        _tapInterstitialAd.SetLoadListener(new InterstitialAdLoadListener(this));
        _tapInterstitialAd.Load();
    }
    /// <summary>
    /// 播放video广告——interstitial视频
    /// </summary>
    private void PlayInterstitialAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapInterstitialAd != null)
        {
            _tapInterstitialAd.SetInteractionListener(new InterstitialAdInteractionListener(this));
            _tapInterstitialAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    /// <summary>
    /// 上报用户行为——用于观察用户点击广告等反馈
    /// </summary>
    private void OnClickUserActionButton()
    {
        var userActions = new UserAction[3];

        var jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        for (int i = 0; i < 3; i++)
        {
            var tmp = new UserAction(actionType: i, actionTime: (long)(DateTime.UtcNow - jan1st1970).TotalMilliseconds,
                amount: i * 1000, winStatus: i % 2);
            userActions[i] = tmp;
        }
        TapAdSdk.UploadUserAction(userActions, new CustomUserAction(this));
    }


    /// <summary>
    /// 用户行为内容封装
    /// </summary>
    public sealed class CustomUserAction : IUserAction
    {
        private readonly TapADN example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public CustomUserAction(TapADN context)
        {
            example = context;
        }

        public void OnSuccess()
        {
            example.ShowText($"上报成功");
        }

        public void OnError(int code, string message)
        {
            example.ShowText($"上报失败 code: {code} message: {message}");
        }
    }

    /// <summary>
    /// 权限开关——限制广告 SDK 获取设备信息
    /// </summary>
    public sealed class FormalCustomControllerWrapper : ICustomController
    {
        private readonly TapADN example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public FormalCustomControllerWrapper(TapADN context)
        {
            example = context;
        }


        public bool CanUseLocation => false;

        public TapAdLocation GetTapAdLocation => null;

        public bool CanUsePhoneState => false;
        public string GetDevImei => "";
        public bool CanUseWifiState => false;
        public bool CanUseWriteExternal => false;
        public string GetDevOaid => null;
        public bool Alist => false;
        public bool CanUseAndroidId => false;
        public CustomUser ProvideCustomer() => null;
    }
    /// <summary>
    /// 监控激励广告效果和用户行为
    /// </summary>
    public sealed class RewardVideoInteractionListener : IRewardVideoInteractionListener
    {
        private readonly TapADN example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public RewardVideoInteractionListener(TapADN context)
        {
            example = context;
        }

        public void OnAdShow(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdShow");
        }

        public void OnAdClose(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClose");
            // 广告关闭时自动上传用户行为
            example.OnClickUserActionButton(); // 调用上传方法
        }

        public void OnVideoComplete(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnVideoComplete");
        }

        public void OnVideoError(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnVideoError");
        }

        public void OnRewardVerify(TapRewardVideoAd ad, bool rewardVerify, int rewardAmount, string rewardName, int code, string msg)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnRewardVerify rewardVerify: {rewardVerify} rewardAmount: {rewardAmount} rewardName: {rewardName} code: {code} msg: {msg}");
        }

        public void OnSkippedVideo(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnSkippedVideo");
        }

        public void OnAdClick(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClick");
            Debug.LogErrorFormat($"激励视频 点击");
        }

        public void OnAdValidShow(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdValidShow");
        }
    }
    /// <summary>
    /// 激励视频广告监听器——反馈广告状态
    /// </summary>
    public sealed class RewardVideoAdLoadListener : IRewardVideoAdLoadListener
    {
        private readonly TapADN example;
        public event Action<TapRewardVideoAd> OnAdReadyToPlay;
        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public RewardVideoAdLoadListener(TapADN context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载激励视频错误! 错误 code: {code} message: {message}");
        }

        public void OnRewardVideoAdCached(TapRewardVideoAd ad)
        {
            example.ShowText($"{ad.AdType}素材 Cached 完毕! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Cached ad.IsReady");
            // 触发"可播放"事件
            OnAdReadyToPlay?.Invoke(ad);
        }

        public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
        {
            example.ShowText($"{ad.AdType}素材 Load 完毕! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Load ad.IsReady");
        }
    }

   
    /// <summary>
    /// 横幅广告加载监听器
    /// </summary>
    public sealed class BannerAdLoadListener : IBannerAdLoadListener
    {
        private readonly TapADN example;
        public event Action<TapBannerAd> OnAdReadyToPlay;
        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public BannerAdLoadListener(TapADN context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载Banner错误! 错误 code: {code} message: {message}");
        }

        public void OnBannerAdLoad(TapBannerAd ad)
        {
            example.ShowText($"{ad.AdType}广告 Loaded 完毕! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
            // 触发"可播放"事件
            OnAdReadyToPlay?.Invoke(ad);
        }
    }
    /// <summary>
    /// 追踪横幅广告从展示到关闭过程中的各种用户交互行为
    /// </summary>
    public sealed class BannerInteractionListener : IBannerAdInteractionListener
    {
        private readonly TapADN example;
        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public BannerInteractionListener(TapADN context)
        {
            example = context;
        }

        public void OnAdShow(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdShow");
        }

        public void OnAdClose(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClose");
            // 广告关闭时自动上传用户行为
            example.OnClickUserActionButton(); // 调用上传方法
        }

        public void OnAdClick(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClick");
        }

        public void OnDownloadClick(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnDownloadClick");
        }

        public void OnAdValidShow(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdValidShow");
        }
    }
    /// <summary>
    /// 开屏广告加载过程的监听器
    /// </summary>
    public sealed class SplashAdLoadListener : ISplashAdLoadListener
    {
        private readonly TapADN example;
        public event Action<TapSplashAd> OnAdReadyToPlay;
        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public SplashAdLoadListener(TapADN context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载Splash错误! 错误 code: {code} message: {message}");
        }

        public void OnSplashAdLoad(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}广告 OnSplashAdLoad! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
            // 触发"可播放"事件
            OnAdReadyToPlay?.Invoke(ad);
        }
    }
    /// <summary>
    /// 追踪开屏广告从展示到结束（或被跳过）过程中的各种关键事件
    /// </summary>
    public sealed class SplashInteractionListener : ISplashAdInteractionListener
    {
        private readonly TapADN example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public SplashInteractionListener(TapADN context)
        {
            example = context;
        }

        public void OnAdSkip(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdSkip");
            ad.Dispose();
            // 广告关闭时自动上传用户行为
            example.OnClickUserActionButton(); // 调用上传方法
        }

        public void OnAdTimeOver(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdTimeOver");
            ad.Dispose();
        }

        public void OnAdClick(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClick");
            // 广告关闭时自动上传用户行为
            example.OnClickUserActionButton(); // 调用上传方法
        }

        public void OnAdShow(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdShow");
        }

        public void OnAdValidShow(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdValidShow");
            // 广告关闭时自动上传用户行为
            example.OnClickUserActionButton(); // 调用上传方法
        }
    }
    /// <summary>
    /// 作为插屏广告加载过程的监听器
    /// </summary>
    public sealed class InterstitialAdLoadListener : IInterstitialAdLoadListener
    {
        private readonly TapADN example;
        public event Action<TapInterstitialAd> OnAdReadyToPlay;
        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public InterstitialAdLoadListener(TapADN context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载Splash错误! 错误 code: {code} message: {message}");
        }

        public void OnInterstitialAdLoad(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}广告 OnInterstitialAdLoad! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
            // 触发"可播放"事件
            OnAdReadyToPlay?.Invoke(ad);
        }

    }
    /// <summary>
    /// 全程追踪插屏广告的展示状态和用户交互行为
    /// </summary>
    public sealed class InterstitialAdInteractionListener : IInterstitialAdInteractionListener
    {
        private readonly TapADN example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public InterstitialAdInteractionListener(TapADN context)
        {
            example = context;
        }

        public void OnAdShow(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdShow");
        }

        public void OnAdClose(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClose");
            // 广告关闭时自动上传用户行为
            example.OnClickUserActionButton(); // 调用上传方法
        }

        public void OnAdError(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdError");
        }

        public void OnAdValidShow(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdValidShow");
        }

        public void OnAdClick(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClick");
        }
    }
}
