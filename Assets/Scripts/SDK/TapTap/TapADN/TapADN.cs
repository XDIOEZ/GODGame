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
    // banner ����id
    private const int PORTRAIT_BANNER_ID = 1048214;
    // banner ����id
    private const int LANDSCAPE_BANNER_ID = 1048214;
    // rewardVideo ����id
    private const int PORTRAIT_REWARD_ID = 1048214;
    // rewardVideo ����id
    private const int LANDSCAPE_REWARD_ID = 1048214;
    // splash ����id
    private const int PORTRAIT_SPLASH_ID = 1048214;
    // splash ����id
    private const int LANDSCAPE_SPLASH_ID = 1048214;

    // interstitial ����id
    private const int PORTRAIT_INTERSTITIAL_ID = 1048214;
    // interstitial ����id
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

    // Unity ���߳�ID:
    private static int mainThreadId;



    //�ص�����
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
        ShowText("��ʼ�����");
        callback();
    }



    /// <summary>
    /// �����û�����Ȩ��
    /// </summary>
    private void RequestPermission()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd ��Ҫ�ȳ�ʼ��!");
            return;
        }
        TapAdSdk.RequestPermissionIfNecessary();
        ShowText("����Ȩ��");
    }

    /// <summary>
    /// ����video��桪��������Ƶ
    /// </summary>
    /// <param name="horizontal"></param>
    private async void LoadRewardVideoAd(bool horizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd û�г�ʼ��!���Զ���ʼ��");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd ��ʼ�����");
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
            .RewardName("������Ƶ")
            .RewardCount(9)
            .UserId("123")
            .Build();
        _tapRewardAd = new TapRewardVideoAd(request);
        // ����������������"�ɲ���"�¼�
        var loadListener = new RewardVideoAdLoadListener(this);
        loadListener.OnAdReadyToPlay += (ad) =>
        {
            // �����ز�������ɺ��Զ�����
            PlayRewardVideoAd(); // ֱ�ӵ��ò��ŷ���
        };
        _tapRewardAd.SetLoadListener(loadListener);
        _tapRewardAd.Load();
    }
    /// <summary>
    /// ����video��桪��������Ƶ
    /// </summary>
    private void PlayRewardVideoAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd ��Ҫ�ȳ�ʼ��!");
            return;
        }
        if (_tapRewardAd != null)
        {
            _tapRewardAd.SetInteractionListener(new RewardVideoInteractionListener(this));
            _tapRewardAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] δ���غ���Ƶ,�޷�����!");
        }
    }
    /// <summary>
    /// ����video��桪��Banner��Ƶ
    /// </summary>
    /// <param name="isHorizontal"></param>
    private async void LoadBannerAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd û�г�ʼ��!���Զ���ʼ��");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd ��ʼ�����");
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
        // ����������������"�ɲ���"�¼�
        var loadListener = new BannerAdLoadListener(this);
        loadListener.OnAdReadyToPlay += (ad) =>
        {
            // �����ز�������ɺ��Զ�����
            PlayRewardVideoAd(); // ֱ�ӵ��ò��ŷ���
        };
        _tapBannerAd.SetLoadListener(new BannerAdLoadListener(this));
        _tapBannerAd.Load();
    }
    /// <summary>
    /// ����video��桪��Banner��Ƶ
    /// </summary>
    private void PlayBannerAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd ��Ҫ�ȳ�ʼ��!");
            return;
        }
        if (_tapBannerAd != null)
        {
            int baseLine = 0;
            int offset = 0;
            int.TryParse(bannerBaselineInput.text, out baseLine);
            if (baseLine != 0 && baseLine != 1)
            {
                Debug.LogErrorFormat("Banner Baseline ֻ�����ó� 0 ���� 1");
            }
            int.TryParse(bannerOffsetInput.text, out offset);
            _tapBannerAd.baseline = baseLine;
            _tapBannerAd.offset = offset;
            _tapBannerAd.SetInteractionListener(new BannerInteractionListener(this));
            _tapBannerAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] δ���غ���Ƶ,�޷�����!");
        }
    }
    /// <summary>
    /// ����video��桪��Splash��Ƶ
    /// </summary>
    /// <param name="isHorizontal"></param>
    private async void LoadSplashAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd û�г�ʼ��!���Զ���ʼ��");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd ��ʼ�����");
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
    /// ����video��桪��Splash��Ƶ
    /// </summary>
    private void PlaySplashAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd ��Ҫ�ȳ�ʼ��!");
            return;
        }
        if (_tapSplashAd != null)
        {
            _tapSplashAd.SetInteractionListener(new SplashInteractionListener(this));
            _tapSplashAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] δ���غ���Ƶ,�޷�����!");
        }
    }
    /// <summary>
    /// ����video��桪��interstitial��Ƶ
    /// </summary>
    /// <param name="isHorizontal"></param>
    private async void LoadInterstitialAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd û�г�ʼ��!���Զ���ʼ��");
            Init(RequestPermission);
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd ��ʼ�����");
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
    /// ����video��桪��interstitial��Ƶ
    /// </summary>
    private void PlayInterstitialAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd ��Ҫ�ȳ�ʼ��!");
            return;
        }
        if (_tapInterstitialAd != null)
        {
            _tapInterstitialAd.SetInteractionListener(new InterstitialAdInteractionListener(this));
            _tapInterstitialAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] δ���غ���Ƶ,�޷�����!");
        }
    }
    /// <summary>
    /// �ϱ��û���Ϊ�������ڹ۲��û�������ȷ���
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
    /// �û���Ϊ���ݷ�װ
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
            example.ShowText($"�ϱ��ɹ�");
        }

        public void OnError(int code, string message)
        {
            example.ShowText($"�ϱ�ʧ�� code: {code} message: {message}");
        }
    }

    /// <summary>
    /// Ȩ�޿��ء������ƹ�� SDK ��ȡ�豸��Ϣ
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
    /// ��ؼ������Ч�����û���Ϊ
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
            // ���ر�ʱ�Զ��ϴ��û���Ϊ
            example.OnClickUserActionButton(); // �����ϴ�����
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
            Debug.LogErrorFormat($"������Ƶ ���");
        }

        public void OnAdValidShow(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdValidShow");
        }
    }
    /// <summary>
    /// ������Ƶ�������������������״̬
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
            Debug.LogErrorFormat($"���ؼ�����Ƶ����! ���� code: {code} message: {message}");
        }

        public void OnRewardVideoAdCached(TapRewardVideoAd ad)
        {
            example.ShowText($"{ad.AdType}�ز� Cached ���! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Cached ad.IsReady");
            // ����"�ɲ���"�¼�
            OnAdReadyToPlay?.Invoke(ad);
        }

        public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
        {
            example.ShowText($"{ad.AdType}�ز� Load ���! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Load ad.IsReady");
        }
    }

   
    /// <summary>
    /// ��������ؼ�����
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
            Debug.LogErrorFormat($"����Banner����! ���� code: {code} message: {message}");
        }

        public void OnBannerAdLoad(TapBannerAd ad)
        {
            example.ShowText($"{ad.AdType}��� Loaded ���! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
            // ����"�ɲ���"�¼�
            OnAdReadyToPlay?.Invoke(ad);
        }
    }
    /// <summary>
    /// ׷�ٺ������չʾ���رչ����еĸ����û�������Ϊ
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
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdShow");
        }

        public void OnAdClose(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdClose");
            // ���ر�ʱ�Զ��ϴ��û���Ϊ
            example.OnClickUserActionButton(); // �����ϴ�����
        }

        public void OnAdClick(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdClick");
        }

        public void OnDownloadClick(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnDownloadClick");
        }

        public void OnAdValidShow(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdValidShow");
        }
    }
    /// <summary>
    /// ���������ع��̵ļ�����
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
            Debug.LogErrorFormat($"����Splash����! ���� code: {code} message: {message}");
        }

        public void OnSplashAdLoad(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��� OnSplashAdLoad! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
            // ����"�ɲ���"�¼�
            OnAdReadyToPlay?.Invoke(ad);
        }
    }
    /// <summary>
    /// ׷�ٿ�������չʾ���������������������еĸ��ֹؼ��¼�
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
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdSkip");
            ad.Dispose();
            // ���ر�ʱ�Զ��ϴ��û���Ϊ
            example.OnClickUserActionButton(); // �����ϴ�����
        }

        public void OnAdTimeOver(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdTimeOver");
            ad.Dispose();
        }

        public void OnAdClick(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClick");
            // ���ر�ʱ�Զ��ϴ��û���Ϊ
            example.OnClickUserActionButton(); // �����ϴ�����
        }

        public void OnAdShow(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdShow");
        }

        public void OnAdValidShow(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdValidShow");
            // ���ر�ʱ�Զ��ϴ��û���Ϊ
            example.OnClickUserActionButton(); // �����ϴ�����
        }
    }
    /// <summary>
    /// ��Ϊ���������ع��̵ļ�����
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
            Debug.LogErrorFormat($"����Splash����! ���� code: {code} message: {message}");
        }

        public void OnInterstitialAdLoad(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��� OnInterstitialAdLoad! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
            // ����"�ɲ���"�¼�
            OnAdReadyToPlay?.Invoke(ad);
        }

    }
    /// <summary>
    /// ȫ��׷�ٲ�������չʾ״̬���û�������Ϊ
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
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdShow");
        }

        public void OnAdClose(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdClose");
            // ���ر�ʱ�Զ��ϴ��û���Ϊ
            example.OnClickUserActionButton(); // �����ϴ�����
        }

        public void OnAdError(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdError");
        }

        public void OnAdValidShow(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdValidShow");
        }

        public void OnAdClick(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}��Ƶ OnAdClick");
        }
    }
}
