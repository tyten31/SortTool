namespace SortTool.Radix
{
    internal class RadixSort
    {
        public List<string> Sort(List<string> words)
        {
            // Get length of longest word
            var length = words.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            var buckets = new List<RadixBucket>();

            for (int i = length; i > 0; i--)
            {
                var leftOvers = new List<string>();

                foreach (var word in words)
                {
                    if (word.Length >= i)
                    {
                        var character = word[i - 1];
                        var bucket = buckets.Find(x => x.Character.Equals(character));

                        if (bucket == null)
                        {
                            var newBucket = new RadixBucket { Character = character };
                            newBucket.Items.Add(word);
                            buckets.Add(newBucket);
                        }
                        else
                        {
                            bucket.Items.Add(word);
                        }
                    }
                    else
                    {
                        leftOvers.Add(word);
                    }
                }

                words = Squish(buckets, leftOvers);
            }

            return words;
        }

        private List<string> Squish(List<RadixBucket> buckets, List<string> leftOvers)
        {
            var sortedWords = new List<string>(leftOvers);

            foreach (var bucket in buckets.OrderBy(x => x.Character).ToList())
            {
                sortedWords.AddRange(bucket.Items);
                bucket.Items.Clear();
            }

            return sortedWords;
        }
    }
}