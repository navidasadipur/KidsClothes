using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsClothes.Core.Utility
{
    public enum DiscountType
    {
        Percentage = 1,
        Amount = 2
    }
    public enum GeoDivisionType
    {
        Country = 0,
        State = 1,
        City = 2,
    }

    public enum StaticContents
    {
        ContactUsDescriptionImg = 12,
        AboutDescription = 15,
        firstImageAboutPage = 16,
        WorkingHours = 1008,
        Youtube = 29,
        Instagram = 28,
        Twitter = 27,
        Pinterest = 30,
        Facebook = 26,
        linkedin = 33,
        SocialDescription = 1034,
        BlogImage = 1013,
        ContactInfo = 1014,
        companyServices = 3003,
        CopyRight = 3004,
        ImplementaitonService = 3005,
        WorkingHoursSaturdayWednesday = 11,
        WorkingHoursTuesday = 10,
        StoreOneAddressPhone = 1044,
        StoreTwoAddressPhone = 1046,
        StoreOneWorkingHours = 1045,
        StoreTwoWorkingHours = 1047,

        Address = 5,
        Email = 6,
        Phone = 7,
        ContactUsMap = 4,

        DiscountNews = 5027,

        Logo = 14,
        HomeMiddleBanner = 2,
        NewsBackImage = 8,

        BlogAd = 32,
    }

    public enum StaticContentTypes
    {
        HeaderFooter = 9,
        About = 13,
        AboutProperties = 14,
        AboutCustomerCounter = 5,
        CustomersComments = 3,

        HomeTopSlider = 17,
        Contact = 2,

        Guide = 9,
        Popup = 11,
        PageBanner = 12,
        OurServices = 3,
    }

    public enum PaymentStatus
    {
        Unprocessed = 1,
        Failed =2,
        Succeed =3,
        Expired = 4
    }

    public enum AditionalFeatureType
    {
        Volume = 1
    }

}
