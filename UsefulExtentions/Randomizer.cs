using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace UsefulExtentions
{
    public class Randomizer
    {
        public static DataTable GetRandomOrder(int rowCount)
        {
            var table = new DataTable("RandomOrder");

            table.Columns.Add("id", typeof(double));
            table.Columns.Add("isbn", typeof(string));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("copies", typeof(double));
            table.Columns.Add("mount", typeof(string));
            table.Columns.Add("format", typeof(string));
            table.Columns.Add("lamination", typeof(string));
            table.Columns.Add("imposition", typeof(double));

            string[] isbnArr = GetRandomIsbn(rowCount);
            string[] nameArr = GetRandomName(rowCount);
            double[] copiesnArr = GetRandomNumbers(rowCount);
            string[] mountArr = GetRandomMount(rowCount);
            string[] formatArr = GetRandomFormat(rowCount);
            string[] laminationArr = GetRandomLamination(rowCount);
            double[] impositionArr = GetRandomImposition(rowCount);

            DataRow row;

            for (int i = 0; i < rowCount; i++)
            {
                row = table.NewRow();
                row["id"] = i;
                row["isbn"] = isbnArr[i];
                row["name"] = nameArr[i];
                row["copies"] = copiesnArr[i];
                row["mount"] = mountArr[i];
                row["format"] = formatArr[i];
                row["lamination"] = laminationArr[i];
                row["imposition"] = impositionArr[i];
                table.Rows.Add(row);
            }

            return table;
        }


        public static string[] GetRandomIsbn(int count)
        {
            var random = new Random();
            var isbnArr = new string[count];

            for(int i = 0; i < count; i++)
                isbnArr[i] = $"9-785-5-{random.Next(1000, 9999)}-{random.Next(1000, 9999)}-{random.Next(0, 9)}";

            return isbnArr;
        }

        public static string[] GetRandomName(int count, int lenght = 10)
        {
            var random = new Random();
            var nameArr = new string[count];
            var tempArr = new char[lenght];

            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < lenght; i++)
                {
                    tempArr[i] = (char)random.Next(97, 122);
                }

                nameArr[j] = String.Join("", tempArr);
            }
            
            return nameArr;
        }

        public static double[] GetRandomNumbers(int count)
        {
            var random = new Random();
            var numbersArr = new double[count];

            for (int i = 0; i < count; i++)
            {
                numbersArr[i] = random.Next(1, 20);
            }

            return numbersArr;
        }

        public static string[] GetRandomMount(int count)
        {
            var random = new Random();
            var mount = new string[2]{"кбс", "скрепка"};
            var tempArr = new string[count];

            for (int i = 0; i < count; i++)
            {
                tempArr[i] = mount[random.Next(0, 1)];
            }

            return tempArr;
        }

        public static string[] GetRandomFormat(int count)
        {
            var random = new Random();
            var format = new string[2] { "А5", "А4" };
            var tempArr = new string[count];

            for (int i = 0; i < count; i++)
            {
                tempArr[i] = format[random.Next(0, 1)];
            }

            return tempArr;
        }

        public static string[] GetRandomLamination(int count)
        {
            var random = new Random();
            var lamination = new string[2] { "глянец", "матовый" };
            var tempArr = new string[count];

            for (int i = 0; i < count; i++)
            {
                tempArr[i] = lamination[random.Next(0, 1)];
            }

            return tempArr;
        }

        public static double[] GetRandomImposition(int count)
        {
            var random = new Random();
            var tempArr = new double[count];

            for (int i = 0; i < count; i++)
            {
                tempArr[i] = random.Next(1, 2);
            }

            return tempArr;
        }
    }
}
