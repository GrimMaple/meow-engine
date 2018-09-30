/* - time.h ----------------------------------------------------------------------------------------
* Cross-platform implementation of system time
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
*
* NOTE:
*
*     This file requires some platform-specific implementations to be done
*     
*     The time submodule assumes that the return value is a DWORD containing
*     amount of milliseconds passed since the program startup.
* --------------------------------------------------------------------------------------------------
*/

#include "api.h"

void  TimeInit( void );
dword CurrentTime( void );