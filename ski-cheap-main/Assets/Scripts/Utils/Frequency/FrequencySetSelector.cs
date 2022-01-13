using System.Collections.Generic;
using System.Linq;

public class FrequencySelector<T> where T : IHasFrequency
{
    private int TotalFrequency;
    private List<FrequencyRangeMapping> FrequencyRangeMap;

    public FrequencySelector(IEnumerable<T> items)
    {
        var itemArray = items.ToArray();
        TotalFrequency = 0;
        FrequencyRangeMap = new List<FrequencyRangeMapping>();

        for (var i = itemArray.Count() - 1; i >= 0; i--)
        {
            var item = itemArray[i];
            var itemFrequency = item.GetFrequency();
            var minFrequencyNumber = TotalFrequency;
            var maxFrequencyNumber = TotalFrequency + itemFrequency;
            
            var itemFrequencyRange = new FrequencyRangeMapping(item, minFrequencyNumber, maxFrequencyNumber);
            FrequencyRangeMap.Add(itemFrequencyRange);

            TotalFrequency = maxFrequencyNumber;
        }
    }

    public T GetItem()
    {
        var randomRoll = UnityEngine.Random.Range(0, TotalFrequency);
        var randomItem = FrequencyRangeMap
            .First(item => randomRoll > item.MinSelectorNumber && randomRoll <= item.MaxSelectorNumber)
            .Item;
        return randomItem;
    }

    private class FrequencyRangeMapping
    {
        public T Item;
        public int MinSelectorNumber;
        public int MaxSelectorNumber;

        public FrequencyRangeMapping(T item, int minSelectorNumber, int maxSelectorNumber)
        {
            Item = item;
            MinSelectorNumber = minSelectorNumber;
            MaxSelectorNumber = maxSelectorNumber;
        }
    }
}
