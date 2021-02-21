using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Utility.Utils;

namespace Utility.Collections
{
    /// <summary>
    /// 排序 帮助类型
    /// </summary>

    public class SortHelper
    {
        #region static

        /// <summary>
        /// a=10,b=15，在不用第三方变量的前提下，把a,b的值互换
        /// </summary>
        public static void SubjectOne<T>(ref  T obj1,ref  T obj2)
        {
            Type type = typeof(T);
            //是数字情况下不适用第三方变量
            if (TypeUtils.IsNumber(type))
            {
                decimal a = (decimal)Convert.ChangeType(obj1, typeof(decimal));
                decimal b = (decimal)Convert.ChangeType(obj2, typeof(decimal));
                a += b;//a+b
                b = a - b;//a a+b-b
                a = a - b;//b a+b-a
                obj1 = unchecked((T)Convert.ChangeType(a,type));
                obj2 = unchecked((T)Convert.ChangeType(b,type));
            }
            else
            {
                var temp = obj1;
                obj1 = obj2;
                obj2 = temp;
            }

        }

        /// <summary>
        /// 冒泡 排序 2次遍历  第2次遍历  一直相邻比较换位置 o(n^2)
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void BubbleSort<Entity>(Entity[] objs, IComparer<Entity> comparer=null,int length=-1)
        {
            comparer = comparer ?? Comparer<Entity>.Default;
            length = length == -1 ? objs.Length : length;
            for (int i = 0;  i < length; i++)
            {
                var isSort = true;
                for (int j = length-1; j > i; j--)
                {
                    //方案1 相邻 2 个 元素 比较 (可能 重复比较)
                    int res = comparer.Compare(objs[j], objs[j-1]);
                    if (res < 0)
                    {
                        var min = objs[j-1];
                        objs[j - 1] = objs[j];
                        objs[j] = min;
                        isSort = false;
                    }

                    //方案2 某 元素(符合会实时更新) 跟 任何一个元素比较
                    //int res = comparer.Compare(objs[i], objs[j]);
                    //if (res > 0)
                    //{
                    //    var min = objs[j];
                    //    objs[j] = objs[i];
                    //    objs[i] = min;
                    //    isSort = false;
                    //}
                }
                if (isSort)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 选择 排序 2次遍历 第2次遍历找到最小元素 替换 o(n^2)
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void SelectionSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null,int length=-1)
        {
            length = length == -1 ? objs.Length : length;
            comparer = comparer ?? Comparer<Entity>.Default;
            for (int i = 0; i < length; i++)
            {
           
                int j = i+1;
                int minIndex = -1;
                Entity min= objs[i];
                for (; j < length; j++)
                {
                    int res = comparer.Compare(min, objs[j]);
                    if (res > 0)
                    {
                        min = objs[j];
                        minIndex = j;
                    }
                }
                if (minIndex != -1)
                {
                    objs[minIndex] = objs[i];
                    objs[i] = min;
                }
            }
        }


        /// <summary>
        /// 插入 排序(1000以下效率快,多用 二分 插入 排序) 2次遍历 第2次遍历 匹配元素 则插入 否则终止 o(n) bad: o(n^2)
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void InsertionSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null,int length=-1)
        {
            length = length == -1 ? objs.Length : length;
            comparer = comparer ?? Comparer<Entity>.Default;
            for (int i = 1; i < length; ++i)
            {
                Entity value = objs[i];
                int j;//插入的位置
                for (j = i - 1; j >= 0; j--)
                {
                    if (comparer.Compare(objs[j] , value)>0)
                    {
                        objs[j + 1] = objs[j];//移动数据
                    }
                    else
                    {
                        break;
                    }
                }
                objs[j + 1] = value; //插入数据
            }
        }


        /// <summary>
        /// 二分 插入 排序 2次遍历 第2次遍历 匹配元素 则插入 否则终止 o(n) bad: o(n^2)
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void BinaryInsertionSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null, int length = -1)
        {
            length = length == -1 ? objs.Length : length;
            comparer = comparer ?? Comparer<Entity>.Default;
            int left, right, middle;
            for (int i = 1; i < length; i++)
            {
                Entity key = objs[i];
                left = 0;
                right = i - 1;
                while (left <= right)
                {
                    middle = (left + right) / 2;
                    if (comparer.Compare(objs[middle],key)>0)
                        right = middle - 1;
                    else
                        left = middle + 1;
                }
                for (int j = i - 1; j >= left; j--)
                {
                    objs[j + 1] = objs[j];
                }
                objs[left] = key;
            }
        }

        /// <summary>
        /// 希尔 排序  类似 插入排序
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="gap">2 or 3 其他基数 排序后结果可能不对 1死循环 </param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void ShellSort<Entity>(Entity[] objs,int gap=2, IComparer<Entity> comparer = null,int length=-1)
        {
            length = length == -1 ? objs.Length : length;
            comparer = comparer ?? Comparer<Entity>.Default;
            for (int r = length / gap; r >= 1; r /= gap)//r>=1保证至少有两个可以比较的数，r/=2确定序列对
            {
                for (int i = r; i < length; i++ )//插入排序算法
                {
                    Entity temp = objs[i];
                    int j = i - r;
                    for (; j >= 0 && comparer.Compare(temp , objs[j])<0; objs[j + r] = objs[j],j -= r)//对配对的两个数进行比较
                    {
                    }
                    objs[j + r] = temp;
                }
            }

        }

        /// <summary>
        /// 归并排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void MergeSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null, int length = -1)
        {
            length = length == -1 ? objs.Length : length;
            Entity[] tempObjs = new Entity[length];
            MergeSort(objs, 0, length-1, tempObjs, comparer);

        }

        /// <summary>
        /// 归并排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="low">0</param>
        /// <param name="high"> objs.Length-1</param>
        /// <param name="tempObjs"></param>
        public static void MergeSort<Entity>(Entity[] objs, int low, int high,Entity[] tempObjs, IComparer<Entity> comparer = null)
        {
            if (low >= high)
                return;
            int mid = (low + high) / 2;
            MergeSort(objs, low, mid, tempObjs,comparer);
            MergeSort(objs, mid + 1, high, tempObjs, comparer);
            comparer = comparer ?? Comparer<Entity>.Default;
            // 合并两个有序序列
            int length = 0; // 表示辅助空间有多少个元素
            int i_start = low;
            int i_end = mid;
            int j_start = mid + 1;
            int j_end = high;
            while (i_start <= i_end && j_start <= j_end)
            {
                if (comparer.Compare(objs[i_start] , objs[j_start])<0)
                {
                    tempObjs[length] = objs[i_start];
                    length++;
                    i_start++;
                }
                else
                {
                    tempObjs[length] = objs[j_start];
                    length++;
                    j_start++;
                }
            }
            while (i_start <= i_end)
            {
                tempObjs[length] = objs[i_start];
                i_start++;
                length++;
            }
            while (j_start <= j_end)
            {
                tempObjs[length] = objs[j_start];
                length++;
                j_start++;
            }
            // 把辅助空间的数据放到原空间
            for (int i = 0; i < length; i++)
            {
                objs[low + i] = tempObjs[i];
            }

        }
        
        /// <summary>
        /// 堆排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void HeapSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null, int length = -1)
        {
            length = length==-1? objs.Length:length;
            comparer = comparer ?? Comparer<Entity>.Default;
            //构建堆
            for (int i = length / 2; i >= 0; i--)
            {
                HeapAdjust(objs, i, length, comparer);
            }
            for (int i = length - 1; i > 0; i--)
            {
                //将堆顶元素与末位元素调换
                Entity temp = objs[0];
                objs[0] = objs[i];
                objs[i] = temp;
                length--;//数组长度-1 隐藏堆尾元素
                HeapAdjust(objs, 0, length, comparer); //将堆顶元素下沉 目的是将最大的元素浮到堆顶来
            }
        }

        private static void HeapAdjust<Entity>(Entity[] objs, int i, int length, IComparer<Entity> comparer = null)
        {
            // 调整i位置的结点
            int max = i; // 先保存当前结点的下标
            // 当前结点左右孩子结点的下标
            int lchild = i * 2 + 1;
            int rchild = i * 2 + 2;
            if (lchild < length && comparer.Compare(objs[lchild] , objs[max])>0)
            {
                max = lchild;
            }
            if (rchild < length && comparer.Compare(objs[rchild] ,objs[max])>0)
            {
                max = rchild;
            }
            // 若i处的值比其左右孩子结点的值小，就将其和最大值进行交换
            if (max != i)
            {
                Entity temp = objs[i];
                objs[i] = objs[max];
                objs[max] = temp;
                // 递归
                HeapAdjust(objs, max, length,comparer);
            }
        }



        /// <summary>
        /// 计数排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void CountingSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null, int length = -1)
        {
            //length = length == -1 ? objs.Length : length;
            //int[] indexs = new int[length];
            //for (int i = 0; i < length; i++)
            //{
            //    indexs[i] = i;
            //}
            //CountingSort(indexs);
            //for (int i = 0; i < length; i++)
            //{
            //    Console.Write(indexs[i]+" ");
            //}
        }

        /// <summary>
        /// 计数排序(基于值大于0条件排序):重复的元素 会 去重, 基于整数排序
        /// </summary>
        /// <param name="arr"></param>
        public static void CountingSort(int[] arr)
        {
            //找出数组中的最大值
            int max = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
            }
            //初始化计数数组
            int[] countArr = new int[max + 1];
            //计数
            for (int i = 0; i < arr.Length; i++)
            {
                countArr[arr[i]]++;
                arr[i] = 0;
            }
            //排序
            int index = 0;
            for (int i = 0; i < countArr.Length; i++)
            {
                if (countArr[i] > 0)
                {
                    arr[index++] = i;
                }
            }
        }

        /// <summary>
        /// 桶排序 基于整数排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void BucketSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null, int length = -1)
        {

        }

        /// <summary>
        /// 桶排序 基于整数排序
        /// </summary>
        /// <param name="arr"></param>
        public static void BucketSort(int[] arr)
        {

            //最大最小值
            int max = arr[0];
            int min = arr[0];
            int length = arr.Length;

            for (int i = 1; i < length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
                else if (arr[i] < min)
                {
                    min = arr[i];
                }
            }

            //最大值和最小值的差
            int diff = max - min;

            //桶列表
            List<List<int>> bucketList = new List<List<int>>();
            for (int i = 0; i < length; i++)
            {
                bucketList.Add(new List<int>());
            }

            //每个桶的存数区间
            float section = (float)diff / (float)(length - 1);

            //数据入桶
            for (int i = 0; i < length; i++)
            {
                //当前数除以区间得出存放桶的位置 减1后得出桶的下标
                int num = (int)(arr[i] / section) - 1;
                if (num < 0)
                {
                    num = 0;
                }
                bucketList[num].Add(arr[i]);
            }

            //桶内排序
            for (int i = 0; i < bucketList.Count; i++)
            {
                //jdk的排序速度当然信得过
                bucketList[i].Sort();
            }

            //写入原数组
            int index = 0;
            foreach (List<int> arrayList in  bucketList)
            {
                foreach (int value in arrayList)
                {
                    arr[index] = value;
                    index++;
                }
            }
        }


        /// <summary>
        /// 基数排序 基于整数排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs"></param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void RadixSort<Entity>(Entity[] objs, IComparer<Entity> comparer = null, int length = -1)
        {

        }

        /// <summary>
        /// 基数排序
        /// </summary>
        /// <param name="arr"></param>
        public static void RadixSort(int[] arr)
        {
            int length = arr.Length;

            //最大值
            int max = arr[0];
            for (int i = 0; i < length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
            }
            //当前排序位置
            int location = 1;

            //桶列表
            List<List<int>> bucketList = new List<List<int>>();

            //长度为10 装入余数0-9的数据
            for (int i = 0; i < 10; i++)
            {
                bucketList.Add(new List<int>());
            }

            while (true)
            {
                //判断是否排完
                int dd = (int)Math.Pow(10, (location - 1));
                if (max < dd)
                {
                    break;
                }

                //数据入桶
                for (int i = 0; i < length; i++)
                {
                    //计算余数 放入相应的桶
                    int number = ((arr[i] / dd) % 10);
                    bucketList[number].Add(arr[i]);
                }

                //写回数组
                int nn = 0;
                for (int i = 0; i < 10; i++)
                {
                    int size = bucketList[i].Count;
                    for (int ii = 0; ii < size; ii++)
                    {
                        arr[nn++] = bucketList[i][ii];
                    }
                    bucketList[i].Clear();
                }
                location++;
            }
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs">数组</param>
        /// <param name="comparer"></param>
        /// <param name="length"></param>
        public static void QuickSort<Entity>(Entity[] objs, IComparer<Entity> comparer=null,int length=-1)
        {
            length = length == -1 ? objs.Length : length;
            QuickSort(objs, 0, length - 1, comparer);
        }
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="objs">数组</param>
        /// <param name="low">低端 数组第一个记录</param>
        /// <param name="high">高端 数组最后一个记录</param>
        /// <param name="comparer"></param>
        public static void QuickSort<Entity>(Entity[] objs, int low, int high, IComparer<Entity> comparer=null)
        {
            if (low >= high)
                return;
            int i = low;
            comparer = comparer ?? Comparer<Entity>.Default;
            int j = high;
            // 基准数
            var baseval = objs[low];
            while (i < j)
            {
                // 从右向左找比基准数小的数
                while (i < j && comparer.Compare(objs[j] , baseval)>=0)
                {
                    j--;
                }
                if (i < j)
                {
                    objs[i] = objs[j];
                    i++;
                }
                // 从左向右找比基准数大的数
                while (i < j && comparer.Compare(objs[i] , baseval)<0)
                {
                    i++;
                }
                if (i < j)
                {
                    objs[j] = objs[i];
                    j--;
                }
            }
            // 把基准数放到i的位置
            objs[i] = baseval;
            // 递归
            QuickSort(objs, low, i - 1,comparer);
            QuickSort(objs, i + 1, high, comparer);
        }

        #endregion  static method
    }
}
