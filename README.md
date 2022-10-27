# NKLPLocalizer
Custom Unity localizer for personal needs.


[RU]

Описание: 
Скрипт ищет на уровне объекты типа Text и заменяет ключ на строку выбранной локализации.
Также можно получить строку по ключу напрямую через скрипт.
Язык сохраняется автоматически в PlayerPrefs.

Использование:
1. Скачать файлы и добавить их в корень Unity проекта.
2. На нужную сцену добвить префаб Localizer.
3. Заполнить файл локализации Resources/locale.csv 
   Каждый столбец - это язык, первый столбец - универсальный ключ.
   Каждая строка - это строки в каждой локализации, соответствующие ключу.
   Внимание: последним языком должен быть none, иначе чтение файла не будет корректным.
4. При необходимости можно обращаться к функциям: 
   NKLPLocalizer.LocalizeAllText() - обновить все тексты.
   NKLPLocalizer.GetString(key) - возвращает строку в текущей локализации по ключу. 
   NKLPLocalizer.ChangeLang(new_lang) - поменять язык.
   Для получения объекта локализатора можно использовать функцию GetLocalizer() в Scripts/FunctionExample.cs
   

[EN]

Desc:
Script finds Text objects at level and replaces keys with localized strings.
You can also get localized string by key directly.
Selected language saves to PlayerPrefs automatically.

Usage:
1. Download files and place them at root of your project.
2. Add Localizer prefab to scene you need.
3. Edit the Resources/locale.csv file:
   Each column is a language, first column is unique key.
   Each string contains localized strings of each language.
4. If required, following functions may be called:
   NKLPLocalizer.LocalizeAllText() - reload all texts.
   NKLPLocalizer.GetString(key) - returns localized string by key.
   NKLPLocalizer.ChangeLang(new_lang) - changes language.
   There is an example of getting Localizer object in GetLocalizer() function in Scripts/FunctionExample.cs
