/* - window_shared.c -------------------------------------------------------------------------------
* Cross-platform implementation of window lifecycle
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016-2017
*
* NOTE:
*
*     This file requires some platform-specific implementations to be done
* --------------------------------------------------------------------------------------------------
*/

#include "window_shared.h"
#include "events.h"
#include "renderer.h"
#include "audio.h"
#include "time.h"
#include "extensions.h"
#include "mio/joystick.h"

dword lastProcessTime = 0;
dword lastDrawTime = 0;
bool  shouldExit = 0;
dword lastFps;
dword fps;
dword width, height;
bool  fullscreen = 0;

dword refreshTime = 16;
dword redrawTime = 16;

void ChangeSize( dword w, dword h )
{
	width = w;
	height = h;
	RaiseEvent( EVT_WND_SIZE, w, h );
}

dword MEOW_API window_create( void )
{
	AudioInit( );
	//return WindowInit( );
	WindowInit( );
	TimeInit( );
	RendererSetup( width, height );
	ExtensionsInit( );
	return 0;
}

dword MEOW_API window_cleanup( void )
{
	AudioCleanup( );
	return WindowCleanup( );
}

dword  MEOW_API window_get_redraw_interval( void )
{
	return redrawTime;
}

dword  MEOW_API window_get_refresh_interval( void )
{
	return refreshTime;
}

void MEOW_API window_set_redraw_interval( dword in )
{
	redrawTime = in;
}

void MEOW_API window_set_refresh_interval( dword in )
{
	refreshTime = in;
}

void MEOW_API window_resize( dword w, dword h )
{
	width = w;
	height = h;

	SendSizeMessage( w, h );
}

void MEOW_API window_get_size( dword *w, dword *h )
{
	*w = width;
	*h = height;
}

dword MEOW_API window_fps( void )
{
	return fps;
}

void MEOW_API window_run( void )
{
	lastProcessTime = CurrentTime( );
	dword tmpFps = 0;


	while ( !shouldExit )
	{
		ProcessMessages( );

		// Count elapsed time and pass it to Draw and Update functions
		dword currentTime = CurrentTime( );
		dword sleepTime = ( refreshTime - currentTime + lastProcessTime > redrawTime - currentTime + lastDrawTime ) ? redrawTime - currentTime + lastDrawTime : refreshTime - currentTime + lastProcessTime;
		/*if ( sleepTime < refreshTime && sleepTime < redrawTime && sleepTime > 0)
		{
			Sleep( sleepTime );
			continue;
		}
		if ( currentTime - lastProcessTime >= refreshTime )
		{
			RaiseEvent( EVT_UPDATE, currentTime - lastProcessTime, 133 );
			lastProcessTime = currentTime;
		}
		if ( currentTime - lastDrawTime >= redrawTime )
		{
			Begin( );
			RaiseEvent( EVT_DRAW, currentTime - lastProcessTime, 133 );
			End( );
			lastDrawTime = currentTime;
			tmpFps++;
		}*/
		AudioCycle( );
		// FIXME: add linux joystick support, or just empty methods
		#ifdef WIN32
		joystick_cycle( currentTime - lastProcessTime );
		#endif
		RaiseEvent( EVT_UPDATE, currentTime - lastProcessTime, 0 );
		Begin( );
		RaiseEvent( EVT_DRAW, currentTime - lastProcessTime, 0 );
		End( );
		lastProcessTime = currentTime;
		tmpFps++;
		if ( currentTime - lastFps >= 1000 )
		{
			fps = tmpFps;
			lastFps = currentTime;
			tmpFps = 0;
		}
	}
}

void MEOW_API window_break_cycle( void )
{
	shouldExit = 1;
}

bool MEOW_API window_is_fullscreen( void )
{
	return fullscreen;
}

void MEOW_API window_set_fullscreen( bool fs )
{
	fullscreen = fs;
	SetFullscreen( fs );
}