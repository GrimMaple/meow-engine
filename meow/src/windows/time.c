/* - time.c ----------------------------------------------------------------------------------------
* Windows implementation of system time
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
* --------------------------------------------------------------------------------------------------
*/

#include <Windows.h>
#include <mmsystem.h>
#include <stdio.h>

#include "../time.h"

double freq = 0.0;
qword CounterStart;

void TimeInit( void )
{
	LARGE_INTEGER li;
	QueryPerformanceFrequency( &li );
	freq = (double)li.QuadPart / 1000.0;
	QueryPerformanceCounter( &li );
	CounterStart = li.QuadPart;
}

dword CurrentTime( void )
{
	LARGE_INTEGER li;
	QueryPerformanceCounter( &li );
	dword ret = (dword)(( (double)li.QuadPart - (double)CounterStart ) / freq);
	return ret;
}