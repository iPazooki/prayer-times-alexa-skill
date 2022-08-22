using System;
using System.Threading.Tasks;
using FluentAssertions;
using PrayerTime.Service;
using Xunit;

namespace PrayerTime.Tests;

public class FunctionTest
{
    private readonly IPrayerService _prayerService;

    public FunctionTest()
    {
        _prayerService = new PrayerService();
    }

    [Fact]
    public async Task Test_Prayer_By_City()
    {
        _prayerService.Should().NotBeNull();

        var result = await _prayerService.GetAdhanTime(DateOnly.FromDateTime(DateTime.Now), "United Kingdom", "London");

        result.Should().NotBeNull();

        result.Fajr.Should().NotBeNullOrEmpty();
        result.Dhuhr.Should().NotBeNullOrEmpty();
        result.Maghrib.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Test_Prayer_By_Coordinate()
    {
        _prayerService.Should().NotBeNull();

        var result = await _prayerService.GetAdhanTime(DateOnly.FromDateTime(DateTime.Now), 51.5074, 0.1278);

        result.Should().NotBeNull();

        result.Fajr.Should().NotBeNullOrEmpty();
        result.Dhuhr.Should().NotBeNullOrEmpty();
        result.Maghrib.Should().NotBeNullOrEmpty();
    }
}