using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using PrayerTimes;
using PrayerTimes.Service;
using FluentAssertions;
using Alexa.NET.Request;

namespace PrayerTimes.Tests
{
    public class FunctionTest
    {
        private readonly IPrayerService prayerService;

        public FunctionTest()
        {
            prayerService = new PrayerService();
        }

        [Fact]
        public async Task Test_Prayer_By_City()
        {
            prayerService.Should().NotBeNull();

            var result = await prayerService.GetTimings(DateTime.Now, "United Kingdom", "London");

            result.Should().NotBeNull();

            result.Fajr.Should().NotBeNullOrEmpty();
            result.Dhuhr.Should().NotBeNullOrEmpty();
            result.Maghrib.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Test_Prayer_By_Coordinate()
        {
            prayerService.Should().NotBeNull();

            var result = await prayerService.GetTimings(DateTime.Now, 51.5074, 0.1278);

            result.Should().NotBeNull();

            result.Fajr.Should().NotBeNullOrEmpty();
            result.Dhuhr.Should().NotBeNullOrEmpty();
            result.Maghrib.Should().NotBeNullOrEmpty();
        }
    }
}