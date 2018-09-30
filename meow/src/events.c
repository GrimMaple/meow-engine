/* - events.c --------------------------------------------------------------------------------------
* Basic eventing system implementation
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2016
* --------------------------------------------------------------------------------------------------
*/

#include "events.h"

#define EVENTS_COUNT 13

event_t events[ EVENTS_COUNT ];

void RaiseEvent( dword type, int32 fparam, int32 sparam )
{
	if ( type >= EVENTS_COUNT )
		return;
	if ( events[ type ] )
		events[ type ]( fparam, sparam );
}

void MEOW_API events_subscribe( dword type, event_t evt )
{
	if ( type >= EVENTS_COUNT )
		return;
	events[ type ] = evt;
}

void MEOW_API events_unsubscribe( dword type )
{
	if ( type >= EVENTS_COUNT )
		return;
	events[ type ] = 0;
}