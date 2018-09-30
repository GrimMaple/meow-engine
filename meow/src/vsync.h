/* - vsync.h --------------------------------------------------------------------------------------
* Vsync control - checks if VSync can be manipulated and manipulates if so
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016
* -------------------------------------------------------------------------------------------------
*/

#ifndef _VSYNC_H_
#define _VSYNC_H_

#include "api.h"

void  MEOW_API vsync_set( bool );
dword MEOW_API vsync_get( );

#endif

