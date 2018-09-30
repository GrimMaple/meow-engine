/* - vsync.c --------------------------------------------------------------------------------------
* Vsync control implementation under Windows
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016
* -------------------------------------------------------------------------------------------------
*/

#include "../vsync.h"
#include "windows.h"
#include <wglext.h>
#include <glext.h>

#define LOAD_FUNC( x, STR ) if( !x ) x = wglGetProcAddress( STR )

PFNWGLGETEXTENSIONSSTRINGEXTPROC wglGetExtensionsString = 0;
PFNWGLSWAPINTERVALEXTPROC        wglSwapInterval        = 0;
PFNWGLGETSWAPINTERVALEXTPROC     wglGetSwapInterval     = 0;

bool vsyncEnabled = 0;
bool allowed = 0;

void GetFuncs( )
{
	LOAD_FUNC( wglGetExtensionsString, "wglGetExtensionsStringEXT" );
	LOAD_FUNC( wglSwapInterval,        "wglSwapIntervalEXT"        );
	LOAD_FUNC( wglGetSwapInterval,     "wglGetSwapIntervalEXT"     );
}

void CheckVSync( )
{
	allowed = (bool)strstr( wglGetExtensionsString( ), "WGL_EXT_swap_control" );
}

void MEOW_API vsync_set( bool enabled )
{
	GetFuncs( );
	CheckVSync( );
	if ( allowed )
	{
		wglSwapInterval( enabled ? 1 : 0 );
		vsyncEnabled = enabled;
	}
}

dword MEOW_API vsync_get( )
{
	if ( allowed )
		return wglGetSwapInterval( );
	return -1;
}