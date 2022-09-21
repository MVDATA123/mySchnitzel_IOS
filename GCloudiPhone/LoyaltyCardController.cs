using System;
using System.Drawing;
using System.Timers;
using CoreGraphics;
using Foundation;
using GCloudiPhone.Caching;
using GCloudiPhone.Helpers;
using GCloudShared.Repository;
using GCloudShared.Service;
using GCloudShared.Shared;
using Refit;
using SafariServices;
using SidebarNavigation;
using StoreKit;
using UIKit;
using WebKit;

namespace GCloudiPhone
{
    public partial class LoyaltyCardController : UIViewController
    {
        private UIBarButtonItem menuBarButton;
        private UIBarButtonItem loginBarButton;

        private Random random;
        private Timer timer;

        private readonly NSUrl urlOurProducts = new NSUrl("https://myschnitzel.at/apppart/speisekarte-produkte/");
        private readonly NSUrl urlOurMenu = new NSUrl("https://myschnitzel.at/apppart/speisekarten/");

        private readonly UserRepository _userRepository;
        private readonly IAuthService _authService;


        protected SidebarController SidebarControllerlogi
        {
            get
            {
                if (ManagerRootViewController.Instance.TryGetTarget(out var managerRootView))
                {
                    return managerRootView.SidebarController;
                }
                return null;
            }
        }

        public LoyaltyCardController(IntPtr handle) : base(handle)
        {
            //menuBarButton = new UIBarButtonItem(UIImage.FromBundle("MenuIcon"), UIBarButtonItemStyle.Plain, (sender, e) => SidebarController.ToggleMenu());
            loginBarButton = new UIBarButtonItem(UIImage.FromBundle("LoginIcon"), UIBarButtonItemStyle.Plain, (sender, e) => TabBarController.PerformSegue("LoginSegue", this));

            _userRepository = new UserRepository(DbBootstraper.Connection);
            _authService = RestService.For<IAuthService>(HttpClientContainer.Instance.HttpClient);

            random = new Random();
#if DEBUG
            timer = new Timer(TimeSpan.FromSeconds(20).TotalMilliseconds)
            {
                AutoReset = true
            };
#else
            timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds)
            {
                AutoReset = true
            };
#endif
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InvokeOnMainThread(() =>
            {   //Sa mainstoryboard-a je izbrisana pozadinska slika
                //BackgroundImage.Image = CacheHolder.Instance.NextImage();
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //LoginMessageLabel.RemoveFromSuperview();
            LoginButton.RemoveFromSuperview();
            NavigationItem.RightBarButtonItem = null;
            NavigationItem.LeftBarButtonItem = null;

            //ShadowView.Layer.ShadowColor = UIColor.Black.CGColor;
            //ShadowView.Layer.ShadowOffset = new CGSize(5.0d, 5.0d);
            //ShadowView.Layer.ShadowRadius = 5.0f;
            //ShadowView.Layer.ShadowOpacity = 0.7f;

            //Menjamo boju pozadine i navigation item
            View.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            //UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            //NavigationItem.Title = "";
            this.NavigationController.SetNavigationBarHidden(true, true);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var user = _userRepository.GetCurrentUser();
            if (user != null)
            {
                var totalPoints = _authService.GetTotalPointsByUserID(user.UserId).Result;
                var totalPointsNew = totalPoints.Replace("\"", "");


                TotalPointsLabel.Text = totalPointsNew;
                TotalPointsLabel.TextColor = UIColor.Red;

                PointsLabel.Text = " Punkte";
                PointsLabel.TextColor = UIColor.Black;

            }




            //var span = totalPointsNew + " Punkte";
            //var indexOfPunkte = span.IndexOf(" Punkte");

            timer.Elapsed += Timer_Elapsed;

            InvokeInBackground(() =>
            {
                var backgroundImage = CacheHolder.Instance.NextImage();
                //Sa mainstoryboard-a je izbrisana pozadinska slika
                //InvokeOnMainThread(() => BackgroundImage.Image = backgroundImage);
            });

            timer.Start();

            if (((AppDelegate)UIApplication.SharedApplication.Delegate).AuthState == AuthState.Unauthorized)
            {
                NavigationItem.LeftBarButtonItem = null;
               // View.Add(LoginMessageLabel);
                View.Add(LoginButton);
                //NavigationItem.RightBarButtonItem = loginBarButton; // login dugme vec postoji, tako da je za sada izbaceno ovo bar button dugme
                LoyaltyCardImage.Image = UIImage.FromBundle("Logo");

                ShadowView.Hidden = true;
            }
            else
            {
                ShadowView.Hidden = false;

                // TODO: Uncomment this!
                //if (new UserRepository(DbBootstraper.Connection).GetCurrentUser().RoleName.Equals("Managers"))
                //{
                //    SidebarController.Disabled = false;
                //    NavigationItem.LeftBarButtonItem = menuBarButton;
                //}

                InvokeInBackground(SetLoyaltyCard);
            }

            TabBarController.TabBar.Hidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //SidebarController.Disabled = true;
            timer.Stop();
            timer.Elapsed -= Timer_Elapsed;

        }

        public override void ViewDidDisappear(bool animated)
        {
            //LoginMessageLabel.RemoveFromSuperview();
            LoginButton.RemoveFromSuperview();
            NavigationItem.RightBarButtonItem = null;
            NavigationItem.LeftBarButtonItem = null;

            if ((NavigationController == null && IsMovingFromParentViewController) || (ParentViewController != null && ParentViewController.IsBeingDismissed))
            {
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }

            base.ViewDidDisappear(animated);
        }

        private void SetLoyaltyCard()
        {
            var loyaltyCard = CachingService.GetLoyaltyCard();
            InvokeOnMainThread(() => LoyaltyCardImage.Image = loyaltyCard);
        }

        //partial void StoreButton_TouchUpInside(UIButton sender)
        //{
        //    TabBarController.TabBar.Hidden = false;
        //    ((TabBarController)TabBarController).ChangeSelectedItem(1);
        //}

        //Store list and store details
        partial void MapButton_TouchUpInside(UIButton sender)
        {
            TabBarController.TabBar.Hidden = false;
            ((TabBarController)TabBarController).ChangeSelectedItem(1);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        //Settings
        partial void ProfileButton_TouchUpInside(UIButton sender)
        {
            TabBarController.TabBar.Hidden = false;
            ((TabBarController)TabBarController).ChangeSelectedItem(3);
        }

        //partial void EBillButton_TouchUpInside(UIButton sender)
        //{
        //    TabBarController.TabBar.Hidden = false;
        //    ((TabBarController)TabBarController).ChangeSelectedItem(3);
        //}


        //OurProducts web view
        partial void OpenOurProducts(UIButton sender)
        {
            // Ako koristimo Safari, gde je back button automatski implementiran:
            // UIApplication.SharedApplication.OpenUrl(urlOurProducts);

            TabBarController.TabBar.Hidden = false;
            // Automatski nas prebacuje na webViewSiteController
            ((TabBarController)TabBarController).ChangeSelectedItem(2);
        }

        //Speisekarte
        partial void OpenOurMenu(UIButton sender)
        {
            // Ako koristimo Safari, gde je back button automatski implementiran:
            // UIApplication.SharedApplication.OpenUrl(urlOurMenu);

            TabBarController.TabBar.Hidden = false;
            // Automatski nas prebacuje na webViewOurProducts
            ((TabBarController)TabBarController).ChangeSelectedItem(4);
        }

        //Coupons - type 1,2,3
        partial void OpenAktionenTab(UIButton sender)
        {
            // Ako koristimo Safari, gde je back button automatski implementiran:
            // UIApplication.SharedApplication.OpenUrl(urlOurMenu);

            TabBarController.TabBar.Hidden = false;
            // Automatski nas prebacuje na webViewOurProducts
            ((TabBarController)TabBarController).ChangeSelectedItem(5);
        }

        //Coupons - type 4
        partial void OpenSpecialProductsTab(UIButton sender)
        {
            // Ako koristimo Safari, gde je back button automatski implementiran:
            // UIApplication.SharedApplication.OpenUrl(urlOurMenu);

            TabBarController.TabBar.Hidden = false;
            // Automatski nas prebacuje na webViewOurProducts
            ((TabBarController)TabBarController).ChangeSelectedItem(6);
        }


        //On line shop
        partial void OnlineShop(UIButton sender)
        {
            // Ako koristimo Safari, gde je back button automatski implementiran:
            // UIApplication.SharedApplication.OpenUrl(urlOurMenu);

            TabBarController.TabBar.Hidden = false;
            // Automatski nas prebacuje na webViewOurProducts
            ((TabBarController)TabBarController).ChangeSelectedItem(7
                );
        }

       
    }
}