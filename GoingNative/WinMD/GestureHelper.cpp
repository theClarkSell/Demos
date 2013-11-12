// Class1.cpp


#include "pch.h"
#include "GestureHelper.h"
#include "..\win32-DLL\GestureWrapper.h"

using namespace WinMD;
using namespace Platform;
using namespace Gestures;


GestureHelper::GestureHelper()
{
}

// disables by calling enum windows and finding the process
LONG32 GestureHelper::EnableGestures() {

	DWORD processId = GetCurrentProcessId();

	GestureWrapper *x = new GestureWrapper();
	x->TBD(processId);
	
	return processId;
}

//disables by 
LONG32 GestureHelper::DisableGestures() {
	DWORD processId = GetCurrentProcessId();

	GestureWrapper *x = new GestureWrapper();
	x->DisableGestures(processId);

	return processId;
}