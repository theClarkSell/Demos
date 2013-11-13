#pragma once

namespace WinMD
{
    public ref class GestureHelper sealed
    {
    public:
		GestureHelper();
		LONG32 DisableViaDirect();
		LONG32 DisableViaEnum();
    };
}