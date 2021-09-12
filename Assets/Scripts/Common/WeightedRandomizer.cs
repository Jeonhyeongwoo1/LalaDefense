using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class WeightedRandomizer
{
    private static System.Random _random = new System.Random();

    public static string TakeOne(Dictionary<string, int> weight)
    {
        var sortedSpawnRate = Sort(weight);
        int sum = 0;
        foreach (var spawn in weight)
        {
            sum += spawn.Value;
        }

        int roll = _random.Next(0, sum);

        string selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        return selected;
    }

    private static List<KeyValuePair<string, int>> Sort(Dictionary<string, int> weights)
    {
        var list = new List<KeyValuePair<string, int>>(weights);

        list.Sort(
            delegate (KeyValuePair<string, int> firstPair,
                        KeyValuePair<string, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );

        return list;
    }
}
