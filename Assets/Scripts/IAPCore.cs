using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;


public class IAPCore : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static string noAds = "noads";
    public static string unlockMina = "unlockMina";
    [Header("Weapons")]
    [SerializeField] private Weapon _mina;
    public static string unlockThorns = "unlockThorns";
    [SerializeField] private Weapon _thorns;
    public static string unlockKnife = "unlockKnife";
    [SerializeField] private Weapon _knife;
    public static string unlockMissile = "unlockMissile";
    [SerializeField] private Weapon _missile;
    public static string unlockBoomerang = "unlockBoomerang";
    [SerializeField] private Weapon _boomerang;
    public static string unlockFireworks = "unlockFireworks";
    [SerializeField] private Weapon _fireworks;
    public static string unlockBayraktar = "unlockBayraktar";
    [SerializeField] private Weapon _bayraktar;
    public static string unlockAll = "unlockAll";
    [Header("Buttons")]
    [SerializeField] private Button _adsButton;
    [SerializeField] private Button _allButton;

    void Awake()
    {
        if (PlayerPrefs.HasKey("noAdsBuy"))
            _adsButton.interactable = false;
        if (_mina.GetActive())
            if (_thorns.GetActive())
                if (_knife.GetActive())
                    if (_missile.GetActive())
                        if (_boomerang.GetActive())
                            if (_fireworks.GetActive())
                                if (_bayraktar.GetActive())
                                    _allButton.interactable = false;
    }

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(noAds, ProductType.NonConsumable);
        builder.AddProduct(unlockMina, ProductType.NonConsumable);
        builder.AddProduct(unlockThorns, ProductType.NonConsumable);
        builder.AddProduct(unlockKnife, ProductType.NonConsumable);
        builder.AddProduct(unlockMissile, ProductType.NonConsumable);
        builder.AddProduct(unlockBoomerang, ProductType.NonConsumable);
        builder.AddProduct(unlockFireworks, ProductType.NonConsumable);
        builder.AddProduct(unlockBayraktar, ProductType.NonConsumable);
        builder.AddProduct(unlockAll, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyNoAds()
    {
        BuyProductID(noAds);
    }
    public void BuyMina()
    {
        BuyProductID(unlockMina);
    }
    public void BuyThorns()
    {
        BuyProductID(unlockThorns);
    }
    public void BuyKnife()
    {
        BuyProductID(unlockKnife);
    }
    public void BuyMissile()
    {
        BuyProductID(unlockMissile);
    }
    public void BuyBoomerang()
    {
        BuyProductID(unlockBoomerang);
    }
    public void BuyFireworks()
    {
        BuyProductID(unlockFireworks);
    }
    public void BuyBayraktar()
    {
        BuyProductID(unlockBayraktar);
    }
    public void BuyAll()
    {
        BuyProductID(unlockAll);
    }



    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, noAds, StringComparison.Ordinal))
        {
            if (!PlayerPrefs.HasKey("noAdsBuy"))
            {
                PlayerPrefs.SetInt("noAdsBuy", 0);
                PlayerPrefs.Save();
                Destroy(AdsInitializer.Instance);
                Destroy(InterstitialAds.Instance);
                Destroy(RewardedAds.Instance);
                Destroy(ShowAdByTime.Instance);
            }
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockMina, StringComparison.Ordinal))
        {
            _mina.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockThorns, StringComparison.Ordinal))
        {
            _thorns.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockKnife, StringComparison.Ordinal))
        {
            _knife.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockMissile, StringComparison.Ordinal))
        {
            _missile.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockBoomerang, StringComparison.Ordinal))
        {
            _boomerang.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockFireworks, StringComparison.Ordinal))
        {
            _fireworks.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockBayraktar, StringComparison.Ordinal))
        {
            _bayraktar.SetActive(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlockAll, StringComparison.Ordinal))
        {
            _mina.SetActive(true);
            _thorns.SetActive(true);
            _knife.SetActive(true);
            _missile.SetActive(true);
            _boomerang.SetActive(true);
            _fireworks.SetActive(true);
            _bayraktar.SetActive(true);
        }
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}