
private NKLPLocalizer GetLocalizer()
{
	/* 
	   �������, ������� ���� �� ����� ������ ������������
	   ����� ��������� � ����� ������ �� ������, � ���������� ������ ������� ������������ �������� � ����������,
	   � �������� ������ �������, �������� GetString(), ChangeLang() � ������.
	   
	   Function, that searches localizer script object on current scene.
	   This function could be placed inside any script at scene, so you can get localizer object and
	   call functions such as GetString(), ChangeLang(), etc.
	*/

	NKLPLocalizer res = (NKLPLocalizer)FindObjectOfType(typeof(NKLPLocalizer));
	if (res == null)
	{
		// ���� �� �������
		Debug.LogError("Unable to find localizer object instance :(");
	}
	return res;
}
