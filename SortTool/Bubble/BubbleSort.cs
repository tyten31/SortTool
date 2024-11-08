namespace SortTool.Bubble
{
    internal static class BubbleSort
    {
        public static List<string> Sort(List<string> words)
        {
            // Loop the number of items in list
            //for (int j = 0; j < words.Count; j++)
            //{
            //    // compare side by side items and alphabetize
            //    for (int i = 0; i < words.Count - 1; i++)
            //    {
            //        if (words[i].CompareTo(words[i + 1]) > 0)
            //        {
            //            (words[i + 1], words[i]) = (words[i], words[i + 1]);
            //        }
            //    }
            //}

            var hadSwitch = true;

            while (hadSwitch)
            {
                hadSwitch = false;

                // compare side by side items and alphabetize
                for (int i = 0; i < words.Count - 1; i++)
                {
                    if (words[i].CompareTo(words[i + 1]) > 0)
                    {
                        (words[i + 1], words[i]) = (words[i], words[i + 1]);
                        hadSwitch = true;
                    }
                }
            }

            return words;
        }
    }
}