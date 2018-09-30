/* - events.h --------------------------------------------------------------------------------------
* Basic eventing system to pass low-level callback to higher level core
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016
* --------------------------------------------------------------------------------------------------
*/

#ifndef _EVENTS_H_
#define _EVENTS_H_

#include "api.h"

#define EVT_DRAW         0
#define EVT_UPDATE       1
#define EVT_KEY_DOWN     2
#define EVT_KEY_UP       3
#define EVT_LMB_DOWN     4
#define EVT_LMB_UP       5
#define EVT_RMB_DOWN     6
#define EVT_RMB_UP       7
#define EVT_MOUSE_MOVE   8
#define EVT_JOY_BTN_UP   9
#define EVT_JOY_BTN_DOWN 10
#define EVT_WND_SIZE     11
#define EVT_JOY_AXIS     12

typedef void( __stdcall *event_t )( int32, int32 );

void RaiseEvent( dword, int32, int32 );

void MEOW_API events_subscribe( dword, event_t );
void MEOW_API events_unsubscribe( dword );

#endif