using DriverMatching.core.Models;
using DriverMatching.core.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class MatcherTests
{
    [Test]
    public void BruteForce_ShouldFindNearestDrivers()
    {
        var matcher = new BruteForceMatcher();
        matcher.AddDriver(new Driver("d1", 0, 0));
        matcher.AddDriver(new Driver("d2", 3, 4));
        matcher.AddDriver(new Driver("d3", 1, 1));
        matcher.AddDriver(new Driver("d4", 10, 10));
        matcher.AddDriver(new Driver("d5", 0, 1));

        var order = new Order(0, 0);
        var result = matcher.FindNearestDrivers(order, 3);

        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Id, Is.EqualTo("d1"));
        Assert.That(result[1].Id, Is.EqualTo("d5"));
        Assert.That(result[2].Id, Is.EqualTo("d3"));
    }

    [Test]
    public void KDTree_ShouldMatchBruteForce()
    {
        var brute = new BruteForceMatcher();
        var kdtree = new KDTreeMatcher();

        var drivers = new[]
        {
            new Driver("a", 1, 2),
            new Driver("b", 5, 5),
            new Driver("c", 0, 0),
            new Driver("d", 10, 1),
            new Driver("e", 3, 4),
            new Driver("f", 2, 2)
        };

        foreach (var d in drivers)
        {
            brute.AddDriver(d);
            kdtree.AddDriver(d);
        }

        var order = new Order(1, 1);
        var bruteRes = brute.FindNearestDrivers(order, 4);
        var kdRes = kdtree.FindNearestDrivers(order, 4);

        CollectionAssert.AreEquivalent(
            bruteRes.Select(d => d.Id),
            kdRes.Select(d => d.Id)
        );
    }

    [Test]
    public void GridBucket_UpdateAndRemove_WorkCorrectly()
    {
        var grid = new GridBucketMatcher(5);
        grid.AddDriver(new Driver("x", 1, 1));
        grid.AddDriver(new Driver("y", 9, 9));

        var order = new Order(0, 0);
        var r1 = grid.FindNearestDrivers(order, 2);
        Assert.That(r1.Select(d => d.Id), Does.Contain("x"));

        grid.UpdateDriver("x", 20, 20);
        var r2 = grid.FindNearestDrivers(order, 2);
        Assert.That(r2.Select(d => d.Id), Does.Not.Contain("x"));
    }
}