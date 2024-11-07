namespace SortTool.Quick
{
    internal static class QuickSort
    {
        public static List<string> Sort(List<string> words)
        {
            if (words.Count <= 1)
            {
                return words;
            }

            var pivot = words.Last();
            var before = new List<string>();
            var after = new List<string>();
            var same = new List<string>();

            foreach (var word in words)
            {
                if (word.CompareTo(pivot) < 0)
                {
                    before.Add(word);
                }
                else if (word.CompareTo(pivot) > 0)
                {
                    after.Add(word);
                }
                else
                {
                    same.Add(word);
                }
            }

            Sort(before);
            Sort(after);

            words.Clear();
            words.AddRange(before);
            words.AddRange(same);
            words.AddRange(after);

            return words;
        }
    }
}