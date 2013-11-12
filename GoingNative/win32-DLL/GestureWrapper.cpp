
#include "stdafx.h"
#include "GestureWrapper.h"
#include <shellapi.h>

using namespace Gestures;

GestureWrapper::GestureWrapper()
{}

GestureWrapper::~GestureWrapper()
{
}

BOOL CALLBACK EnumWindowsProc(HWND hWnd, LPARAM lParam) {

	DWORD dwThreadId, dwProcessId;

	if (!hWnd)
		return TRUE;		// Not a window

	dwThreadId = GetWindowThreadProcessId(hWnd, &dwProcessId);

	if (dwProcessId == lParam)
	{
		IPropertyStore* pPropStore;
		HRESULT hrReturnValue = SHGetPropertyStoreForWindow(hWnd, IID_PPV_ARGS(&pPropStore));

		if (SUCCEEDED(hrReturnValue))
		{
			PROPVARIANT var;
			var.vt = VT_BOOL;
			var.boolVal = VARIANT_TRUE;

			hrReturnValue = pPropStore->SetValue(PKEY_EdgeGesture_DisableTouchWhenFullscreen, var);
			pPropStore->Release();
		}
	}

	return TRUE;
}

void GestureWrapper::TBD(DWORD processId){
	bool x = EnumWindows(EnumWindowsProc, processId);
}

void GestureWrapper::DisableGestures(DWORD processId) {
	HWND windowHandle = GestureWrapper::GetProcessWindow(processId);
	GestureWrapper::SetGesture(windowHandle, VARIANT_TRUE);
}

void GestureWrapper::DisableGestures(HWND windowHandle) {
	GestureWrapper::SetGesture(windowHandle, VARIANT_TRUE);
}

void GestureWrapper::EnableGestures(DWORD processId) {
	HWND windowHandle = GestureWrapper::GetProcessWindow(processId);
	GestureWrapper::SetGesture(windowHandle, VARIANT_FALSE);
}

void GestureWrapper::SetGesture(HWND hWnd, VARIANT_BOOL enabled) {

	IPropertyStore* pPropStore;
	HRESULT hrReturnValue = SHGetPropertyStoreForWindow(hWnd, IID_PPV_ARGS(&pPropStore));

	if (SUCCEEDED(hrReturnValue))
	{
		PROPVARIANT var;
		var.vt = VT_BOOL;
		var.boolVal = enabled;

		hrReturnValue = pPropStore->SetValue(PKEY_EdgeGesture_DisableTouchWhenFullscreen, var);
		if (SUCCEEDED(hrReturnValue))
		{

		}

		pPropStore->Release();
	}

}

HWND GestureWrapper::GetProcessWindow(DWORD processId)
{
	bool bFound = false;
	HWND prevWindow = 0;

	while (!bFound) {
		HWND desktopWindow = GetDesktopWindow();
		if (!desktopWindow)
			break;

		HWND nextWindow = FindWindowEx(desktopWindow, prevWindow, NULL, NULL);
		if (!nextWindow)
			break;

		DWORD procId = -1;
		GetWindowThreadProcessId(nextWindow, &procId);

		if (procId == processId) {
			wchar_t windowText[1024];
			
			if (IsWindowVisible(nextWindow) 
				&& !IsIconic(nextWindow) 
				&& GetWindowText(nextWindow, (LPWSTR) windowText, sizeof(windowText) / sizeof(wchar_t))
				&& !GetParent(nextWindow))
				
				return nextWindow;
		}

		prevWindow = nextWindow;
	}

	return 0;
}