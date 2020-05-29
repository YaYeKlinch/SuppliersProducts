using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SuppliersProducts.Controllers
{
    internal class Plagiator
    {
        int MIN_LCS_LENGTH = 15; //мінімальна довжина однакової ділянки коду, яка свідчить про плагіат
        public double LongestCommonSubstringTest(string test, string other)//модифікований метод найдовшого спільного рядка
                                                                           //test - рядок, який перевіряємо на плагіат
                                                                           //other - рядок, із яким перевіряємо
        {
            int originalLength = test.Length;//початкова довжина рядка
            int lcsLength;//довжина найдовшого спільного рядка (НСР)
            do//повторюємо дії, поки НСР не буде закоротким
            {
                int n = test.Length;
                int m = other.Length;
                int[,] matr = new int[n, m];
                lcsLength = 0;
                int maxI = 0;
                for (int i = 0; i < n; i++)//заповнює матрицю пошуку НСР
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (test[i] == other[j])
                        {
                            matr[i, j] = (i == 0 || j == 0) ? 1 : matr[i - 1, j - 1] + 1;
                            if (matr[i, j] > lcsLength)
                            {
                                lcsLength = matr[i, j];
                                maxI = i;
                            }
                        }
                    }
                }

                if (lcsLength > 0)//якщо НСР знайдено
                {
                    string lcs = test.Substring(maxI + 1 - lcsLength, lcsLength);//знаходимо цей НСР
                    test = test.Replace(lcs, "");//видаляємо з рядка, який перевіряємо, всі входження НСР
                    other = other.Remove(other.IndexOf(lcs), lcsLength);//із рядка, з яким перевіряємо, видаляємо лише перше входження НСР
                }
            }
            while (lcsLength >= MIN_LCS_LENGTH);//повторюємо дії, поки НСР не буде закоротким

            //коефіцієнт плагіату знаходиться як відношення довжини унікальної частини тексту до всієї довжини рядка
            return 1.0 - (double)test.Length / originalLength;
        }

        public double WShinglingTest(string test, string other)//метод шинглів
        {
            //шукаємо шингли рядка test та відразу рахуємо їхній хеш
            int testCountShingles = test.Length - MIN_LCS_LENGTH + 1;
            List<int> testShingles = new List<int>();
            for (int i = 0; i < testCountShingles; ++i)
            {
                testShingles.Add(test.Substring(i, MIN_LCS_LENGTH).GetHashCode());
            }

            //шукаємо шингли рядка other та відразу рахуємо їхній хеш
            int otherCountShingles = other.Length - MIN_LCS_LENGTH + 1;
            List<int> otherShingles = new List<int>();
            for (int i = 0; i < otherCountShingles; ++i)
            {
                otherShingles.Add(other.Substring(i, MIN_LCS_LENGTH).GetHashCode());
            }

            //коефіцієнт плагіату знаходиться як відношення кількості однакових хешів до кількості всіх хешів рядка test
            return (double)testShingles.Intersect(otherShingles).Count() / testShingles.Distinct().Count();
        }

        public double AveragePlagTest(string test, string other)//середнє арифметичне обох методів
        {
            return (LongestCommonSubstringTest(test, other) + WShinglingTest(test, other)) / 2;
        }
    }

    internal class Normalizator
    {
        string separator = "";
        static List<string> punctuator = new List<string>();//роздільники, записані у файлі граматики
        static List<string> programKeywords = new List<string>();//усі ключові слова, які є в програмі
        static List<string> numbers = new List<string>();
        static Dictionary<string, string> keywordsDictionary = new Dictionary<string, string>();

        public string GetNormalizedCode(string code)
        {
            string filePath = "C:\\Users\\Ihor\\Documents\\GitHub\\SuppliersProducts\\SuppliersProducts\\Controllers\\Grammar.txt";//розташування файлу з граматикою (роздільники + ключові слова)
            ReadGrammar(filePath);//зчитуємо граматику
            FindProgramKeywords(code);//шукаємо всі ключові слова, які є в програмі
            FillDictionary();//заповнюємо словник ключове слово - токен
            return NormalizeCode(code);//повертаємо нормалізований код
        }

        private string NormalizeCode(string code)//виконує нормалізацію коду
        {
            code = RemoveComments(code);//видалення коментарів
            code = RemoveText(code);//видалення не програмного тексту

            //кожне ключове слово замінюється на відповідний йому токен
            programKeywords.Sort(CompareStringLength);
            foreach (string s in programKeywords)
            {
                if (s.Length > 1)
                {
                    code = code.Replace(s, separator + keywordsDictionary[s]);
                }
                else
                {
                    if (keywordsDictionary.ContainsValue(s))
                    {
                        code = code.Replace(separator + s, separator + separator);
                        code = code.Replace(s, separator + keywordsDictionary[s]);
                        code = code.Replace(separator + separator, separator + s);
                    }
                    else
                    {
                        code = code.Replace(s, separator + keywordsDictionary[s]);
                    }
                }
            }

            //числа замінюються на токен, який відповідає типу int
            numbers.Sort(CompareStringLength);
            foreach (string s in numbers)
                code = code.Replace(s, "#number#");
            code = code.Replace("#number#.#number#", "#number#");
            code = code.Replace("#number#,#number#", "#number#");
            code = code.Replace("#number#", separator + keywordsDictionary["int"]);
            code = code.Replace(separator, "");

            //видалення відступів
            code = code.Replace(" ", "");
            code = code.Replace("\n", "");
            code = code.Replace("\r", "");

            return code;
        }

        private int CompareStringLength(string a, string b)//порівнює довжину двох рядків
        {
            if (a.Length == b.Length)
                return 0;
            if (a.Length > b.Length)
                return -1;
            return 1;
        }

        private void FillDictionary()//заповнює словник токенів
        {
            bool allKeyworsAdded = true;
            do
            {
                for (int i = 1; i < programKeywords.Count; ++i)
                {
                    if (!keywordsDictionary.ContainsKey(programKeywords[i]))
                    {
                        if (keywordsDictionary.ContainsKey(programKeywords[i - 1]))
                        {
                            keywordsDictionary.Add(programKeywords[i], keywordsDictionary[programKeywords[i - 1]]);
                        }
                        else
                        {
                            if (keywordsDictionary.ContainsKey(programKeywords[i + 1]))
                            {
                                keywordsDictionary.Add(programKeywords[i], keywordsDictionary[programKeywords[i + 1]]);
                            }
                            else
                            {
                                allKeyworsAdded = false;
                            }
                        }
                    }
                }
            }
            while (!allKeyworsAdded);
        }

        private void ReadGrammar(string path)//зчитує граматику з файлу
        {
            StreamReader sr = new StreamReader(path);

            separator = sr.ReadLine();//роздільник у файлі з граматикою

            //зчитує роздільники в мові програмування
            string line = sr.ReadLine();
            while (!line.Equals(separator))
            {
                punctuator.Add(line);
                line = sr.ReadLine();
            }

            //зчитує всі стандартні ключові слова
            line = sr.ReadLine();
            while (!line.Equals(separator))
            {
                string[] spl = line.Split(separator);
                string[] buf = spl[0].Split(' ');
                if (spl.Length > 1)
                {
                    foreach (string s in buf)
                    {
                        if (!keywordsDictionary.ContainsKey(s))
                            keywordsDictionary.Add(s, spl[1]);
                    }
                }
                line = sr.ReadLine();
            }

            sr.Close();
        }

        private void FindProgramKeywords(string code)//пошук усіх використаних у програмі ключових слів
        {
            code = RemoveComments(code);//видалення коментарів
            code = RemoveText(code);//видалення не програмного тексту
            code = code.Replace("\n", " ");//видалення абзаців
            code = code.Replace("\r", " ");
            foreach (string s in punctuator)//видалення роздільників мови програмування
                code = code.Replace(s, " ");

            string[] splitCode = code.Split(' ');//тут знаходяться всі використані ключові слова
            foreach (string s in splitCode)//знаходить всі унікальні ключові слова
            {
                if (s.Length > 0 && !programKeywords.Contains(s))
                {
                    if (!IsNumber(s))
                    {
                        programKeywords.Add(s);
                    }
                    else
                    {
                        if (!numbers.Contains(s))
                            numbers.Add(s);
                    }
                }
            }
        }

        private bool IsNumber(string n)//перевірка, чи є рядок числом
        {
            if (!Char.IsNumber(n[0]))
                return false;
            for (int i = 1; i < n.Length; ++i)
                if (!Char.IsNumber(n[i]))
                    return false;
            return true;
        }

        private string RemoveComments(string code)//видаляє коментарі
        {
            //видалення коментарів виду //текст
            while (code.Contains("//"))
            {
                int i = code.IndexOf("//");
                string buf1 = code.Substring(0, i);
                string buf2 = code.Remove(0, i + 2);
                i = buf2.IndexOf("\n");
                if (i != -1) buf2 = buf2.Remove(0, i);
                code = buf1 + buf2;
            }

            //видалення коментарів виду /*текст*/
            while (code.Contains("/*"))
            {
                int i = code.IndexOf("/*");
                string buf1 = code.Substring(0, i);
                string buf2 = code.Remove(0, i + 2);
                i = buf2.IndexOf("*/");
                if (i != -1) buf2 = buf2.Remove(0, i + 2);
                code = buf1 + "\n" + buf2;
            }

            return code;
        }

        private string RemoveText(string code)//видаляє не програмний текст
        {
            //видалення тексту виду "текст"
            code = code.Replace("\\\"", " ");
            while (code.Contains("\""))
            {
                int i = code.IndexOf("\"");
                string buf1 = code.Substring(0, i);
                string buf2 = code.Remove(0, i + 1);
                i = buf2.IndexOf("\"");
                if (i != -1) buf2 = buf2.Remove(0, i + 1);
                code = buf1 + buf2;
            }

            //видалення символів виду 'с'
            code = code.Replace("\\\'", " ");
            while (code.Contains("\'"))
            {
                int i = code.IndexOf("\'");
                string buf1 = code.Substring(0, i);
                string buf2 = code.Remove(0, i + 1);
                i = buf2.IndexOf("\'");
                if (i != -1) buf2 = buf2.Remove(0, i + 1);
                code = buf1 + buf2;
            }

            return code;
        }
    }
}