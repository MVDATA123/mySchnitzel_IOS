// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GCloudiPhone
{
    [Register ("LoyaltyCardController")]
    partial class LoyaltyCardController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Aktionen { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LoginMessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LoyaltyCardImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MapButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ProfileButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ShadowView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TotalPointsLabel { get; set; }

        [Action ("MapButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void MapButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("OnlineShop:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnlineShop (UIKit.UIButton sender);

        [Action ("OpenAktionenTab:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenAktionenTab (UIKit.UIButton sender);

        [Action ("OpenOurMenu:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenOurMenu (UIKit.UIButton sender);

        [Action ("OpenOurProducts:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenOurProducts (UIKit.UIButton sender);

        [Action ("OpenSpecialProductsTab:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenSpecialProductsTab (UIKit.UIButton sender);

        [Action ("ProfileButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ProfileButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Aktionen != null) {
                Aktionen.Dispose ();
                Aktionen = null;
            }

            if (BackgroundView != null) {
                BackgroundView.Dispose ();
                BackgroundView = null;
            }

            if (LoginButton != null) {
                LoginButton.Dispose ();
                LoginButton = null;
            }

            if (LoginMessageLabel != null) {
                LoginMessageLabel.Dispose ();
                LoginMessageLabel = null;
            }

            if (LoyaltyCardImage != null) {
                LoyaltyCardImage.Dispose ();
                LoyaltyCardImage = null;
            }

            if (MapButton != null) {
                MapButton.Dispose ();
                MapButton = null;
            }

            if (PointsLabel != null) {
                PointsLabel.Dispose ();
                PointsLabel = null;
            }

            if (ProfileButton != null) {
                ProfileButton.Dispose ();
                ProfileButton = null;
            }

            if (ShadowView != null) {
                ShadowView.Dispose ();
                ShadowView = null;
            }

            if (TotalPointsLabel != null) {
                TotalPointsLabel.Dispose ();
                TotalPointsLabel = null;
            }
        }
    }
}