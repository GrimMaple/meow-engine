/* - audio.h --------------------------------------------------------------------------------------
* Entry point for the low-level audio system
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
* -------------------------------------------------------------------------------------------------
*/

#ifndef _AUDIO_H_
#define _AUDIO_H_

#include "api.h"

#ifdef _WINDOWS
#include "windows\windows_audio.h"
#else
#include "linux/linux_audio.h"
#endif

typedef struct audio_device* audio_device_ptr;

audio_device_ptr MEOW_API audio_open( int32, dword, dword, dword, dword );
void             MEOW_API audio_close( audio_device* );
void             MEOW_API audio_write( audio_device*, void*, dword );
void             MEOW_API audio_stop( audio_device* );
void             MEOW_API audio_subscribe( audio_device*, audio_callback );

void                      AudioInit( );
void                      AudioCycle( );
void                      AudioCleanup( );

void                      AudioInsert( audio_device* );
void                      AudioRemove( audio_device* );

#endif