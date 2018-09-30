/* - window_shared.h -------------------------------------------------------------------------------
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

#ifndef _WINDOW_SHARED_H_
#define _WINDOW_SHARED_H_

#include "api.h"

/*
 *	Non-implemented
 *
 *	Those functions are not implemented by window_shared
 *	Thus they must be implemented in a specific platform's c file
 */
void   MEOW_API window_set_text( string );
void   ProcessMessages( void );
void   End( void );
void   SendSizeMessage( dword, dword );
void   WindowResize( dword, dword );
void   SetFullscreen( bool );
dword  WindowInit( void );
dword   WindowCleanup( void );


/*
 *	Implemented
 *
 *	Those functions are implemented by window_shared
 *	No need to re-implement them again
 */

void  ChangeSize( dword, dword );
dword MEOW_API window_cleanup( void );
dword MEOW_API window_create( void );
void  MEOW_API window_resize( dword, dword );
void  MEOW_API window_get_size( dword*, dword* );
dword MEOW_API window_fps( void );
void  MEOW_API window_run( void );
bool  MEOW_API window_is_fullscreen( void );
void  MEOW_API window_break_cycle( void );
void  MEOW_API window_set_fullscreen( bool );
dword MEOW_API window_get_redraw_interval( void );
dword MEOW_API window_get_refresh_interval( void );
void  MEOW_API window_set_redraw_interval( dword );
void  MEOW_API window_set_refresh_interval( dword );


#endif