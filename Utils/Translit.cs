using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Intranet.Utils
{
    public class Translit
    {
        public static string TranslitName(string fn)
        {
            string[] RussianTable = {"Аа", "Бб", "Вв", "Гг", "Дд", "Ее", "Ёё", "Жж", "Зз", "Ии",
										"Йй", "Кк", "Лл", "Мм", "Нн", "Оо", "Пп", "Рр", "Сс", "Тт",
										"Уу", "Фф", "Хх", "Цц", "Чч", "Шш", "Щщ", "Ъъ", "Ыы", "Ьь",
										"Ээ", "Юю", "Яя", " "};

            string[] EnglishTable = {"a", "b", "v", "g", "d", "e", "e", "zh", "z", "i",
										"y", "k", "l", "m", "n", "o", "p", "r", "s", "t",
										"u", "f", "kh", "ts", "ch", "sh", "shch", "'", "y", "'",
										"e", "yu", "ya", "_"};

            StringBuilder sb = new StringBuilder("");

            for (int index = 0; index < fn.Length; index++)
            {
                char lsFn = fn[index];
                bool Russian = false;
                int position = -1;
                for (int i = 0; i < RussianTable.Length; i++)
                {
                    if (RussianTable[i].IndexOf(lsFn) > -1)
                    {
                        position = i;
                        Russian = true;
                        break;
                    }
                }
                if (Russian == true)
                    sb.Append(EnglishTable[position]);
                else
                    sb.Append(lsFn);
            }

            return sb.ToString(); 
        }
    }
}
