
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
	HINSTANCE hInstance;
	char String[255];
	HANDLE hProcess;

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

void GestureWrapper::TBD(DWORD processId)
{
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
	//if (!hWnd)
	//{
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
	//}
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

		// Check whether window belongs to the correct process.
		DWORD procId = -1;
		GetWindowThreadProcessId(nextWindow, &procId);

		if (procId == processId) {
			// Add additional checks. In my case, I had to bring the window to front so these checks were necessary.
			wchar_t windowText[1024];
			if (IsWindowVisible(nextWindow) && !IsIconic(nextWindow) && GetWindowText(nextWindow, (LPWSTR) windowText, sizeof(windowText) / sizeof(wchar_t))
				&& !GetParent(nextWindow))
				return nextWindow;
		}

		prevWindow = nextWindow;
	}

	return 0;
}