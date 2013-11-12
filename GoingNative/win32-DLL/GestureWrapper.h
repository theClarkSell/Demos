#pragma once

#include <propsys.h>
#include <propkey.h>

namespace Gestures
{
	class __declspec(dllexport) GestureWrapper sealed
	{
	public:
		GestureWrapper();
		~GestureWrapper();
		void DisableGestures(DWORD processId);
		void DisableGestures(HWND windowHandle);
		void EnableGestures(DWORD processId);
		void TBD(DWORD processId);
	private:
		void SetGesture(HWND hWnd, VARIANT_BOOL enabled);
		HWND GetProcessWindow(DWORD processId);
	};
}