namespace SortTool.Merge
{
    internal static class MergeSort
    {
        public static List<string> Sort(List<string> words)
        {
            // If list has 1 element we have hit the bottom so return
            if (words.Count <= 1)
            {
                return words;
            }

            var middle = words.Count / 2;
            var left = words.Take(middle).ToList();
            var right = words.Skip(middle).ToList();

            left = Sort(left);
            right = Sort(right);

            return Merge(left, right);
        }

        private static List<string> Merge(List<string> left, List<string> right)
        {
            var sorted = new List<string>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left[0].CompareTo(right[0]) < 0)
                    {
                        sorted.Add(left[0]);
                        left.Remove(left[0]);
                    }
                    else
                    {
                        sorted.Add(right[0]);
                        right.Remove(right[0]);
                    }
                }
                else if (left.Count > 0)
                {
                    sorted.Add(left[0]);
                    left.Remove(left[0]);
                }
                else
                {
                    sorted.Add(right[0]);
                    right.Remove(right[0]);
                }
            }

            return sorted;
        }
    }
}