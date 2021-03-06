﻿// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using Microsoft.WindowsAzure.MobileServices.Test.UnitTests;
using Microsoft.WindowsAzure.MobileServices.TestFramework;

namespace Microsoft.WindowsAzure.MobileServices.Test
{
    [Tag("unit")]
    [Tag("http")]
    public class HttpUtilityTests : TestBase
    {

        [TestMethod]
        public static void TryParseQueryUri_ReturnsTrue_WhenQueryIsRelativeOrAbsoluteUri()
        {
            var data = new[]
            {
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileApp, 
                    Query = "/about?$filter=a eq b&$orderby=c", 
                    Absolute = false,
                    Result = MobileAppUriValidator.DummyMobileApp + "about?$filter=a eq b&$orderby=c"
                },
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileAppWithoutTralingSlash, 
                    Query = MobileAppUriValidator.DummyMobileAppWithoutTralingSlash + "/about?$filter=a eq b&$orderby=c", 
                    Absolute = true,
                    Result = MobileAppUriValidator.DummyMobileAppWithoutTralingSlash + "/about?$filter=a eq b&$orderby=c"
                },
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileAppUriWithFolder,
                    Query = MobileAppUriValidator.DummyMobileAppUriWithFolder + "about?$filter=a eq b&$orderby=c", 
                    Absolute = true,
                    Result = MobileAppUriValidator.DummyMobileAppUriWithFolder + "about?$filter=a eq b&$orderby=c"
                },
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileAppUriWithFolderWithoutTralingSlash,
                    Query = MobileAppUriValidator.DummyMobileAppUriWithFolderWithoutTralingSlash + "/about?$filter=a eq b&$orderby=c", 
                    Absolute = true,
                    Result = MobileAppUriValidator.DummyMobileAppUriWithFolderWithoutTralingSlash + "/about?$filter=a eq b&$orderby=c"
                }
            };

            foreach (var item in data)
            {
                Uri result;
                bool absolute;
                Assert.IsTrue(HttpUtility.TryParseQueryUri(new Uri(item.ServiceUri), item.Query, out result, out absolute));
                Assert.AreEqual(absolute, item.Absolute);
                AssertEx.QueryEquals(result.AbsoluteUri, item.Result);
            }
        }

        [TestMethod]
        public static void TryParseQueryUri_ReturnsFalse_WhenQueryIsNotRelativeOrAbsoluteUri()
        {
            var data = new[]
            {
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileApp, 
                    Query = "about?$filter=a eq b&$orderby=c", 
                    Result = MobileAppUriValidator.DummyMobileApp + "about?$filter=a eq b&$orderby=c"
                },
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileAppWithoutTralingSlash, 
                    Query = "$filter=a eq b&$orderby=c", 
                    Result = MobileAppUriValidator.DummyMobileAppWithoutTralingSlash + "/about?$filter=a eq b&$orderby=c"
                },
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileAppUriWithFolder, 
                    Query = "$filter=a eq b&$orderby=c", 
                    Result = MobileAppUriValidator.DummyMobileAppUriWithFolder + "about?$filter=a eq b&$orderby=c"
                },
                new 
                {
                    ServiceUri = MobileAppUriValidator.DummyMobileAppUriWithFolderWithoutTralingSlash, 
                    Query = "$filter=a eq b&$orderby=c", 
                    Result = MobileAppUriValidator.DummyMobileAppUriWithFolderWithoutTralingSlash + "/about?$filter=a eq b&$orderby=c"
                }
            };

            foreach (var item in data)
            {
                Uri result;
                bool absolute;
                Assert.IsFalse(HttpUtility.TryParseQueryUri(new Uri(item.ServiceUri), item.Query, out result, out absolute));
                Assert.IsFalse(absolute);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetUriWithoutQuery_ReturnsUriWithPath()
        {
            Tuple<string, string>[] input = new[]
            {
                Tuple.Create("http://contoso.com/asdf?$filter=3", "http://contoso.com/asdf"),
                Tuple.Create("http://contoso.com/asdf/def?$filter=3", "http://contoso.com/asdf/def"),
                Tuple.Create("https://contoso.com/asdf/def?$filter=3", "https://contoso.com/asdf/def")
            };

            foreach (var item in input)
            {
                AssertEx.QueryEquals(HttpUtility.GetUriWithoutQuery(new Uri(item.Item1)), item.Item2);
            }
        }
    }
}
