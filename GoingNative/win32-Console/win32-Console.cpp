// win32-Console.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "..\win32-DLL\GestureWrapper.h"

#include <iostream>

using namespace std;
using namespace Gestures;

int _tmain(int argc, _TCHAR* argv[])
{
	//for command line purposes.

	bool runHandle = false;
	HWND hWnd;
	GestureWrapper *gestureWrapper = new GestureWrapper();

	if (runHandle) 	{

		hWnd = GetConsoleWindow();

		if (!hWnd) {
			cout << "no window handle";
		}

		gestureWrapper -> DisableGestures(hWnd);
	}
	else {
		DWORD processId = GetCurrentProcessId();
		gestureWrapper -> DisableGestures(processId);
	}

	
	cout << "enter something to stop";
	int foo;
	cin >> foo;

	return 0;
}