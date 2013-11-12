#pragma once

namespace WinMD
{
    public ref class GestureHelper sealed
    {
    public:
		GestureHelper();
		LONG32 DisableGestures();
		LONG32 EnableGestures();
    };
}