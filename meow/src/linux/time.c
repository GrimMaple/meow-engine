/* - time.c ----------------------------------------------------------------------------------------
* Linux implementation of system time
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
* --------------------------------------------------------------------------------------------------
*/

#include <time.h>
#include <stdio.h>

#include "../time.h"

struct timespec begin;

void TimeInit( void )
{
	clock_gettime( CLOCK_REALTIME, &begin );
}

dword CurrentTime( void )
{
	struct timespec end;
	clock_gettime( CLOCK_REALTIME, &end );
	dword res = (end.tv_sec - begin.tv_sec) * 1000 + (end.tv_nsec - begin.tv_nsec) / 1000000;
	return res;
}