#include "../vsync.h"

bool allowed = 0;
bool vsyncEnabled = 0;

void GetFuncs( void )
{
	// TODO: implement
}

void CheckVSync( void )
{
	// TODO: implement
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