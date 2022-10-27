using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class NKLPLocalizer : MonoBehaviour

{
    private void print_debug(string str)
    {
        // функция для вывода отладочной информации при включенной опции
        if (moreDebug)
        {
            Debug.Log(str);
        }
    }

    public class LField
    {
        // структура, которая хранит текст и его начальное значение.
        public Text TextObject;
        public string LocaleKey;

        public LField(Text TextObject, string LocaleKey)
        {
            this.TextObject = TextObject;
            this.LocaleKey = LocaleKey;
        }
    }

    [SerializeField] bool moreDebug = true; // расширенная отладка
    public bool dontDestroyOnLoad = false;  // не удалять объект при смене сцены
    public List<LField> all_texts = new List<LField>();  // переменная для всех структур Lfield
    public string lang = "en"; // язык
    public Dictionary<string, Dictionary<string, string>> locales = new Dictionary<string, Dictionary<string, string>>();

    // Инициализация локализации. Код ищет все тексты на сцене.
    void Start()
    {
        var FoundTexts = FindObjectsOfType(typeof(Text)); // ищем тексты на канвасах

        foreach (Text FoundText in FoundTexts)
        {
            // запомниаем
            all_texts.Add(new LField(FoundText, FoundText.text));
        }
        print_debug(string.Format("NKLP Localizer: <color=green>found {0} text objects.</color>", all_texts.Count));

        // загрузка языковой настройки
        if (PlayerPrefs.HasKey("lang"))
        {
            if (lang != "" && lang != "auto")
            {
                // язык прописан (не авто)
                if (lang != PlayerPrefs.GetString("lang") && lang.Length > 0)
                {
                    // если сейчас сохранен другой, то меняем 
                    PlayerPrefs.SetString("lang", lang);
                }
            }
            // выбираем язык
            lang = PlayerPrefs.GetString("lang");
            print_debug(string.Format("NKLP Localizer: current language: {0}", lang));
        }
        else
        {
            // если нету языковой настройки, то ставим текущую
            PlayerPrefs.SetString("lang", lang);
            Debug.Log(string.Format("NKLP Localizer: No language setting found! Setting PlayerPref with key 'lang' to {0}", lang));
        }

        var locale = Resources.Load<TextAsset>("Locale"); // прочитать файл локализации

        if (locale == null)
        {
            Debug.LogError("NKLP Localizer: No Localization file found!");
        }

        string[] lines = locale.text.Split('\n'); // разбить файл по строчкам

        string[] locales_array = lines[0].Split(";");  // получить список языков

        // проеверка коректности текущего языка
        if (!locales_array.Contains(lang))
        {
            // неверный язык
            Debug.LogError(string.Format("NKLP Localizer: language {0} is not in file", lang));
            PlayerPrefs.SetString("lang", locales_array[1]);  // выставить дефолтный язык в случае ошибки
            lang = locales_array[1];
        }

        for (int locale_i = 1; locale_i < locales_array.Length; locale_i++)
        {
            locales.Add(locales_array[locale_i], new Dictionary<string, string>()); // заполнить главный словарь локализации словарями для переводов
        }


        long locale_lines_count = 0; // счетчик загруженных строк
        for (int key_i = 1; key_i < lines.Length; key_i++)
        {
            // чтение файла локализации
            string[] locale_line = lines[key_i].Split(";");
            if (locale_line.Length > 2)
            {
                for (int locale_i = 1; locale_i < locales_array.Length; locale_i++)
                {
                    // запись перевода для каждого языка
                    locales[locales_array[locale_i]].Add(locale_line[0], locale_line[locale_i]);
                }
                locale_lines_count++;
            }
        }

        print_debug(string.Format("NKLP Localizer: <color=green> Loaded {0} lines of localization! </color>", locale_lines_count));

        LocalizeAllText();
    }

    public void LocalizeAllText()
    {
        // функция для полной локализации всего найденного текста
        lang = PlayerPrefs.GetString("lang");

        Dictionary<string, string> current_locale = locales[lang];

        foreach (LField text in all_texts)
        {
            if (current_locale.Keys.Contains(text.LocaleKey))
            {
                text.TextObject.text = current_locale[text.LocaleKey];
            }
        }
    }

    public string GetString(string key)
    {
        // функция возвращает строку на текущем языке по ключу
        Dictionary<string, string> current_locale = locales[lang];

        if (current_locale.Keys.Contains(key))
        {
            return current_locale[key];
        } 
        else
        {
            // неверный ключ
            Debug.LogError(string.Format("NKLP Localizer: No such key ({0})", key));
            return "<ERROR>";
        }
    }

    public void ChangeLang(string new_lang)
    {
        // поменять язык
        if (locales.Keys.Contains(new_lang))
        {
            PlayerPrefs.SetString("lang", new_lang);
            print_debug(string.Format("NKLP Localizer: <color=aqua>language changed to {0}.</color>", new_lang));
            LocalizeAllText();
        } else
        {
            // такого перевода нету
            Debug.LogError(string.Format("NKLP Localizer: language {0} is not in file", new_lang));
        }
    }

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            // Make localizer persistent
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
