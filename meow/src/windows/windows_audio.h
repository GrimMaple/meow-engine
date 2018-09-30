/* - windows_audio.h ------------------------------------------------------------------------------
* Entry point for the low-level audio for windows
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
* -------------------------------------------------------------------------------------------------
*/

#ifndef _WINDOWS_AUDIO_H_
#define _WINDOWS_AUDIO_H_

#include <Windows.h>
#include <mmsystem.h>

typedef void ( __stdcall *audio_callback )( );

typedef struct audio_device
{
	HWAVEOUT         waveOut;
	CRITICAL_SECTION crt;
	WAVEHDR          hdrs[2];
	bool             block_done;
	audio_callback   callback;
} audio_device;

void Cycle( audio_device* );

#endif