using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsk
{
    class ShortestWayFounder
    {
        public int[,] arr;
        public int from;
        public int too;

        public ShortestWayFounder(int[,] arr, int from, int too)
        {
            this.arr = arr;
            this.from = from;
            this.too = too;
        }

        public List<string> CalculateShortestWay()
        {
            List<Vertices> checkVerticesList = new List<Vertices>(arr.GetLength(1) - 1);
            for (int i = 0; i < arr.GetLength(1) - 1; i++)
            {
                checkVerticesList.Add(new Vertices(int.MaxValue, false));
            }
            checkVerticesList.Insert(0, new Vertices(0, true));

            int currentHighest = from;

            while (currentHighest != too)
            {
                int minWay = int.MaxValue;
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[currentHighest, j] != 9999 && arr[currentHighest, j] != int.MaxValue)
                    {
                        checkVerticesList[j].value = Math.Min(checkVerticesList[j].value, arr[currentHighest, j] + checkVerticesList[currentHighest].value);
                        if (checkVerticesList[j].value < minWay && checkVerticesList[j].fill == false)
                        {
                            minWay = checkVerticesList[j].value;
                        }
                    }
                }

                List<Vertices> changeWayList = new List<Vertices>();
                for (int i = 0; i < checkVerticesList.Count; i++)
                {
                    if ((checkVerticesList[i].value <= minWay &&
                        checkVerticesList[i].fill == false) ||
                        (checkVerticesList[i].fill == false &&
                        checkVerticesList[i].value < minWay))
                    {
                        checkVerticesList[i].fill = true;
                        currentHighest = i;
                        break;
                    }
                }
            }

            List<string> way = new List<string>();
            int reverseWayVertices = too;
            while (reverseWayVertices != 0)
            {
                int minReverseWayValue = int.MaxValue;
                int minReverseWayVertices = too;
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    if (arr[i, reverseWayVertices] != int.MaxValue &&
                        arr[i, reverseWayVertices] != 9999 &&
                        arr[i, reverseWayVertices] + checkVerticesList[reverseWayVertices - 1].value < minReverseWayValue)
                    {
                        minReverseWayValue = arr[i, reverseWayVertices] + checkVerticesList[reverseWayVertices - 1].value;
                        minReverseWayVertices = i;
                    }
                }
                way.Add((reverseWayVertices + 1) + "величина пути " + minReverseWayValue);
                reverseWayVertices = minReverseWayVertices;
            }
            return way;
        }
    }
}