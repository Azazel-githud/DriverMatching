using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DriverMatching.core.Models;
using DriverMatching.core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

[MemoryDiagnoser]
[RankColumn]
public class MatcherBenchmark
{
    private List<Driver> _drivers = new();
    private Order _order = new(50, 50);

    [Params(100, 1000, 10000)]
    public int DriverCount;

    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(42);
        _drivers = Enumerable.Range(0, DriverCount)
            .Select(i => new Driver($"d{i}", rnd.Next(0, 100), rnd.Next(0, 100)))
            .ToList();
        _order = new Order(50, 50);
    }

    [Benchmark]
    public IReadOnlyList<Driver> BruteForce()
    {
        var m = new BruteForceMatcher();
        foreach (var d in _drivers) m.AddDriver(d);
        return m.FindNearestDrivers(_order, 5);
    }

    [Benchmark]
    public IReadOnlyList<Driver> KDTree()
    {
        var m = new KDTreeMatcher();
        foreach (var d in _drivers) m.AddDriver(d);
        return m.FindNearestDrivers(_order, 5);
    }

    [Benchmark]
    public IReadOnlyList<Driver> GridBucket()
    {
        var m = new GridBucketMatcher(10);
        foreach (var d in _drivers) m.AddDriver(d);
        return m.FindNearestDrivers(_order, 5);
    }
}

// Главный класс
public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<MatcherBenchmark>();
    }
}