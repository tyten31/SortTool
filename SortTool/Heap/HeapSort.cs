namespace SortTool.Heap
{
    internal static class HeapSort
    {
        public static List<string> Sort(List<string> words)
        {
            var sortedWords = new List<string>();

            // We start Heapify at half the list size because we don't want to heapify leaf nodes.
            // Half or Half + 1 nodes in a binary tree are leaf nodes
            for (int i = (words.Count / 2) - 1; i >= 0; i--)
            {
                Heapify(words, words.Count, i);              
            }

            // At this point, the root is the largest (last alphabetically) so add it to the beginning of the sortedWords list.
            // Switch the first and last items in the list
            // Remove last element of list
            // Heapify new shortened list 
            // repeat
            for(int i = words.Count-1; i >=0; i--)
            {
                sortedWords.Insert(0, words[0]);

                words[0] = words[i];
                words = words.Take(i).ToList();

                Heapify(words, words.Count, 0);
            }

            return sortedWords;
        }

        // Heapify will move each parent node so that it is alphabetically after each child
        private static void Heapify(List<string> words, int length, int rootIndex)
        {
            //a c d b t o y
            //0 1 2 3 4 5 6
            //        a
            //    c       d
            //b      t o     y

            //a c y b t o d
            //0 1 2 3 4 5 6
            //        a
            //    c       y
            //b      t o     d

            //a t y b c o d
            //0 1 2 3 4 5 6
            //        a
            //    t       y
            //b      c o     d

            //y t a b c o d
            //0 1 2 3 4 5 6
            //        y
            //    t       a
            //b      c o     d

            //y t o b c a d
            //0 1 2 3 4 5 6
            //        y
            //    t       o
            //b      c a     d

            var largestIndex = rootIndex;
            var leftIndex = (2 * rootIndex) + 1;
            var rightIndex = (2 * rootIndex) + 2;

            // If left child is larger than root
            if (leftIndex < length && words[leftIndex].CompareTo(words[largestIndex]) > 0)
            {
                largestIndex = leftIndex;
            }

            // If right child is larger than largest so far
            if (rightIndex < length && words[rightIndex].CompareTo(words[largestIndex]) > 0)
            {
                largestIndex = rightIndex;
            }

            // If largest is not root
            if (largestIndex != rootIndex)
            {
                (words[largestIndex], words[rootIndex]) = (words[rootIndex], words[largestIndex]);

                // Recursively heapify the affected sub-tree
                Heapify(words, length, largestIndex);
            }
        }
    }
}