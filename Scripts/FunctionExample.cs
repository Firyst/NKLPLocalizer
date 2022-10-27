
private NKLPLocalizer GetLocalizer()
{
	/* 
	   Функция, которая ищет на сцене скрипт локализатора
	   Можно поместить в любой скрипт на уровне, а полученный объект скрипта локализатора записать в переменную,
	   и вызывать нужные функции, например GetString(), ChangeLang() и другие.
	   
	   Function, that searches localizer script object on current scene.
	   This function could be placed inside any script at scene, so you can get localizer object and
	   call functions such as GetString(), ChangeLang(), etc.
	*/

	NKLPLocalizer res = (NKLPLocalizer)FindObjectOfType(typeof(NKLPLocalizer));
	if (res == null)
	{
		// если не найдено
		Debug.LogError("Unable to find localizer object instance :(");
	}
	return res;
}
