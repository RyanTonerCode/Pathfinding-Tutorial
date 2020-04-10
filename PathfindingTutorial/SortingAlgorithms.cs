using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingTutorial
{
    /**
     * Generic Comparison Sorting Class implementing:
     * Selection Sort
     * Insertion Sort
     * Merge Sort
     * Quick Sort
     * Includes some running and testing functionality
     * @author Ryan Toner
     */
    public class SortingAlgorithms<T> where T : IComparable
    {
        /**
            Enums for easy sort picking
        */
        public enum SortType { INSERTION, SELECTION, BUBBLE, MERGE, QUICK }

        /**
        * @param type enum of desired sort type
        * @param dataset array of items to sort
        * Times a given sort for the input dataset in precise number of milliseconds returned as double.
        */
        public static double timeSort(SortType type, T[] dataset)
        {
            stitchArray = null;
            mergeItems = null;
            quickItems = null;

            Stopwatch timer = new Stopwatch();
            timer.Start();
            RunSort(type, ref dataset);
            timer.Stop();

            return timer.Elapsed.TotalMilliseconds;
        }

        /**
        * @param type enum of desired sort type
        * @param dataset array of items to sort
        * Runs the input sort on the given dataset and returns the sorted array
        */
        public static T[] RunSort(SortType type, ref T[] dataset)
        {
            switch (type)
            {
                case SortType.INSERTION:
                    return InsertionSort(dataset);
                case SortType.SELECTION:
                    return SelectionSort(dataset);
                case SortType.BUBBLE:
                    return BubbleSort(dataset);
                case SortType.MERGE:
                    return MergeSort(dataset);
                default:
                    return QuickSort(dataset);
            }
        }

        /**
        * Basic test to see if each item is continuous and increasing (sorted)
        */
        public static bool TestSorted(T[] items)
        {
            int correct = 0;
            for (int i = 0; i < items.Length - 2; i++)
                if (items[i].CompareTo(items[i + 1]) <= 0 && items[i].CompareTo(items[i + 2]) <= 0 && items[i + 1].CompareTo(items[i + 2]) <= 0)
                    correct++;

            return (correct == items.Length - 2);

        }

        /**
        * Runs an insertion sort on the given array
        */
        public static T[] InsertionSort(T[] items)
        {
            for (int i = 1; i < items.Length; i++)
            {
                T item = items[i];
                int j = i - 1;
                while (j >= 0 && item.CompareTo(items[j]) < 0) //check until 0 and number is smaller
                {
                    items[j + 1] = items[j];
                    j--;
                }
                items[j + 1] = item;
            }
            return items;
        }

        /**
        * Runs a selection sort on the given array
        */
        public static T[] SelectionSort(T[] items)
        {
            for (int i = 0; i < items.Length - 1; i++)
            {
                int ind = i;
                T p = items[ind]; //pick first item
                for (int j = i + 1; j < items.Length; j++) //iterate through rest of array
                    if (items[j].CompareTo(p) < 0) //if smaller item is found
                    {
                        ind = j;
                        p = items[ind]; //update selection by swap
                    }
                items[ind] = items[i]; //swap once smallest new element is found
                items[i] = p;
            }
            return items;
        }

        /**
        * Runs a bubble sort on the given array
        */
        public static T[] BubbleSort(T[] items)
        {
            for (int i = 0; i < items.Length - 1; i++)
            {
                T p = items[i]; //pick item
                for (int j = i; j < items.Length; j++) //iterate through rest of array
                {
                    if (p.CompareTo(items[j]) > 0) //picked item is larger than other item
                    {
                        items[i] = items[j]; //swap them
                        items[j] = p;
                        p = items[i];
                    }
                }
            }
            return items;
        }

        #region MergeSort
        private static T[] mergeItems;

        /**
        * Runs a merge sort on the given array
        */
        public static T[] MergeSort(T[] items)
        {
            if (items.Length > 1)
            {
                mergeItems = items;
                stitchArray = new T[items.Length]; //create backup array
                MergeSplit(0, items.Length - 1);
            }

            return mergeItems;
        }

        private static void MergeSplit(int first, int last)
        {
            if (first < last)
            {
                int splitVal = (first + last) / 2; //find middle
                MergeSplit(first, splitVal); //first half
                MergeSplit(splitVal + 1, last); //second half
                MergeJoin(first, splitVal, last); //stitch them together
            }

        }

        private static T[] stitchArray;
        private static void MergeJoin(int first, int mid, int last)
        {
            int stichArrayLength = last + 1 - first; //limit allocation of array to total values

            int i1 = first;
            int i2 = mid + 1;

            int add = 0;
            while (add != stichArrayLength) //keep adding items until both sub-arrays are merged
            {
                if (i2 > last || (i1 <= mid && mergeItems[i1].CompareTo(mergeItems[i2]) < 0)) //list 2 is depleted or first element is smaller
                {
                    stitchArray[add] = mergeItems[i1];
                    i1++;
                }
                else if (i2 <= last) //second list is not depleted
                {
                    stitchArray[add] = mergeItems[i2];
                    i2++;
                }
                else //witchcraft may have occured
                    Console.WriteLine("Error");
                add++;
            }

            for (int i = 0; i < stichArrayLength; i++) //place merged items in appropriate array position
                mergeItems[first + i] = stitchArray[i];


        }
        #endregion

        #region QuickSort
        private static T[] quickItems;

        /**
        * Runs a quick sort on the given array
        */
        private static T[] QuickSort(T[] items)
        {
            if (items.Length > 1)
            {
                quickItems = items;
                QuickSplit(0, items.Length - 1);
            }
            return quickItems;
        }

        private static void QuickSplit(int first, int last)
        {
            if (first < last)
            {
                int splice = QuickSplice(first, last); //pivot point        
                QuickSplit(first, splice - 1);
                QuickSplit(splice + 1, last);
            }
        }

        private static int QuickSplice(int first, int last)
        {

            //method 1 of generating pivot (first element) //T pivot = quickItems[first];

            //method 2 uses median of 3 datapoints: (middle value of each third)
            T pleft = quickItems[first + (last - first) / 6];
            T pmid = quickItems[first + (last - first) / 2];
            T pright = quickItems[last - (last - first) / 6];

            T l_pivot = pleft.CompareTo(pmid) >= 0 ? pleft : pmid;
            T pivot = pright.CompareTo(l_pivot) > 0 ? pright : l_pivot;

            int left = first + 1;
            int right = last;

            while (left <= right)
            {
                while (left <= right && quickItems[left].CompareTo(pivot) <= 0)
                    left++;
                while (left <= right && quickItems[right].CompareTo(pivot) > 0)
                    right--;
                if (left <= right)//swap values about the pivot
                {
                    T p = quickItems[left];
                    quickItems[left] = quickItems[right];
                    quickItems[right] = p;
                }
            }
            quickItems[first] = quickItems[right];//bring last item of the left to the beginning
            quickItems[right] = pivot; //place "sorted" aka positioned pivot between the two sub-arrays
            return right;
        }
        #endregion

    }
}
