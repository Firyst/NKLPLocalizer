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
        // ������� ��� ������ ���������� ���������� ��� ���������� �����
        if (moreDebug)
        {
            Debug.Log(str);
        }
    }

    public class LField
    {
        // ���������, ������� ������ ����� � ��� ��������� ��������.
        public Text TextObject;
        public string LocaleKey;

        public LField(Text TextObject, string LocaleKey)
        {
            this.TextObject = TextObject;
            this.LocaleKey = LocaleKey;
        }
    }

    [SerializeField] bool moreDebug = true; // ����������� �������
    public bool dontDestroyOnLoad = false;  // �� ������� ������ ��� ����� �����
    public List<LField> all_texts = new List<LField>();  // ���������� ��� ���� �������� Lfield
    public string lang = "en"; // ����
    public Dictionary<string, Dictionary<string, string>> locales = new Dictionary<string, Dictionary<string, string>>();

    // ������������� �����������. ��� ���� ��� ������ �� �����.
    void Start()
    {
        var FoundTexts = FindObjectsOfType(typeof(Text)); // ���� ������ �� ��������

        foreach (Text FoundText in FoundTexts)
        {
            // ����������
            all_texts.Add(new LField(FoundText, FoundText.text));
        }
        print_debug(string.Format("NKLP Localizer: <color=green>found {0} text objects.</color>", all_texts.Count));

        // �������� �������� ���������
        if (PlayerPrefs.HasKey("lang"))
        {
            if (lang != "" && lang != "auto")
            {
                // ���� �������� (�� ����)
                if (lang != PlayerPrefs.GetString("lang") && lang.Length > 0)
                {
                    // ���� ������ �������� ������, �� ������ 
                    PlayerPrefs.SetString("lang", lang);
                }
            }
            // �������� ����
            lang = PlayerPrefs.GetString("lang");
            print_debug(string.Format("NKLP Localizer: current language: {0}", lang));
        }
        else
        {
            // ���� ���� �������� ���������, �� ������ �������
            PlayerPrefs.SetString("lang", lang);
            Debug.Log(string.Format("NKLP Localizer: No language setting found! Setting PlayerPref with key 'lang' to {0}", lang));
        }

        var locale = Resources.Load<TextAsset>("Locale"); // ��������� ���� �����������

        if (locale == null)
        {
            Debug.LogError("NKLP Localizer: No Localization file found!");
        }

        string[] lines = locale.text.Split('\n'); // ������� ���� �� ��������

        string[] locales_array = lines[0].Split(";");  // �������� ������ ������

        // ��������� ����������� �������� �����
        if (!locales_array.Contains(lang))
        {
            // �������� ����
            Debug.LogError(string.Format("NKLP Localizer: language {0} is not in file", lang));
            PlayerPrefs.SetString("lang", locales_array[1]);  // ��������� ��������� ���� � ������ ������
            lang = locales_array[1];
        }

        for (int locale_i = 1; locale_i < locales_array.Length; locale_i++)
        {
            locales.Add(locales_array[locale_i], new Dictionary<string, string>()); // ��������� ������� ������� ����������� ��������� ��� ���������
        }


        long locale_lines_count = 0; // ������� ����������� �����
        for (int key_i = 1; key_i < lines.Length; key_i++)
        {
            // ������ ����� �����������
            string[] locale_line = lines[key_i].Split(";");
            if (locale_line.Length > 2)
            {
                for (int locale_i = 1; locale_i < locales_array.Length; locale_i++)
                {
                    // ������ �������� ��� ������� �����
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
        // ������� ��� ������ ����������� ����� ���������� ������
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
        // ������� ���������� ������ �� ������� ����� �� �����
        Dictionary<string, string> current_locale = locales[lang];

        if (current_locale.Keys.Contains(key))
        {
            return current_locale[key];
        } 
        else
        {
            // �������� ����
            Debug.LogError(string.Format("NKLP Localizer: No such key ({0})", key));
            return "<ERROR>";
        }
    }

    public void ChangeLang(string new_lang)
    {
        // �������� ����
        if (locales.Keys.Contains(new_lang))
        {
            PlayerPrefs.SetString("lang", new_lang);
            print_debug(string.Format("NKLP Localizer: <color=aqua>language changed to {0}.</color>", new_lang));
            LocalizeAllText();
        } else
        {
            // ������ �������� ����
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
